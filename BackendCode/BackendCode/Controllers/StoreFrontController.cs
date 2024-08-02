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

namespace StoreFrontController.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoreFrontController : ControllerBase
    {
        private readonly YourDbContext _dbContext;
        private readonly ILogger<StoreFrontController> _logger;

        public StoreFrontController(YourDbContext dbContext, ILogger<StoreFrontController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        // get接口传入store_id和时间，传出订单统计信息
        [HttpGet("GetOrderStats")]
        public async Task<IActionResult> GetOrderStats(string storeId, DateTime date)
        {
            if (string.IsNullOrEmpty(storeId))
            {
                return BadRequest("Store ID is required.");
            }

            try
            {
                var market = await _dbContext.MARKETS
                    .FirstOrDefaultAsync(m => m.START_TIME <= date && m.END_TIME >= date);

                if (market == null)
                {
                    return BadRequest("No active market found for the given date.");
                }

                var orderStats = await _dbContext.ORDERS
                    .Where(o => o.STORE_ACCOUNT_ID == storeId && o.CREATE_TIME.HasValue && o.CREATE_TIME.Value.Date == date.Date)
                    .GroupBy(o => o.ORDER_STATUS)
                    .Select(g => new
                    {
                        OrderStatus = g.Key,
                        Count = g.Count()
                    })
                    .ToListAsync();

                var waitingForShipment = orderStats.FirstOrDefault(os => os.OrderStatus == "待发货")?.Count ?? 0;
                var waitingForReturn = orderStats.FirstOrDefault(os => os.OrderStatus == "待退货")?.Count ?? 0;

                var marketStoreStats = await _dbContext.MARKET_STORES
                    .Where(ms => ms.STORE_ACCOUNT_ID == storeId)
                    .GroupBy(ms => ms.IN_OR_NOT)
                    .Select(g => new
                    {
                        InOrNot = g.Key,
                        Count = g.Count()
                    })
                    .ToListAsync();

                var inCount = marketStoreStats.FirstOrDefault(ms => ms.InOrNot == true)?.Count ?? 0;
                var outCount = marketStoreStats.FirstOrDefault(ms => ms.InOrNot == false)?.Count ?? 0;

                var orderTotal = await _dbContext.ORDERS
                    .Where(o => o.STORE_ACCOUNT_ID == storeId && o.CREATE_TIME.HasValue && o.CREATE_TIME.Value.Date == date.Date)
                    .ToListAsync();

                var orderCount = orderTotal.Count;
                var totalRevenue = orderTotal.Sum(o => o.ACTUAL_PAY) ?? 0;

                return Ok(new
                {
                    WaitingForShipment = waitingForShipment,
                    WaitingForReturn = waitingForReturn,
                    InCount = inCount,
                    OutCount = outCount,
                    OrderCount = orderCount,
                    TotalRevenue = totalRevenue
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting order stats for store {storeId} on date {date}", storeId, date);
                return StatusCode(500, "Internal server error");
            }
        }

        // get接口传入store_id和时间，传出该日起该店一周的订单数目
        [HttpGet("GetWeeklyOrderCount")]
        public async Task<IActionResult> GetWeeklyOrderCount(string storeId, DateTime date)
        {
            if (string.IsNullOrEmpty(storeId))
            {
                return BadRequest("Store ID is required.");
            }

            try
            {
                var startDate = date.Date;
                var endDate = date.AddDays(7).Date;

                var weeklyOrders = await _dbContext.ORDERS
                    .Where(o => o.STORE_ACCOUNT_ID == storeId && o.CREATE_TIME.HasValue && o.CREATE_TIME.Value.Date >= startDate && o.CREATE_TIME.Value.Date < endDate)
                    .GroupBy(o => o.CREATE_TIME.Value.Date)
                    .Select(g => new
                    {
                        Date = g.Key,
                        Count = g.Count()
                    })
                    .ToListAsync();

                var dailyOrderCounts = new List<object>();
                for (var day = startDate; day < endDate; day = day.AddDays(1))
                {
                    var count = weeklyOrders.FirstOrDefault(w => w.Date == day)?.Count ?? 0;
                    dailyOrderCounts.Add(new { Date = day, Count = count });
                }

                return Ok(dailyOrderCounts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting weekly order count for store {storeId} starting from date {date}", storeId, date);
                return StatusCode(500, "Internal server error");
            }
        }
        //Get接口，传出店铺名称和评分
        [HttpGet("UpdateStoreScore")]
        public async Task<IActionResult> UpdateStoreScore(string storeId)
        {
            if (string.IsNullOrEmpty(storeId))
            {
                return BadRequest("Store ID is required.");
            }

            try
            {
                // 计算订单中非空score的平均值
                var averageScore = await _dbContext.ORDERS
                    .Where(o => o.STORE_ACCOUNT_ID == storeId && o.SCORE.HasValue)
                    .AverageAsync(o => o.SCORE);

                // 找到对应的store
                var store = await _dbContext.STORES
                    .FirstOrDefaultAsync(s => s.ACCOUNT_ID == storeId);

                if (store == null)
                {
                    return NotFound("Store not found.");
                }

                // 更新store的STORE_SCORE
                store.STORE_SCORE = averageScore;

                // 保存更改
                await _dbContext.SaveChangesAsync();

                return Ok(new
                {
                    StoreName = store.STORE_NAME,
                    StoreScore = store.STORE_SCORE
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating store score for store {storeId}", storeId);
                return StatusCode(500, "Internal server error");
            }
        }
        //put接口修改店铺名称和地址，此处传""这样的空值即代表不修改原值
        [HttpPut("UpdateStoreInfo")]
        public async Task<IActionResult> UpdateStoreInfo(string storeId, [FromBody] StoreUpdateDto storeUpdateDto)
        {
            if (string.IsNullOrEmpty(storeId))
            {
                return BadRequest("Store ID is required.");
            }

            if (storeUpdateDto == null)
            {
                return BadRequest("Store update information is required.");
            }

            try
            {
                // 查找对应的store
                var store = await _dbContext.STORES
                    .FirstOrDefaultAsync(s => s.ACCOUNT_ID == storeId);

                if (store == null)
                {
                    return NotFound("Store not found.");
                }

                // 更新store的STORE_NAME和ADDRESS
                if (!string.IsNullOrEmpty(storeUpdateDto.StoreName))
                {
                    store.STORE_NAME = storeUpdateDto.StoreName;
                }

                if (!string.IsNullOrEmpty(storeUpdateDto.Address))
                {
                    store.ADDRESS = storeUpdateDto.Address;
                }

                // 保存更改
                await _dbContext.SaveChangesAsync();

                return Ok(new
                {
                    StoreName = store.STORE_NAME,
                    Address = store.ADDRESS
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating store info for store {storeId}", storeId);
                return StatusCode(500, "Internal server error");
            }
        }
        //Post提交审核接口
        [HttpPost("SubmitAuthentication")]
        public async Task<IActionResult> SubmitAuthentication(string storeId, [FromBody] SubmitAuthenticationRequest request)
        {
            if (string.IsNullOrEmpty(storeId))
            {
                return BadRequest("Store ID is required.");
            }

            if (string.IsNullOrEmpty(request.PhotoBase64))
            {
                return BadRequest("Photo is required.");
            }

            if (string.IsNullOrEmpty(request.Authentication))
            {
                return BadRequest("Authentication string is required.");
            }

            try
            {
                // 随机选择一个管理员ID
                var randomAdmin = await _dbContext.ADMINISTRATORS
                    .OrderBy(r => Guid.NewGuid())
                    .FirstOrDefaultAsync();

                if (randomAdmin == null)
                {
                    return NotFound("No administrators found.");
                }
                var store = await _dbContext.STORES
                     .FirstOrDefaultAsync(s => s.ACCOUNT_ID == storeId);

                if (store.CERTIFICATION == true )
                {
                    return BadRequest("已验证");
                }
                // 将 Base64 编码的照片字符串转换为字节数组
                byte[] photoBytes = Convert.FromBase64String(request.PhotoBase64);

                // 创建新的提交验证记录
                var submitAuthentication = new SUBMIT_AUTHENTICATION
                {
                    STORE_ACCOUNT_ID = storeId,
                    ADMINISTRATOR_ACCOUNT_ID = randomAdmin.ACCOUNT_ID,
                    AUTHENTICATION = request.Authentication,
                    STATUS = "待审核",
                    PHOTO = photoBytes
                };

                // 添加到数据库并保存
                _dbContext.SUBMIT_AUTHENTICATIONS.Add(submitAuthentication);
                await _dbContext.SaveChangesAsync();

                return Ok(new
                {
                    Message = "Authentication submitted successfully.",
                    StoreId = submitAuthentication.STORE_ACCOUNT_ID,
                    AdministratorId = submitAuthentication.ADMINISTRATOR_ACCOUNT_ID,
                    Status = submitAuthentication.STATUS
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting authentication for store {storeId}", storeId);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

