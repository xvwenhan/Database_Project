using Microsoft.AspNetCore.Mvc;
using WebApi1.Data;

namespace WebApi1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class YourController : ControllerBase
    {
        private readonly YourDbContext _dbContext;
        private readonly ILogger<YourController> _logger;

        public YourController(YourDbContext dbContext, ILogger<YourController> logger)
        {
            this._dbContext = dbContext;
            this._logger = logger;
        }

        [HttpGet]
        public IActionResult GetTYRuleCodes(string? cust)
        {
            try
            {
                var ruleCodes = string.IsNullOrEmpty(cust) ? _dbContext.ACCOUNT
                    .ToList() : _dbContext.ACCOUNT.Where(code => code.ACCOUNT_ID == cust)
                    .ToList();

                return Ok(ruleCodes);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                return BadRequest($"An error occurred while fetching data: {ex.Message}");
            }
        }
    }
}


