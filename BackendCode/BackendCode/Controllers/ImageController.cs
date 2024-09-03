using BackendCode.Data;
using Microsoft.AspNetCore.Mvc;

namespace BackendCode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly YourDbContext _context;

        public ImagesController(YourDbContext context)
        {
            _context = context;
        }

        [HttpGet("post/{imageId}")]
        public IActionResult GetPostImage(string imageId)
        {
            var image = _context.POST_IMAGES.FirstOrDefault(i => i.IMAGE_ID == imageId);
            if (image != null)
            {
                return File(image.IMAGE, "image/jpeg"); //根据图片类型调整MIME类型
            }

            return NotFound();
        }

        [HttpGet("product/{imageId}")]
        public IActionResult GetProductImage(string imageId)
        {

            var image2 = _context.PRODUCT_IMAGES.FirstOrDefault(i => i.IMAGE_ID == imageId);
            if (image2 != null)
            {
                return File(image2.IMAGE, "image/jpeg"); //根据图片类型调整MIME类型
            }

            var image3 = _context.PRODUCT_DETAILS.FirstOrDefault(pd => pd.IMAGE_ID == imageId);
            if (image3 != null)
            {
                return File(image3.IMAGE, "image/jpeg"); //根据图片类型调整MIME类型
            }
            return NotFound();
        }

        //获取账号头像
        [HttpGet("account/{accountId}")]
        public IActionResult GetAccountImage(string accountId)
        {
            var image = _context.ACCOUNTS.FirstOrDefault(pd => pd.ACCOUNT_ID == accountId);
            if (image != null)
            {
                return File(image.PHOTO, "image/jpeg"); //根据图片类型调整MIME类型
            }
            else
            {
                var image2 = _context.ACCOUNTS.FirstOrDefault(pd => pd.ACCOUNT_ID == "U00000001");
                if (image2 == null) { return NotFound("原始用户头像不存在") ; }
                return File(image2.PHOTO, "image/jpeg"); //根据图片类型调整MIME类型
            }

           // return NotFound();
        }
        
        [HttpGet("market/{imageId}")]
        public IActionResult GetMarketImage(string imageId)
        {
            Console.WriteLine("hhhhhhhhhh\n");
            var image = _context.MARKETS.FirstOrDefault(pd => pd.IMAGE_ID == imageId);
            if (image != null)
            {
                return File(image.POSTERIMG, "image/jpeg"); //根据图片类型调整MIME类型
            }

            return NotFound();
        }

        [HttpGet("authentication/{imageId}")]
        public IActionResult GetAuthImage(string imageId)
        {
            var image = _context.SUBMIT_AUTHENTICATIONS.FirstOrDefault(pd => pd.STORE_ACCOUNT_ID == imageId);
            if (image != null)
            {
                return File(image.PHOTO, "image/jpeg"); //根据图片类型调整MIME类型
            }

            return NotFound();
        }

        [HttpGet("buyerinfo/{imageId}")]
        public IActionResult GetBuyerInfoImage(string imageId)
        {
            var image = _context.BUYERS.FirstOrDefault(pd => pd.ACCOUNT_ID == imageId);
            if (image != null)
            {
                return File(image.PHOTO, "image/jpeg"); //根据图片类型调整MIME类型
            }

            return NotFound();
        }

        [HttpGet("storeinfo/{imageId}")]
        public IActionResult GetStoreInfoImage(string imageId)
        {
            var image = _context.STORES.FirstOrDefault(pd => pd.ACCOUNT_ID == imageId);
            if (image != null)
            {
                return File(image.PHOTO, "image/jpeg"); //根据图片类型调整MIME类型
            }

            return NotFound();
        }


        //获取大分类照片
        [HttpGet("category/{categoryName}")]
        public IActionResult GetCategoryImage(string categoryName)
        {
            var image = _context.CATEGORYS.FirstOrDefault(pd => pd.CATEGORY_NAME == categoryName);
            if (image != null)
            {
                return File(image.CATEGORY_PIC, "image/jpeg"); //根据图片类型调整MIME类型
            }
             return NotFound();
        }
    }
}
