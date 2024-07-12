using BackendCode.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendCode.DTOs.Search;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Search.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly YourDbContext _dbContext;
        private readonly ILogger<SearchController> _logger;

        public SearchController(YourDbContext dbContext, ILogger<SearchController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        // 搜索
        [HttpGet("search")]
        public async Task<IActionResult> Search(bool isStoreSearch, string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return BadRequest("Keyword is required.");
            }

            if (isStoreSearch)
            {
                // 分解关键词，用于包含字符的模糊匹配
                var keywordArray = keyword.ToCharArray();
                var likePattern = string.Join("%", keywordArray);

                var stores = await _dbContext.STORES
                    .Where(s => EF.Functions.Like(s.STORE_NAME, $"%{keyword}%") ||
                                EF.Functions.Like(s.STORE_NAME, $"%{likePattern}%"))
                    .ToListAsync();

                var storeDtos = stores.Select(s => new SearchStoresDTO
                {
                    StoreId = s.ACCOUNT_ID,
                    StoreName = s.STORE_NAME,
                    StoreScore = s.STORE_SCORE
                })
                .OrderByDescending(s => s.StoreName == keyword)  // 完全匹配优先
                .ThenByDescending(s => s.StoreName.StartsWith(keyword))  // 前缀匹配次优
                .ThenBy(s => s.StoreName.Contains(keyword))      // 部分匹配再次
                .ThenBy(s => s.StoreName.IndexOf(keyword))       // 名称包含关键字次之
                .ToList();

                if (storeDtos == null || !storeDtos.Any())
                {
                    return NotFound("No stores found for the given keyword.");
                }

                return Ok(storeDtos);
            }
            else
            {
                // 分解关键词，用于包含字符的模糊匹配
                var keywordArray = keyword.ToCharArray();
                var likePattern = string.Join("%", keywordArray);

                var products = await _dbContext.PRODUCTS
                    .Where(p => EF.Functions.Like(p.PRODUCT_NAME, $"%{keyword}%") ||
                                EF.Functions.Like(p.PRODUCT_NAME, $"%{likePattern}%"))
                    .ToListAsync();

                var productDtos = products.Select(p => new SearchProductsDTO
                {
                    ProductName=p.PRODUCT_NAME,
                    StoreId = p.ACCOUNT_ID,
                    ProductId = p.PRODUCT_ID,
                    ProductPrice = p.PRODUCT_PRICE,
                    SaleOrNot = p.SALE_OR_NOT,
                    Tag = p.TAG,
                    //Pic=p.PRODUCT_PIC
                })
                .OrderByDescending(p => p.ProductId == keyword)  // 完全匹配优先
                .ThenByDescending(p => p.Tag.StartsWith(keyword))  // 前缀匹配次优
                .ThenBy(p => p.Tag.Contains(keyword))             // 部分匹配再次
                .ThenBy(p => p.Tag.IndexOf(keyword))              // 名称包含关键字次之
                .ToList();

                if (productDtos == null || !productDtos.Any())
                {
                    return NotFound("No products found for the given keyword.");
                }

                return Ok(productDtos);
            }
        }
    }
}
