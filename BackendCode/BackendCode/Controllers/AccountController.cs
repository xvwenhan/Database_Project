using Microsoft.AspNetCore.Mvc;
using BackendCode.Data;

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
        [HttpGet("/register")]
        public IActionResult UserRegister(string u_name,string p_number,string passwd)
        {
            var tempAccount = _dbContext.ACCOUNTS.FirstOrDefault(a => a.PHONE_NUMBER == p_number);
            if (tempAccount != null)//如果该手机号存在
            {
                return BadRequest($"该手机号已经被注册");//返回账号已存在
            }
            else//如果是新手机号
            {
                Random random = new();
                int _userId = random.Next(1,10000000);
                string a = _userId.ToString();
                string uidb = "U" + a;

                _dbContext.ACCOUNTS.Add(new BackendCode.Models.ACCOUNT(){
                    ACCOUNT_ID = uidb,
                    USER_NAME = u_name,
                    PHONE_NUMBER = p_number,
                    PASSWORD = passwd
                    
                }) ;
                _dbContext.SaveChanges();
                return Ok(uidb);
            }

        }

        [HttpGet("/login")]
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
        }
    }
}


