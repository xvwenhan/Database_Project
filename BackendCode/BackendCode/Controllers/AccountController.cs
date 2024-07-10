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
            var tempAccount = _dbContext.ACCOUNT.FirstOrDefault(a => a.PHONE_NUMBER == p_number);
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

                _dbContext.ACCOUNT.Add(new BackendCode.Models.ACCOUNT(){
                    ACCOUNT_ID = uidb,
                    USERNAME = u_name,
                    PHONE_NUMBER = p_number,
                    PASSWORD = passwd
                    
                }) ;
                _dbContext.SaveChanges();
                return Ok(uidb);
            }

        }

       /* [HttpGet("/login")]
        public IActionResult IndexTest(string? user_name, string phone_number, string passwd)
        {
            _dbContext.ACCOUNT.Add(new BackendCode.Models.ACCOUNT() { ACCOUNT_ID = "1", USERNAME = user_name, PHONE_NUMBER = phone_number, PASSWORD = passwd });
            _dbContext.SaveChanges();
            var list = _dbContext.ACCOUNT.ToList();

            return Ok(list);
        }*/
    }
}


