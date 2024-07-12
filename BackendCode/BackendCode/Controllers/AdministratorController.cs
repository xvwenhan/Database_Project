using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendCode.Data;
using BackendCode.DTOs.Administrator;

namespace Administrator.Controllers
{
    [ApiController]
    [Route("api/Administrator/[controller]")]
    public class AdministratorController : ControllerBase
    {
        private readonly YourDbContext _dbContext;

        public AdministratorController(YourDbContext context)
        {
            _dbContext = context;
        }

        [HttpGet("GetAllAuthentication")]
        public async Task<IActionResult> GetAllAuthenticationAsync(string adminId)
        {

            // 通过管理员ID获取信息
            var authentications = await _dbContext.SUBMIT_AUTHENTICATIONS
                .Where(bp => bp.ADMINISTRATOR_ACCOUNT_ID == adminId)
                .ToListAsync();

            if (authentications == null || !authentications.Any())
            {
                return NotFound("No authentication found for the given administrator.");
            }

            var allAuthenticationDtos = authentications.Select(p => new ShowAuthenticationDTO
            {
                StoreId =p.STORE_ACCOUNT_ID,
                Authentication =p.AUTHENTICATION,
                Status=p.STATUS

            }).ToList();
            // 返回商品信息
            return Ok(allAuthenticationDtos);
        }
    }


}
