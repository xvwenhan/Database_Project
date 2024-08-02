using BackendCode.Data;
using Microsoft.AspNetCore.Mvc;
using BackendCode.DTOs.LoginModel;
//以下是为了cookie新加的
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
//邮件
using System.Net.Mail;
using System.Net;
//以下是为了使用ID生成器和哈希等
using BackendCode.Services;

namespace Account.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly YourDbContext _context;
        public string filePath = "./Services/account_id.txt";
        public IdGenerator idGenerator;

        public AccountController(YourDbContext context)
        {
            _context = context;
            idGenerator = new IdGenerator(filePath);
        }

        /*登录：
         * 支持用户使用ID或者邮箱作为用户名进行登录
         * 将用户的ID 邮箱 身份装入cookie*/
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            //查看是否为买家
            var user = _context.BUYERS.FirstOrDefault(u => u.ACCOUNT_ID == model.Username || u.EMAIL == model.Username);
            if (user != null && VerifyPassword(model.Password, user.PASSWORD))
            {
                var claims = new List<Claim>
                {
                    //new Claim(ClaimTypes.Name, user.USER_NAME),
                    new Claim(ClaimTypes.NameIdentifier, user.ACCOUNT_ID),
                    new Claim("UserRole", "买家"), // 添加用户角色的 Claim为买家
                    new Claim("UserEmail", user.EMAIL) // 添加用户邮箱
                    //注意，这里的值若为空就会引发后端报错。因此最好携带主码
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

                return Ok(new { Message = "登录成功！", Role = "买家" });
            }

            //查看是否为卖家
            var user2 = _context.STORES.FirstOrDefault(u => u.ACCOUNT_ID == model.Username || u.EMAIL == model.Username);
            if (user2 != null && VerifyPassword(model.Password, user2.PASSWORD))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user2.ACCOUNT_ID),
                    new Claim("UserRole", "商家"), // 添加用户角色的 Claim为商家
                    new Claim("UserEmail", user.EMAIL) // 添加用户邮箱
                    //注意，这里的值若为空就会引发后端报错。因此最好携带主码
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

                return Ok(new { Message = "登录成功！", Role = "商家" });
            }

            //查看是否为管理员
            var user3 = _context.ADMINISTRATORS.FirstOrDefault(u => u.ACCOUNT_ID == model.Username || u.EMAIL == model.Username);
            if (user3 != null && VerifyPassword(model.Password, user3.PASSWORD))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user3.ACCOUNT_ID),
                    new Claim("UserRole", "管理员"), // 添加用户角色的 Claim为管理员
                    new Claim("UserEmail", user.EMAIL) // 添加用户邮箱
                    //注意，这里的值若为空就会引发后端报错。因此最好携带主码
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

                return Ok(new { Message = "登录成功！", Role = "管理员" });
            }
            // 如果验证失败
            if (user != null || user2 != null || user3 != null)
                return Unauthorized(new { Message = "密码错误！" });
            return NotFound(new { Message = "账号不存在！请先注册" });
        }

        /*登出：使用户cookie失效*/
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = User.FindFirst("UserRole")?.Value;
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new
            {
                UserId = userId,
                UserRole = userRole,
                Message = "登出账号成功！"
            });
            //return Ok(new { Message = "登出账号成功！" });
        }

        /*访问受保护端点：
         * 仅做测试之用，无cookie进不去
         * 当用户访问受保护的端点时，ASP.NET Core 会自动将 Claims 从 Cookie 中提取到 HttpContext.User
         */
        [HttpGet("protected")]
        [Authorize]
        public IActionResult Protected()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = User.FindFirst("UserRole")?.Value;
            return Ok(new { Message = $"{userRole}{userId}成功进入受保护端点！" });
        }

        /*密码验证：
         * 后面这个函数要替换成哈希助手中的验证函数*/
        private bool VerifyPassword(string password, string storedHash)
        {
            // 密码验证逻辑（这里直接比较了先）
            return password == storedHash;
        }

        /*修改密码：
         * 输入用户名（邮箱或ID）及新密码*/
        [HttpPost("password_reset")]
        public IActionResult PasswordReset([FromBody] LoginModel model)
        {
            var user = _context.BUYERS.FirstOrDefault(u => u.ACCOUNT_ID == model.Username || u.EMAIL == model.Username);
            if (user == null)
            {
                return NotFound(new { message = "账号不存在！" });
            }
            if(VerifyPassword(model.Password, user.PASSWORD))
                return BadRequest(new {message="新密码不能与旧密码相同！"});

            user.PASSWORD = model.Password;
            _context.SaveChanges();
            return Ok(new { message="密码重置成功！" });
        }

        /*向指定邮箱发送邮箱验证码
         * 由前端判断验证码是否正确*/
        [HttpGet("send_verification_code")]
        public IActionResult SendVerificationCode(string email)
        {
            MailMessage message = new MailMessage();

            // 设置发件人,发件人需要与设置的邮件发送服务器的邮箱一致
            MailAddress fromAddr = new MailAddress("1473030672@qq.com");
            message.From = fromAddr;
            // 设置收件人,可添加多个,添加方法与下面的一样
            message.To.Add(email);
            // 设置邮件标题
            message.Subject = "注册验证码";
            // 设置邮件内容
            Random r = new Random();
            var _verificationCode = r.Next(1000, 10000).ToString();
            message.Body = "您好，您正在注册非遗平台账号，您的验证码为 " + _verificationCode;

            // 设置邮件发送服务器,服务器根据你使用的邮箱而不同,可以到相应的邮箱管理后台查看
            //SmtpClient client = new SmtpClient("smtp.outlook.com", 587);
            SmtpClient client = new SmtpClient("smtp.qq.com", 25);
            // 设置发送人的邮箱账号和授权码
            client.Credentials = new NetworkCredential("1473030672@qq.com", "pxdzrvnwxffdbafa");
            //client.Credentials = new NetworkCredential("1239716933@qq.com", "tgfcpyibnajfhiac");

            // 启用ssl,也就是安全发送
            client.EnableSsl = true;
            // 发送邮件
            try
            {
                client.Send(message);
                return Ok(new { message = "验证码已发送", verificationCode = _verificationCode });
            }
            catch (Exception ex)
            {
                return BadRequest("验证码发送失败: " + ex.Message);
            }
        }

        /*注册：*/
        [HttpPost("register")]
        public IActionResult UserRegister([FromBody] RegisterModel model)
        {
            //方便测试加入这个//////记得删
            var userExists = _context.ACCOUNTS.Any(u => u.EMAIL == model.Email);
            if (userExists)
            {
                return BadRequest(new { message = "邮箱已经存在，不能重复注册！" });
            }

            string newId = idGenerator.GetNextId();
            Console.WriteLine("生成的ID: " + newId);//测试用，记得删

            string uidb = newId;
            if (model.Role == "买家")
            {
                uidb = "U" + uidb;
                _context.BUYERS.Add(new BackendCode.Models.BUYER()
                {
                    ACCOUNT_ID = uidb,
                    EMAIL = model.Email,
                    PASSWORD = model.Password,
                    TOTAL_CREDITS = 0
                }) ;
                _context.SaveChanges();
            }
            else if (model.Role == "商家")
            {
                uidb = "S" + uidb;
                _context.STORES.Add(new BackendCode.Models.STORE()
                {
                    ACCOUNT_ID = uidb,
                    EMAIL = model.Email,
                    PASSWORD = model.Password,
                    STORE_SCORE = 0,
                });
                _context.SaveChanges();
            }
            else
            {
                return BadRequest(new {message="Role字段错误！只能为“买家”或“商家”"});
            }

            return Ok(new {message="注册成功！",accountId= uidb });
        }

        /*检查注册状态*/
        [HttpGet("check_register")]
        public IActionResult CheckRegister(string email)
        {

            var userExists = _context.ACCOUNTS.Any(u => u.EMAIL == email);
            if (userExists)
            {
                return BadRequest(new { message = "邮箱已经存在，不能重复注册！" });
            }
            else
            {
                return Ok(new { message = "邮箱未被注册" });
            }
        }



    }
}


/*if (ModelState.IsValid)
{
    // 如果模型验证成功，执行注册逻辑
    return Ok(new { message = "Registration successful" });
}
else
{
    // 如果模型验证失败，返回错误信息
    return BadRequest(ModelState);
}*/