using Microsoft.EntityFrameworkCore;
using BackendCode.Data;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

//ʹ��cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/api/account/login"; // ���õ�¼·��
        options.AccessDeniedPath = "/api/account/accessdenied"; // ���þܾ�����·��
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // ����Cookie����ʱ��

        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.None; // �������� Cookie
    });

//�����������
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
               builder => builder
                   .WithOrigins("http://localhost:8080")
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials());
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
    app.UseDeveloperExceptionPage();//�¼ӵ� ��֪����ɶ��
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("AllowSpecificOrigin");// ʹ�� CORS �м��
}

app.UseHttpsRedirection();
app.UseAuthentication(); // ������ UseAuthorization ֮ǰ����.������ȷ����������֤�м��
app.UseAuthorization();

app.MapControllers();

app.Run();
