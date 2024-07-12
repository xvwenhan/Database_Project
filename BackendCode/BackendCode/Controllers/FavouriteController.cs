using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendCode.Data;
using BackendCode.DTOs.Favourite;

namespace Favourite.Controllers
{
    [ApiController]
    [Route("api/Favourite/[controller]")]
    public class FavouriteController : ControllerBase
    {
        private readonly YourDbContext _dbContext;

        public FavouriteController(YourDbContext context)
        {
            _dbContext = context;
        }

        [HttpGet("GetFavoriteProducts")]
        public async Task<IActionResult> GetFavoriteProductsAsync(string userId)
        {
            // 获取用户喜欢的商品ID
            var favoriteProductIds = await _dbContext.BUYER_PRODUCT_BOOKMARKS
                .Where(bp => bp.BUYER_ACCOUNT_ID == userId)
                .Select(bp => bp.PRODUCT_ID)
                .ToListAsync();

            // 通过商品ID获取商品详细信息
            var favoriteProducts = await _dbContext.PRODUCTS
                .Where(p => favoriteProductIds.Contains(p.PRODUCT_ID))
                .ToListAsync();

            if (favoriteProducts == null || !favoriteProducts.Any())
            {
                return NotFound("No products found for the given user ID.");
            }

            var productDtos = favoriteProducts.Select(p => new FavouriteProductsDTO
            {
                BuyerId = userId,
                StoreId = p.ACCOUNT_ID,
                ProductId=p.PRODUCT_ID,
                ProductPrice=p.PRODUCT_PRICE,
                SaleOrNot=p.SALE_OR_NOT,
                Tag=p.TAG

            }) .ToList();
            // 返回商品信息
            return Ok(productDtos);
        }
    }

   
}
