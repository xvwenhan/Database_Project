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

        [HttpGet("{imageId}")]
        public IActionResult GetImage(string imageId)
        {
            var image = _context.POST_IMAGES.FirstOrDefault(i => i.IMAGE_ID == imageId);
            if (image != null)
            {
                return File(image.IMAGE, "image/jpeg"); //根据图片类型调整MIME类型
            }
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
    }
}
