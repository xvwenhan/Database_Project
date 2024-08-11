﻿using BackendCode.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace NaviSearchController.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NaviSearchController : ControllerBase
    {
        private readonly YourDbContext _dbContext;
        private readonly ILogger<NaviSearchController> _logger;

        public NaviSearchController(YourDbContext dbContext, ILogger<NaviSearchController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(string keyword, int type)
        {
            if (string.IsNullOrEmpty(keyword) || (type != 0 && type != 1))
            {
                return BadRequest("Keyword and valid type (0 or 1) are required.");
            }

            // 分解关键词，用于包含字符的模糊匹配
            var keywordArray = keyword.ToCharArray();
            var likePattern = string.Join("%", keywordArray);

            if (type == 0)
            {
                // 搜索商品
                var products = await _dbContext.PRODUCTS
                    .Where(p => EF.Functions.Like(p.PRODUCT_NAME, $"%{keyword}%") ||
                                EF.Functions.Like(p.PRODUCT_NAME, $"%{likePattern}%"))
                    .ToListAsync();

                var productDtos = products.Select(p => new
                {
                    ProductId = p.PRODUCT_ID,
                    ProductName = p.PRODUCT_NAME,
                    ProductPrice = p.PRODUCT_PRICE,
                    SaleOrNot = p.SALE_OR_NOT,
                    Tag = p.TAG,
                    Description = p.DESCRIBTION,
                    ProductPic = p.PRODUCT_PIC != null ? Convert.ToBase64String(p.PRODUCT_PIC) : null,
                    StoreId = p.ACCOUNT_ID,
                    StoreName = _dbContext.STORES.FirstOrDefault(s => s.ACCOUNT_ID == p.ACCOUNT_ID)?.STORE_NAME,
                    StoreScore = _dbContext.STORES.FirstOrDefault(s => s.ACCOUNT_ID == p.ACCOUNT_ID)?.STORE_SCORE
                })
                .OrderByDescending(p => p.ProductName == keyword)
                .ThenByDescending(p => p.ProductName.StartsWith(keyword))
                .ThenBy(p => p.ProductName.Contains(keyword))
                .ThenBy(p => p.ProductName.IndexOf(keyword))
                .ToList();

                if (!productDtos.Any())
                {
                    return NotFound("No products found for the given keyword.");
                }

                return Ok(productDtos);
            }
            else if (type == 1)
            {
                // 搜索商家
                var stores = await _dbContext.STORES
                    .Where(s => EF.Functions.Like(s.STORE_NAME, $"%{keyword}%") ||
                                EF.Functions.Like(s.STORE_NAME, $"%{likePattern}%"))
                    .ToListAsync();

                var storeDtos = stores.Select(s => new
                {
                    StoreId = s.ACCOUNT_ID,
                    StoreName = s.STORE_NAME,
                    StoreScore = s.STORE_SCORE,
                    StorePhoto =  _dbContext.ACCOUNTS
                        .Where(p => p.ACCOUNT_ID == s.ACCOUNT_ID)
                        .Select(p => p.PHOTO) // 假设 Photo 是账户表中的字段
                        .FirstOrDefaultAsync(),

                    HomeProducts = _dbContext.PRODUCTS
                        .Where(p => p.ACCOUNT_ID == s.ACCOUNT_ID && !p.SALE_OR_NOT)
                        .Take(4)
                        .Select(p => new
                        {
                            ProductId = p.PRODUCT_ID,
                            ProductName = p.PRODUCT_NAME,
                            ProductPrice = p.PRODUCT_PRICE,
                            ProductPic = p.PRODUCT_PIC != null ? Convert.ToBase64String(p.PRODUCT_PIC) : null
                        }).ToList()
                })
                .OrderByDescending(s => s.StoreName == keyword)
                .ThenByDescending(s => s.StoreName.StartsWith(keyword))
                .ThenBy(s => s.StoreName.Contains(keyword))
                .ThenBy(s => s.StoreName.IndexOf(keyword))
                .ToList();

                if (!storeDtos.Any())
                {
                    return NotFound("No stores found for the given keyword.");
                }

                return Ok(storeDtos);
            }

            return BadRequest("Invalid search type.");
        }
    }
}

