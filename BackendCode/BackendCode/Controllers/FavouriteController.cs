using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendCode.Data;
using BackendCode.DTOs.Favourite;

namespace Favourite.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FavouriteController : ControllerBase
    {
        private readonly YourDbContext _dbContext;

        public FavouriteController(YourDbContext context)
        {
            _dbContext = context;
        }

        [HttpPost("GetFavoriteProducts")]
        public async Task<IActionResult> GetFavoriteProductsAsync([FromBody] GFPModel model)
        {
            // 获取用户喜欢的商品ID
            var favoriteProductIds = await _dbContext.BUYER_PRODUCT_BOOKMARKS
                .Where(bp => bp.BUYER_ACCOUNT_ID == model.userId)
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
                BuyerId = model.userId,
                StoreId = p.ACCOUNT_ID,
                ProductId=p.PRODUCT_ID,
                ProductPrice=p.PRODUCT_PRICE,
                SaleOrNot=p.SALE_OR_NOT,
                Tag=p.TAG

            }) .ToList();
            // 返回商品信息
            return Ok(productDtos);
        }


        [HttpPost("GetFavoriteStores")]
        public async Task<IActionResult> GetFavoriteStoresAsync([FromBody] GFPModel model)
        {
            // 获取用户喜欢的商店ID
            var favoriteStoreIds = await _dbContext.BUYER_STORE_BOOKMARKS
                .Where(bp => bp.BUYER_ACCOUNT_ID == model.userId)
                .Select(bp => bp.STORE_ACCOUNT_ID)
                .ToListAsync();

            // 通过商店ID获取商店详细信息
            var favoriteStores = await _dbContext.STORES
                .Where(p => favoriteStoreIds.Contains(p.ACCOUNT_ID))
                .ToListAsync();

            if (favoriteStores == null || !favoriteStores.Any())
            {
                return NotFound("No stores found for the given user ID.");
            }

            var productDtos = favoriteStores.Select(p => new FavouriteStoresDTO
            {
                BuyerId = model.userId,
                StoreId = p.ACCOUNT_ID,
                StoreName = p.STORE_NAME,
                StoreScore = p.STORE_SCORE,

            }).ToList();
            // 返回商品信息
            return Ok(productDtos);
        }
    }

   
}
