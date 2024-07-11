using Microsoft.AspNetCore.Mvc;           
using BackendCode.Data;                  
using System.IO;                          
using Microsoft.AspNetCore.Http;          

namespace BackendCode.Controllers
{
    [ApiController]               //API������
    [Route("api/[controller]")]   //���ÿ������Ļ���·��
    public class YourController : ControllerBase
    {
        private readonly YourDbContext _dbContext; 
        private readonly ILogger<YourController> _logger; 

        //���캯��
        public YourController(YourDbContext dbContext, ILogger<YourController> logger)
        {
            this._dbContext = dbContext; 
            this._logger = logger;      
        }

        //HTTP GET���� ��ȡָ����Ʒ��ͼƬ
        [HttpGet("GetProductImage")] 
        public IActionResult GetProductImage(string productId)
        {
            //����ƷID�Ƿ�Ϊ�ջ�null
            if (string.IsNullOrEmpty(productId))
            {
                return BadRequest("Product ID is required.");
            }

            try
            {
                var product = _dbContext.PRODUCTS.FirstOrDefault(p => p.PRODUCT_ID == productId);

                //����Ƿ��ҵ��˲�Ʒ�Լ���Ʒ�Ƿ���ͼƬ�����û���ҵ���û��ͼƬ
                if (product == null || product.PRODUCT_PIC == null)
                {
                    return NotFound("No image found for the specified product.");
                }

                return File(product.PRODUCT_PIC, "image/jpeg"); //������ƷͼƬ
            }
            catch (Exception ex) //�����쳣
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                return BadRequest($"An error occurred while fetching the image: {ex.Message}");
            }
        }
    }
}
