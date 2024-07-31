using BackendCode.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using BackendCode.DTOs.Store;
using BackendCode.Models;

namespace StoreOrderController.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoreOrderController : ControllerBase
    {
        private readonly YourDbContext _dbContext;
        private readonly ILogger<StoreOrderController> _logger;

        public StoreOrderController(YourDbContext dbContext, ILogger<StoreOrderController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet("GetOrders")]
        public async Task<IActionResult> GetOrders(string storeId, int orderStatus)
        {
            if (string.IsNullOrEmpty(storeId))
            {
                return BadRequest("Store ID is required.");
            }

            if (orderStatus < 1 || orderStatus > 6)
            {
                return BadRequest("Invalid status type. Allowed values are 1-6.");
            }
            try
            {
                IQueryable<ORDERS> query = _dbContext.ORDERS
                    .Where(o => o.STORE_ACCOUNT_ID == storeId);
                    switch (orderStatus)
                        {
                        case 1: // 全部
                            break;
                        case 2: // 待发货
                            query = query.Where(o => o.ORDER_STATUS == "待发货" && o.DELIVERY_NUMBER == null);
                            break;
                        case 3: // 运输中
                            query = query.Where(o => o.ORDER_STATUS == "运输中" && o.DELIVERY_NUMBER != null);
                            break;
                        case 4: // 已送达
                            query = query.Where(o => o.ORDER_STATUS == "已送达");
                            break;
                        case 5: // 待退货
                            query = query.Where(o => o.ORDER_STATUS == "待退货");
                            break;
                        case 6: // 已退货
                            query = query.Where(o => o.ORDER_STATUS == "已退货" && o.RETURN_OR_NOT == true);
                            break;
                        default:
                            return BadRequest("Invalid order status value.");
                        }
                    var orders = await query.ToListAsync();
                    var orderDTOs =orders.Select(o => new OrderDto
                    {
                        ORDER_ID = o.ORDER_ID,
                        TOTAL_PAY = o.TOTAL_PAY,
                        ACTUAL_PAY = o.ACTUAL_PAY,
                        ORDER_STATUS = o.ORDER_STATUS,
                        CREATE_TIME = o.CREATE_TIME,
                        DELIVERY_NUMBER = o.DELIVERY_NUMBER,
                        SCORE = o.SCORE,
                        REMARK = o.REMARK,
                        RETURN_OR_NOT = o.RETURN_OR_NOT,
                        
                    });

                return Ok(orderDTOs);
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException ex) when (ex.Number == 12704)
            {
                _logger.LogError(ex, "Charset mismatch error.");
                return StatusCode(500, "Internal server error: Charset mismatch.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting orders for store {storeId}", storeId);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
