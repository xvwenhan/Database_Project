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
        private static string _verificationCode;
        private static string _email;
        public LoginController(YourDbContext dbContext, ILogger<LoginController> logger)
        {
            this._dbContext = dbContext;
            this._logger = logger;
        }

        // 发送邮箱验证码
        [HttpGet("/send-verification-code")]
        public IActionResult SendVerificationCode(string email)
        {
            var tempAccount = _dbContext.BUYERS.FirstOrDefault(a => a.PHONE_NUMBER == email);
            if (tempAccount != null) // 如果该邮箱已存在
            {
                return BadRequest("该邮箱已经被注册");
            }

            MailMessage message = new MailMessage();

            // 设置发件人,发件人需要与设置的邮件发送服务器的邮箱一致
            MailAddress fromAddr = new MailAddress("1239716933@qq.com");
            message.From = fromAddr;
            // 设置收件人,可添加多个,添加方法与下面的一样
            message.To.Add(email);
            // 设置邮件标题
            message.Subject = "注册验证码";
            // 设置邮件内容
            Random r = new Random();
            _verificationCode = r.Next(1000, 10000).ToString();
            _email = email;
            message.Body = "您好，您的验证码为 " + _verificationCode;

            // 设置邮件发送服务器,服务器根据你使用的邮箱而不同,可以到相应的邮箱管理后台查看
            SmtpClient client = new SmtpClient("smtp.qq.com", 25);
            // 设置发送人的邮箱账号和授权码
            client.Credentials = new NetworkCredential("1239716933@qq.com", "tgfcpyibnajfhiac");

            // 启用ssl,也就是安全发送
            client.EnableSsl = true;
            // 发送邮件
            try
            {
                client.Send(message);
                return Ok("验证码已发送");
            }
            catch (Exception ex)
            {
                return BadRequest("验证码发送失败: " + ex.Message);
            }
        }

        //注册
        //前端自行完成用户名等为空的错误判断
        [HttpGet("/register")]
        public IActionResult UserRegister(string code,string u_name, string p_number, string passwd, string u_gender, int u_age, string u_address)
        {
            // 验证验证码
            if (code != _verificationCode || p_number != _email)
            {
                return BadRequest("验证码错误或邮箱不匹配");
            }
            var tempAccount = _dbContext.BUYERS.FirstOrDefault(a => a.PHONE_NUMBER == p_number);
            if (tempAccount != null)//如果该手机号存在
            {
                return BadRequest($"该手机号已经被注册");//返回账号已存在
            }
            else//如果是新手机号
            {
                Random random = new();
                int _userId = random.Next(1, 10000000);
                string a = _userId.ToString();
                string uidb = "U" + a;

                _dbContext.BUYERS.Add(new BackendCode.Models.BUYER()
                {
                    ACCOUNT_ID = uidb,
                    USER_NAME = u_name,
                    PHONE_NUMBER = p_number,
                    PASSWORD = passwd,
                    ADDRESS = u_address,
                    GENDER = u_gender,
                    AGE = u_age,
                    TOTAL_CREDITS = 0
                });
                _dbContext.SaveChanges();
                return Ok(uidb);
            }
        }


        [HttpGet("/login")]
        public IActionResult Login(string uid_orp_number, string passwd)
        {
            var tempAccount = _dbContext.BUYERS.FirstOrDefault(a => a.ACCOUNT_ID == uid_orp_number);
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
                var temp2Account = _dbContext.BUYERS.FirstOrDefault(a => a.PHONE_NUMBER == uid_orp_number);
                if (temp2Account != null)//如果该电话号码存在
                {
                    if (temp2Account.PASSWORD == passwd)
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


