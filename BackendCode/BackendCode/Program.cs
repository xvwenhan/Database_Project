using Microsoft.EntityFrameworkCore;
using BackendCode.Data;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//使用cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/api/account/login"; // 设置登录路径
        options.AccessDeniedPath = "/api/account/accessdenied"; // 设置拒绝访问路径
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // 设置Cookie过期时间

        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.None; // 允许跨域 Cookie
    });

//解决跨域问题
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
               builder =>
               {
                   builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();

                   /*     builder.AllowCredentials(); // 允许携带凭据。需要下一个microsoft.aspnetcore.cor包才能用.
                                                    // 但它为什么和AllowAnyOrigin()只能用一个？？？*/
               });

    /*    options.AddPolicy("AllowSpecificOrigin",
               builder => builder
                   .WithOrigins("http://localhost:8081") // 允许的前端地址   mmy:http://100.79.96.89:8080
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials());*/
});




// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<YourDbContext>(options => {
    string connectionString = builder.Configuration.GetConnectionString("OracleConnection"); ;
    options.UseOracle(connectionString);
});
builder.Services.AddControllers();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseDeveloperExceptionPage();//新加的 不知道干啥的
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("AllowAnyOrigin");
    //app.UseCors("AllowSpecificOrigin");// 使用 CORS 中间件
}

app.UseHttpsRedirection();
app.UseAuthentication(); // 必须在 UseAuthorization 之前调用.用于正确配置身份验证中间件
app.UseAuthorization();

app.MapControllers();

app.Run();
