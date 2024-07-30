using BackendCode.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using BackendCode.DTOs.Store;
using StoreViewMarket.Controllers;

namespace StoreOrderController.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoreOrderController : ControllerBase
    {
        private readonly YourDbContext _dbContext;
        private readonly ILogger<StoreViewMarketController> _logger;

        public StoreOrderController(YourDbContext dbContext, ILogger<StoreViewMarketController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
   

    }
}
