using BackendCode.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Account.Controllers
{
    [ApiController]
    [Route("api/account/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly YourDbContext _dbContext;
        private readonly ILogger<LoginController> _logger;

        public LoginController(YourDbContext dbContext, ILogger<LoginController> logger)
        {
            this._dbContext = dbContext;
            this._logger = logger;
        }

        //注册
        //前端自行完成用户名等为空的错误判断
        

        /*[HttpGet("/login")]
        public IActionResult Login(string uid_orp_number, string passwd)
        {
            var tempAccount = _dbContext.ACCOUNTS.FirstOrDefault(a => a.ACCOUNT_ID == uid_orp_number);
            if (tempAccount != null)//如果该uid存在
            {
                if (tempAccount.PASSWORD == passwd)
                {
                    return Ok(1);
                }
                else
                {
                    return BadRequest($"密码输入错误，请重新输入！");
                }
            }
            else//如果uid不存在
            {
                var temp2Account = _dbContext.ACCOUNTS.FirstOrDefault(a => a.PHONE_NUMBER == uid_orp_number);
                if (tempAccount != null)//如果该电话号码存在
                {
                    if (tempAccount.PASSWORD == passwd)
                    {
                        return Ok(1);
                    }
                    else
                    {
                        return BadRequest($"密码输入错误，请重新输入！");
                    }
                }
                else//uid和电话号码都不存在
                {
                    return BadRequest("该账号尚未注册，请先注册");
                }
            }
        }*/
    }
}


