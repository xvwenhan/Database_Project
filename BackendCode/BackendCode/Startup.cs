using Alipay.AopSdk.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
/*public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }
    public void ConfigureServices(IServiceCollection services)
    {
        // ...其他服务配置

        // 配置支付宝客户端
        services.AddSingleton<IAopClient>(provider =>
        {
            var config = provider.GetService<IConfiguration>();
            IAopClient client = new DefaultAopClient(
                config["Alipay:Gatewayurl"],
                config["Alipay:AppId"],
                config["Alipay:PrivateKey"],
                "json",
                "utf-8",
                config["Alipay:AlipayPublicKey"],
                config["Alipay:SignType"] // 确保这里是一个字符串

            );
            return client;
        });

        // 添加控制器服务
        services.AddControllers();
    }



    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
}*/