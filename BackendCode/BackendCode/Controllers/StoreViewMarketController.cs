using BackendCode.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using BackendCode.DTOs.Store;

namespace StoreViewMarket.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoreViewMarketController : ControllerBase
    {
        private readonly YourDbContext _dbContext;
        private readonly ILogger<StoreViewMarketController> _logger;

        public StoreViewMarketController(YourDbContext dbContext, ILogger<StoreViewMarketController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        //Post接口，传入店铺id，传回所有市集信息及该店铺参与情况
        [HttpPost]
        [Route("GetMarketsByStoreId")]
        public async Task<ActionResult<List<MarketDTO>>> GetMarketsByStoreId(string storeID)
        {
            if (string.IsNullOrEmpty(storeID))
            {
                return BadRequest("Store ID is required.");
            }

            try
            {
                var markets = await _dbContext.MARKETS.ToListAsync();
                var marketDTOs = new List<MarketDTO>();

                foreach (var market in markets)
                {
                    var marketStore = await _dbContext.MARKET_STORES
                        .Where(ms => ms.MARKET_ID == market.MARKET_ID && ms.STORE_ACCOUNT_ID == storeID)
                        .Select(ms => ms.IN_OR_NOT)
                        .FirstOrDefaultAsync();

                    var marketDTO = new MarketDTO
                    {
                        MarketId = market.MARKET_ID,
                        Theme = market.THEME,
                        StartTime = market.START_TIME,
                        EndTime = market.END_TIME,
                        Detail = market.DETAIL,
                        PosterImg = market.POSTERIMG != null ? Convert.ToBase64String(market.POSTERIMG) : null,
                        IsStoreParticipating = marketStore
                    };

                    marketDTOs.Add(marketDTO);
                }

                return Ok(marketDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting markets by store ID.");
                return StatusCode(500, "Internal server error.");
            }
        }

        //Put接口，更改店铺参与市集情况
        [HttpPut]
        [Route("UpdateMarketStoreParticipation")]
        public async Task<IActionResult> UpdateMarketStoreParticipation(UpdateMarketStoreDTO request)
        {
            if (request == null || string.IsNullOrEmpty(request.MarketId) || string.IsNullOrEmpty(request.StoreId))
            {
                return BadRequest("Market ID and Store ID are required.");
            }

            try
            {
                var marketStore = await _dbContext.MARKET_STORES
                    .FirstOrDefaultAsync(ms => ms.MARKET_ID == request.MarketId && ms.STORE_ACCOUNT_ID == request.StoreId);

                if (marketStore == null)
                {
                    return NotFound("Market store entry not found.");
                }

                marketStore.IN_OR_NOT = request.InOrOut;
                await _dbContext.SaveChangesAsync();

                return Ok("Update successful.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating market store participation.");
                return StatusCode(500, "Internal server error.");
            }
        }

        //Post接口，传入店铺id，市集theme,传回市集信息及该店铺参与情况
        // 搜索 market
        [HttpGet("searchMarket")]
        public async Task<IActionResult> SearchMarket(string theme, string storeID)
        {
            if (string.IsNullOrEmpty(theme))
            {
                return BadRequest("Theme is required.");
            }

            try
            {
                // 分解关键词，用于包含字符的模糊匹配
                var keywordArray = theme.ToCharArray();
                var likePattern = string.Join("%", keywordArray);

                var markets = await _dbContext.MARKETS
                    .Where(m => EF.Functions.Like(m.THEME, $"%{theme}%") ||
                                EF.Functions.Like(m.THEME, $"%{likePattern}%"))
                    .ToListAsync();

                var marketDTOs = markets.Select(async m =>
                {
                    var marketStore = await _dbContext.MARKET_STORES
                        .Where(ms => ms.MARKET_ID == m.MARKET_ID && ms.STORE_ACCOUNT_ID == storeID)
                        .Select(ms => ms.IN_OR_NOT)
                        .FirstOrDefaultAsync();

                    return new MarketDTO
                    {
                        MarketId = m.MARKET_ID,
                        Theme = m.THEME,
                        StartTime = m.START_TIME,
                        EndTime = m.END_TIME,
                        Detail = m.DETAIL,
                        PosterImg = m.POSTERIMG != null ? Convert.ToBase64String(m.POSTERIMG) : null,
                        IsStoreParticipating = marketStore
                    };
                }).ToList();

                var marketDTOsResolved = await Task.WhenAll(marketDTOs);

                var orderedMarkets = marketDTOsResolved.OrderByDescending(m => m.Theme == theme)  // 完全匹配优先
                                                         .ThenByDescending(m => m.Theme.Contains(theme))  // 部分匹配次优
                                                         .ThenBy(m => m.Theme.StartsWith(theme))  // 前缀匹配再次
                                                         .ThenBy(m => m.Theme.IndexOf(theme))  // 名称包含关键字次之
                                                         .ToList();

                if (!orderedMarkets.Any())
                {
                    return NotFound("No markets found for the given theme.");
                }

                return Ok(orderedMarkets);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching markets by theme.");
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
