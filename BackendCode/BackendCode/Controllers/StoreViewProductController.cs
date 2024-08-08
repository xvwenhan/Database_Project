using BackendCode.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using BackendCode.DTOs.ProductDTO;
using BackendCode.Models;

namespace StoreViewProductController.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoreViewProductController : ControllerBase
    {
        private readonly YourDbContext _dbContext;
        private readonly ILogger<StoreViewProductController> _logger;

        public StoreViewProductController(YourDbContext dbContext, ILogger<StoreViewProductController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        //Get接口，传入store_id, view_type，view_type1 2 3分别代表全部，售出，未售
        [HttpGet("GetProductsByStoreIdAndViewType")]
        public async Task<IActionResult> GetProductsByStoreIdAndViewType(string StoreId, int ViewType)
        {
            if (string.IsNullOrEmpty(StoreId))
            {
                return BadRequest("Store ID is required.");
            }

            if (ViewType < 1 || ViewType > 3)
            {
                return BadRequest("Invalid view type. Allowed values are 1, 2, or 3.");
            }

            try
            {
                IQueryable<PRODUCT> query = _dbContext.PRODUCTS.Where(p => p.ACCOUNT_ID == StoreId);

                if (ViewType == 2)
                {
                    query = query.Where(p => p.SALE_OR_NOT == true);
                }
                else if (ViewType == 3)
                {
                    query = query.Where(p => p.SALE_OR_NOT == false);
                }

                var products = await query.ToListAsync();

                var productDTOs = products.Select(p => new ProductDTO
                {
                    ProductId = p.PRODUCT_ID,
                    ProductName = p.PRODUCT_NAME,
                    ProductPrice = p.PRODUCT_PRICE,
                    SaleOrNot = p.SALE_OR_NOT,
                    Tag = p.TAG,
                    Description = p.DESCRIBTION,
                    StoreTag = p.STORE_TAG,
                    ProductPic = p.PRODUCT_PIC != null ? Convert.ToBase64String(p.PRODUCT_PIC) : null
                }).ToList();

                return Ok(productDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting products by store ID and view type.");
                return StatusCode(500, "Internal server error.");
            }
        }

        //Get接口传入storeid keyword匹配对应商品
        [HttpGet("search")]
        public async Task<IActionResult> Search(string storeId, string keyword)
        {
            if (string.IsNullOrEmpty(storeId) || string.IsNullOrEmpty(keyword))
            {
                return BadRequest("Store ID and Keyword are required.");
            }

            // 分解关键词，用于包含字符的模糊匹配
            var keywordArray = keyword.ToCharArray();
            var likePattern = string.Join("%", keywordArray);

            var products = await _dbContext.PRODUCTS
                .Where(p => p.ACCOUNT_ID == storeId &&
                            (EF.Functions.Like(p.PRODUCT_NAME, $"%{keyword}%") ||
                             EF.Functions.Like(p.PRODUCT_NAME, $"%{likePattern}%")))
                .ToListAsync();

            var productDtos = products.Select(p => new ProductDTO
            {
                ProductId = p.PRODUCT_ID,
                ProductName = p.PRODUCT_NAME,
                ProductPrice = p.PRODUCT_PRICE,
                SaleOrNot = p.SALE_OR_NOT,
                Tag = p.TAG,
                Description = p.DESCRIBTION,
                StoreTag = p.STORE_TAG,
                ProductPic = p.PRODUCT_PIC != null ? Convert.ToBase64String(p.PRODUCT_PIC) : null
            })
            .OrderByDescending(p => p.ProductName == keyword)  // 完全匹配优先
            .ThenByDescending(p => p.ProductName.StartsWith(keyword))  // 前缀匹配次优
            .ThenBy(p => p.ProductName.Contains(keyword))             // 部分匹配再次
            .ThenBy(p => p.ProductName.IndexOf(keyword))              // 名称包含关键字次之
            .ToList();

            if (!productDtos.Any())
            {
                return NotFound("No products found for the given store ID and keyword.");
            }

            return Ok(productDtos);
        }

        //Get接口传入storeid storetag匹配对应商品
        [HttpGet("searchByStoreTag")]
        public async Task<IActionResult> SearchByTag(string storeId, string storeTag)
        {
            if (string.IsNullOrEmpty(storeId) || string.IsNullOrEmpty(storeTag))
            {
                return BadRequest("Store ID and Store Tag are required.");
            }

            var products = await _dbContext.PRODUCTS
                .Where(p => p.ACCOUNT_ID == storeId && p.STORE_TAG == storeTag)
                .Select(p => new ProductDTO
                {
                    ProductId = p.PRODUCT_ID,
                    ProductName = p.PRODUCT_NAME,
                    ProductPrice = p.PRODUCT_PRICE,
                    SaleOrNot = p.SALE_OR_NOT,
                    Tag = p.TAG,
                    Description = p.DESCRIBTION,
                    StoreTag = p.STORE_TAG,
                    ProductPic = p.PRODUCT_PIC != null ? Convert.ToBase64String(p.PRODUCT_PIC) : null
                })
                .ToListAsync();

            if (!products.Any())
            {
                return NotFound("No products found for the given store ID and store tag.");
            }

            return Ok(products);
        }

        //Put接口传入product_id和store_id和更改属性及值
        [HttpPut("editProduct")]
        public async Task<IActionResult> EditProduct(string storeId, [FromBody] Product2DTO updatedProduct)
        {
            var product = await _dbContext.PRODUCTS.FirstOrDefaultAsync(p => p.PRODUCT_ID == updatedProduct.ProductId && p.ACCOUNT_ID == storeId);
            if (product == null)
            {
                return NotFound("Product not found or does not belong to the store.");
            }

            if (!string.IsNullOrEmpty(updatedProduct.ProductName)) product.PRODUCT_NAME = updatedProduct.ProductName;
            //if (updatedProduct.ProductPrice.HasValue) 
                product.PRODUCT_PRICE = updatedProduct.ProductPrice;
            if (!string.IsNullOrEmpty(updatedProduct.Tag)) product.TAG = updatedProduct.Tag;
            if (!string.IsNullOrEmpty(updatedProduct.Description)) product.DESCRIBTION = updatedProduct.Description;

            if (!string.IsNullOrEmpty(updatedProduct.ProductPic))
            {
                try
                {
                    product.PRODUCT_PIC = Convert.FromBase64String(updatedProduct.ProductPic);
                }
                catch (FormatException)
                {
                    return BadRequest("Invalid Base64 string for PRODUCT_PIC.");
                }
            }

            if (!string.IsNullOrEmpty(updatedProduct.StoreTag)) product.STORE_TAG = updatedProduct.StoreTag;

            _dbContext.PRODUCTS.Update(product);
            await _dbContext.SaveChangesAsync();

            return Ok("Product updated successfully.");
        }

        //delete接口，确认是未删除商品后删除该商品
        [HttpDelete("deleteProducts")]
        public async Task<IActionResult> DeleteProducts([FromBody] List<string> productIds, string storeId)
        {
            if (productIds == null || productIds.Count == 0)
            {
                return BadRequest("Product IDs are required.");
            }

            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var products = await _dbContext.PRODUCTS
                        .Where(p => productIds.Contains(p.PRODUCT_ID) && p.ACCOUNT_ID == storeId)
                        .ToListAsync();

                    if (products.Count != productIds.Count)
                    {
                        return NotFound("One or more products not found or do not belong to the store.");
                    }

                    var soldProducts = products.Where(p => p.SALE_OR_NOT == true).ToList();
                    if (soldProducts.Any())
                    {
                        return BadRequest("One or more products have been sold and cannot be deleted.");
                    }

                    foreach (var product in products)
                    {
                        // 删除 MARKET_PRODUCTS 表中对应 productId 的记录
                        var marketProducts = await _dbContext.MARKET_PRODUCTS
                            .Where(ms => ms.PRODUCT_ID == product.PRODUCT_ID)
                            .ToListAsync();
                        _dbContext.MARKET_PRODUCTS.RemoveRange(marketProducts);

                        // 删除 BUYER_PRODUCT_BOOKMARK 表中对应 productId 的记录
                        var bUYER_PRODUCT_BOOKMARK = await _dbContext.BUYER_PRODUCT_BOOKMARKS
                            .Where(ms => ms.PRODUCT_ID == product.PRODUCT_ID)
                            .ToListAsync();
                        _dbContext.BUYER_PRODUCT_BOOKMARKS.RemoveRange(bUYER_PRODUCT_BOOKMARK);

                        // 查找 ORDERS 表中对应 productId 的订单
                        var orders = await _dbContext.ORDERS
                            .Where(o => o.PRODUCT_ID == product.PRODUCT_ID)
                            .ToListAsync();

                        foreach (var order in orders)
                        {
                            // 删除 RETURN 表中对应 orderId 的记录
                            var returns = await _dbContext.RETURNS
                                .Where(r => r.ORDER_ID == order.ORDER_ID)
                                .ToListAsync();
                            _dbContext.RETURNS.RemoveRange(returns);

                            // 删除 ORDERS 表中的记录
                            _dbContext.ORDERS.Remove(order);
                        }

                        // 删除 PRODUCTS 表中的记录
                        _dbContext.PRODUCTS.Remove(product);
                    }

                    // 保存更改并提交事务
                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return Ok("Products deleted successfully.");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
        }



        //post接口，新建商品
        [HttpPost("addProduct")]
        public async Task<IActionResult> AddProduct(string storeId, [FromBody] Product1DTO newProduct)
        {
            // 生成唯一的 PRODUCT_ID
            string productId;
            do
            {
                productId = "P" + new Random().Next(1000000, 9999999).ToString();
            } while (await _dbContext.PRODUCTS.AnyAsync(p => p.PRODUCT_ID == productId));

            var product = new PRODUCT
            {
                PRODUCT_ID = productId,
                PRODUCT_NAME = newProduct.ProductName,
                PRODUCT_PRICE = newProduct.ProductPrice,
                SALE_OR_NOT = false, // 默认值
                TAG = newProduct.Tag,
                DESCRIBTION = newProduct.Description,
                ACCOUNT_ID = storeId,
                STORE_TAG = newProduct.StoreTag
            };

            if (!string.IsNullOrEmpty(newProduct.ProductPic))
            {
                try
                {
                    product.PRODUCT_PIC = Convert.FromBase64String(newProduct.ProductPic);
                }
                catch (FormatException)
                {
                    return BadRequest("Invalid Base64 string for PRODUCT_PIC.");
                }
            }

            _dbContext.PRODUCTS.Add(product);
            await _dbContext.SaveChangesAsync();

            return Ok("Product added successfully.");
        }


    }
}

