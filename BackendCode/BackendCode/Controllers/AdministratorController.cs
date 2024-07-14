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

        //修改
        [HttpPut("UpdateStoreAuthentication")]
        public async Task<IActionResult> UpdateStoreAuthentication(string storeId, bool result)
        {
            // 查找提交的认证
            var temp = await _dbContext.SUBMIT_AUTHENTICATIONS
                .FirstOrDefaultAsync(a => a.STORE_ACCOUNT_ID == storeId);

            // 查找商店
            var tempstore = await _dbContext.STORES
                .FirstOrDefaultAsync(b => b.ACCOUNT_ID == storeId);

            if (temp == null)
            {
                return NotFound("No authentication found for the given store.");
            }

            // 确保商店存在
            if (tempstore == null)
            {
                return NotFound("Store not found.");
            }

            if (result)
            {
                temp.STATUS = "已通过";
                tempstore.CERTIFICATION = true;
            }
            else
            {
                temp.STATUS = "已拒绝";
                tempstore.CERTIFICATION = false;
            }

            // 保存更改
            try
            {
                await _dbContext.SaveChangesAsync();
                return Ok($"Authentication status updated successfully:{temp.STATUS}");
            }
            catch (DbUpdateException ex)
            {
                // 捕获数据库更新异常并返回错误消息
                return StatusCode(500, $"An error occurred while updating the database: {ex.Message}");
            }
            
        }
    }

}

