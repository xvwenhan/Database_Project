using Microsoft.AspNetCore.Mvc;           
using BackendCode.Data;                  
using System.IO;                          
using Microsoft.AspNetCore.Http;          

namespace BackendCode.Controllers
{
    [ApiController]               //API控制器
    [Route("api/[controller]")]   //设置控制器的基础路由
    public class YourController : ControllerBase
    {
        private readonly YourDbContext _dbContext; 
        private readonly ILogger<YourController> _logger; 

        //构造函数
        public YourController(YourDbContext dbContext, ILogger<YourController> logger)
        {
            this._dbContext = dbContext; 
            this._logger = logger;      
        }

        //HTTP GET方法 获取指定产品的图片
        [HttpGet("GetProductImage")] 
        public IActionResult GetProductImage(string productId)
        {
            //检查产品ID是否为空或null
            if (string.IsNullOrEmpty(productId))
            {
                return BadRequest("Product ID is required.");
            }

            try
            {
                var product = _dbContext.PRODUCTS.FirstOrDefault(p => p.PRODUCT_ID == productId);

                //检查是否找到了产品以及产品是否有图片，如果没有找到或没有图片
                if (product == null || product.PRODUCT_PIC == null)
                {
                    return NotFound("No image found for the specified product.");
                }

                return File(product.PRODUCT_PIC, "image/jpeg"); //返回商品图片
            }
            catch (Exception ex) //捕获异常
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                return BadRequest($"An error occurred while fetching the image: {ex.Message}");
            }
        }
    }
}
