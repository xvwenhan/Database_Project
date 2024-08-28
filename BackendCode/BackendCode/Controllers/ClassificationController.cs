using BackendCode.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using BackendCode.DTOs.Store;
using BackendCode.Models;
using Alipay.AopSdk.Core.Domain;
using BackendCode.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace ClassificationController.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassificationController : ControllerBase
    {
        private readonly YourDbContext _dbContext;
        private readonly ILogger<ClassificationController> _logger;

        public ClassificationController(YourDbContext dbContext, ILogger<ClassificationController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        [HttpGet("GetCategoryByName")]
        public async Task<IActionResult> GetCategoryByName(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
            {
                return BadRequest("Category name is required.");
            }

            try
            {
                var category = await _dbContext.CATEGORYS
                    .Where(c => c.CATEGORY_NAME == categoryName)
                    .Select(c => new
                    {
                        c.CATEGORY_PIC,
                        c.CATEGORY_DESCRIPTION
                    })
                    .FirstOrDefaultAsync();

                if (category == null)
                {
                    return NotFound("Category not found.");
                }

                return Ok(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting category by name.");
                return StatusCode(500, "Internal server error.");
            }
        }
        [HttpGet("getProductsByTag")]
        //[Authorize]
        public async Task<IActionResult> GetProductsByTag(string tag)
        {
            try
            {
                // 如果tag是具体的数值
                if (int.TryParse(tag, out int tagNumber) && tagNumber >= 1 && tagNumber <= 19)
                {
                    var products = await _dbContext.PRODUCTS
                        .Where(p => p.TAG == tag)
                        .Select(p => new
                        {
                            ProductId = p.PRODUCT_ID,
                            ProductName = p.PRODUCT_NAME,
                            ProductPrice = p.PRODUCT_PRICE,
                            SubCategoryName = _dbContext.SUB_CATEGORYS
                                .Where(sc => sc.SUBCATEGORY_ID == p.TAG)
                                .Select(sc => sc.SUBCATEGORY_NAME)
                                .FirstOrDefault(),
                            Images = _dbContext.PRODUCT_IMAGES
                                .Where(pi => pi.PRODUCT_ID == p.PRODUCT_ID)
                                .Select(pi => new ImageModel { ImageId = pi.IMAGE_ID })
                                .ToList()
                        })
                        .ToListAsync();

                    if (products == null || !products.Any())
                    {
                        return NotFound("No products found for the given tag.");
                    }

                    return Ok(products);
                }

                // 如果tag是分类名（如 "服装"、"首饰"、"工艺品"、"家具"、"小物件"）
                var validCategories = new[] { "服装", "首饰", "工艺品", "家具", "小物件" };
                if (validCategories.Contains(tag))
                {
                    var subCategoryIds = await _dbContext.SUB_CATEGORYS
                        .Where(sc => sc.CATEGORY_NAME == tag)
                        .Select(sc => sc.SUBCATEGORY_ID)
                        .ToListAsync();

                    var products = await _dbContext.PRODUCTS
                        .Where(p => subCategoryIds.Contains(p.TAG))
                        .Select(p => new
                        {
                            ProductId = p.PRODUCT_ID,
                            ProductName = p.PRODUCT_NAME,
                            ProductPrice = p.PRODUCT_PRICE,
                            SubCategoryName = _dbContext.SUB_CATEGORYS
                                .Where(sc => sc.SUBCATEGORY_ID == p.TAG)
                                .Select(sc => sc.SUBCATEGORY_NAME)
                                .FirstOrDefault(),
                            Images = _dbContext.PRODUCT_IMAGES
                                .Where(pi => pi.PRODUCT_ID == p.PRODUCT_ID)
                                .Select(pi => new ImageModel { ImageId = pi.IMAGE_ID })
                                .ToList()
                        })
                        .ToListAsync();

                    if (products == null || !products.Any())
                    {
                        return NotFound("No products found for the given category.");
                    }

                    return Ok(products);
                }

                // 如果tag不符合预期的取值范围
                return BadRequest("Invalid tag value. Please provide a valid tag.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}
