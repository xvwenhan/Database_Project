using BackendCode.Data;
using Microsoft.AspNetCore.Mvc;
using BackendCode.DTOs.LoginModel;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
/////以下是为了cookie新加的
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Cors;///
///以下是为了哈希新加的
using System.Security.Cryptography;

namespace Account.Controllers
{
    /*调试说明：账号密码都是007 */
    [ApiController]
    [EnableCors("AllowSpecificOrigin")] // 应用 CORS 策略到整个控制器////////////////
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly YourDbContext _context;

        public AccountController(YourDbContext context)
        {
            _context = context;
        }

        //后面这个登录要替换成下面被注释掉的登录函数（这个不需要发邮箱，测试起来比较简便）
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            // 验证用户名和密码
            var user = _context.BUYERS.FirstOrDefault(u => u.ACCOUNT_ID == model.Username);
            if (user != null && VerifyPassword(model.Password, user.PASSWORD))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.USER_NAME),
                    new Claim(ClaimTypes.NameIdentifier, user.ACCOUNT_ID)
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    
                    // 可以在此处设置额外的 Cookie 属性
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return Ok(new { Message = "登录成功！" });
            }

            // 如果验证失败
            return Unauthorized(new { Message = "Invalid login attempt" });
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { Message = "Logout successful" });
        }

        //仅做测试之用，无cookie进不去
        [HttpGet("protected")]
        [Authorize]
        public IActionResult Protected()
        {
            return Ok(new { Message = "This is a protected endpoint" });
        }

        //后面这个函数要替换成哈希助手中的验证函数
        private bool VerifyPassword(string password, string storedHash)
        {
            // 实现你的密码验证逻辑（例如哈希比较，这里直接比较了先）
            return password == storedHash;
        }


        //为方便测试还没加邮箱验证
        [HttpPost("password-reset")]
        public IActionResult PasswordReset(string accountId, string newPassword)
        {
            var user = _context.BUYERS.FirstOrDefault(u => u.ACCOUNT_ID == accountId);
            user.PASSWORD = newPassword;
            _context.SaveChanges();
            return Ok("密码重置成功！");
        }

    }
    //哈希密码助手（这块代码目前没用过）
    public class PasswordHelper
    {
        private const int SaltSize = 16; // 128 bit
        private const int KeySize = 32; // 256 bit
        private const int Iterations = 10000;

        public static string HashPassword(string password)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var salt = new byte[SaltSize];
                rng.GetBytes(salt);

                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations))
                {
                    var key = pbkdf2.GetBytes(KeySize);
                    var hash = new byte[SaltSize + KeySize];
                    Array.Copy(salt, 0, hash, 0, SaltSize);
                    Array.Copy(key, 0, hash, SaltSize, KeySize);

                    return Convert.ToBase64String(hash);
                }
            }
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            var hashBytes = Convert.FromBase64String(hashedPassword);
            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations))
            {
                var key = pbkdf2.GetBytes(KeySize);
                for (int i = 0; i < KeySize; i++)
                {
                    if (hashBytes[i + SaltSize] != key[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }//之后需要替换密码验证 变成这个
    }
}


//邮箱我单独拆出来了

/*public class LoginController : ControllerBase
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


    [HttpGet("/already-registered")]
    public IActionResult test_register(string email)
    {
        var tempAccount = _dbContext.BUYERS.FirstOrDefault(a => a.PHONE_NUMBER == email);//要改这里
        if (tempAccount != null) // 如果该邮箱已存在
        {
            return BadRequest("该邮箱已经被注册");
        }
        return Ok("邮箱未被注册过账号，可以注册");
    }

    //注册
    //前端自行完成用户名等为空的错误判断
    [HttpGet("/register")]
    public IActionResult UserRegister(string code, string u_name, string p_number,string email, string passwd, string u_gender, int u_age, string u_address)
    {
        // 验证验证码
        if (code != _verificationCode || p_number != _email)
        {
            return BadRequest("验证码错误或邮箱不匹配");
        }
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

    [HttpGet("/login")]
    public async Task<IActionResult> Login(string uid_orp_number, string passwd)
    {
        var tempAccount = _dbContext.BUYERS.FirstOrDefault(a => a.ACCOUNT_ID == uid_orp_number);
        if (tempAccount != null)//如果该uid存在
        {
            if (tempAccount.PASSWORD == passwd)
            {
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

        var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,uid_orp_number),
                    new Claim(ClaimTypes.NameIdentifier, uid_orp_number)//要改，在乱写
                };

        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            // 可以在此处设置额外的 Cookie 属性
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        return Ok(new { Message = "登录成功！" });
    }
}*/

