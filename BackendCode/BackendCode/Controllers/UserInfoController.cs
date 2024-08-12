using BackendCode.Data;
using Microsoft.AspNetCore.Mvc;
using BackendCode.DTOs.UserInfo;
using Microsoft.EntityFrameworkCore;

namespace UserInfo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserInfoController : ControllerBase
    {
        private readonly YourDbContext _dbContext;
        private readonly ILogger<UserInfoController> _logger;
       
        public UserInfoController(YourDbContext dbContext, ILogger<UserInfoController> logger)
        {
            this._dbContext = dbContext;
            this._logger = logger;

        }

        // 根据用户id返回个人详细信息
        [HttpGet("detailedInfo")]
        public async Task<IActionResult> ShowUserInfo(string uid)
        {
            var infos = await _dbContext.BUYERS.FirstOrDefaultAsync(a => a.ACCOUNT_ID == uid);

            if (infos == null)
            {
                return NotFound("User not found.");
            }

            var response = new UserInfoDTO
            {
                Id = infos.ACCOUNT_ID,
                UserName = infos.USER_NAME,
                EMAIL = infos.EMAIL,
                Gender = infos.GENDER,
                Age = infos.AGE,
                TotalCredits = infos.TOTAL_CREDITS,
                Address = infos.ADDRESS
            };
            return Ok(response);
        }


        // 根据用户id返回头像和简介
        [HttpPost("GetPhotoAndDescribtion")]
        public async Task<IActionResult> GetPhotoAndDescribtion([FromBody]GPADModel model)
        {
            var infos = await _dbContext.BUYERS
                .Where(b => b.ACCOUNT_ID == model.Id)
                .SingleOrDefaultAsync();

            if (infos == null)
            {
                return NotFound("不存在该用户");
            }

            var response = new PhotoAndDescribtionDTO
            {
                Photo= infos.PHOTO != null ? Convert.ToBase64String(infos.PHOTO) : null,
                Describtion =infos.DESCRIBTION,
            };
            return Ok(response);
        }

        // 修改头像或简介
        [HttpPut("SetPhotoAndDescribtion")]
        public async Task<IActionResult> SetPhotoAndDescribtion([FromBody] SPADModel model)
        {
            string des = "";
            string pho = "";
            // 查找用户
            /*var temp = await _dbContext.ACCOUNTS
                .FirstOrDefaultAsync(a => a.ACCOUNT_ID == model.Id);*/
            var temp = await _dbContext.BUYERS
                .Where(b => b.ACCOUNT_ID == model.Id)
                .SingleOrDefaultAsync();

            if (temp == null)
            {
                return NotFound("不存在该用户");
            }

            if (model.Describtion !=null)
            {
                temp.DESCRIBTION = model.Describtion;
                des = "简介已被成功更改";
            }
            else
            {
                des = "简介未被更改";
            }

            if (model.Photo != null)
            {
                /*var ms = new MemoryStream();
                var image = model.Photo;
                await image.CopyToAsync(ms);
                var imageData = ms.ToArray();*/
                byte[] photoBytes = Convert.FromBase64String(model.Photo);
                temp.PHOTO = photoBytes;
                pho = "头像已被更改";
            }
            else
            {
                pho = "头像未被更改";
            }

            // 保存更改
            try
            {
                await _dbContext.SaveChangesAsync();
                return Ok($"申请已被处理，处理结果:{des}，{pho}");
            }
            catch (DbUpdateException ex)
            {
                // 捕获数据库更新异常并返回错误消息
                return StatusCode(500, $"更新数据库时发生错误： {ex.Message}");
            }
        }

    }
}


