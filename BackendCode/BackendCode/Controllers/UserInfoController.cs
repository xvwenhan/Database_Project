using BackendCode.Data;
using Microsoft.AspNetCore.Mvc;
using BackendCode.DTOs.UserInfo;
using Microsoft.EntityFrameworkCore;

namespace UserInfo.Controllers
{
    [ApiController]
    [Route("api/userInfo/[controller]")]
    public class UserInfoController : ControllerBase
    {
        private readonly YourDbContext _dbContext;
        private readonly ILogger<UserInfoController> _logger;
       
        public UserInfoController(YourDbContext dbContext, ILogger<UserInfoController> logger)
        {
            this._dbContext = dbContext;
            this._logger = logger;

        }

        // 根据用户id返回个人详细信息
        [HttpGet("/detailedInfo")]
        public async Task<IActionResult> ShowUserInfo(string uid)
        {
            var infos = await _dbContext.BUYERS.FirstOrDefaultAsync(a => a.ACCOUNT_ID == uid);

            if (infos == null)
            {
                return NotFound("User not found.");
            }

            var response = new UserInfoDTO
            {
                Id = infos.ACCOUNT_ID,
                UserName = infos.USER_NAME,
                EMAIL = infos.EMAIL,
                Gender = infos.GENDER,
                Age = infos.AGE,
                TotalCredits = infos.TOTAL_CREDITS,
                Address = infos.ADDRESS
            };
            return Ok(response);
        }

    }
}


