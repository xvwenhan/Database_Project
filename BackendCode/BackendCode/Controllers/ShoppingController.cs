using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendCode.Data;
using BackendCode.Models;
using BackendCode.DTOs.Shopping;

namespace BackendCode.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingController : ControllerBase
    {
        private readonly YourDbContext _dbContext;

        public ShoppingController(YourDbContext context)
        {
            _dbContext = context;
        }

        /***************************************/
        /* 获取店铺名称、店铺评分接口          */
        /* 给出店铺ID-STORE_ID                 */
        /* 返回店铺名称和店铺评分              */
        /***************************************/
        [HttpGet("GetStoreInfo")]
        public async Task<IActionResult> GetStoreNameAndScoreAsync(string storeId)
        {
            /* 查询店铺信息 */
            var store = await _dbContext.STORES.FirstOrDefaultAsync(s => s.ACCOUNT_ID == storeId);
            if (store == null) //店铺ID不存在
            {
                return NotFound("未找到该店铺");
            }

            /* 返回商家信息 */
            var storeInfo = new StoreInfoDTO
            {
                name = store.STORE_NAME, 
                score = store.STORE_SCORE
            };

            return Ok(storeInfo); //返回店铺信息
        }

        /***************************************/
        /* 商家设置认证资料接口                */
        /* 商家上传图片和描述                  */
        /* 将认证信息和图片存入数据库          */
        /***************************************/
        [HttpPut("SetAuthentication")]
        public async Task<IActionResult> SetAuthenticationAsync([FromForm] AuthenticationInfoDTO AuthenticationInfoDto)
        {
            /* 查询商家信息 */
            var store = await _dbContext.STORES.FirstOrDefaultAsync(s => s.ACCOUNT_ID == AuthenticationInfoDto.storeID);
            if (store == null) //不是商家
            { 
                return BadRequest("不是商家，无法设置认证资料");
            }

            /* 检查是否已上传过认证资料 */
            var existingAuth = await _dbContext.SUBMIT_AUTHENTICATIONS.FirstOrDefaultAsync(a => a.STORE_ACCOUNT_ID == store.ACCOUNT_ID);
            if (existingAuth != null)
            {
                return BadRequest("已上传过认证资料");
            }

            /* 随机选择一个管理员账号ID 用于后续审核 */
            var admins = await _dbContext.ADMINISTRATORS.ToListAsync(); //获取所有管理员账号
            if (admins.Count == 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "无法获取管理员账号");
            }
            var randomAdmin = admins[Random.Shared.Next(admins.Count)]; //随机选择一个管理员账号

            /* 图片相关处理 */
            var ms = new MemoryStream(); //创建一个内存流对象
            var image = AuthenticationInfoDto.image[0]; //获取表单中上传的图片数组中的第一个图片
            await image.CopyToAsync(ms); //将图片复制到内存流中
            var imageData = ms.ToArray(); //将内存流中的数据转换为字节数组

            /* 创建新的认证资料 */
            var newAuth = new SUBMIT_AUTHENTICATION
            {
                STORE_ACCOUNT_ID = store.ACCOUNT_ID,
                ADMINISTRATOR_ACCOUNT_ID = randomAdmin.ACCOUNT_ID,
                AUTHENTICATION = AuthenticationInfoDto.description,
                PHOTO = imageData, 
                STATUS = "待审核" 
            };

            _dbContext.SUBMIT_AUTHENTICATIONS.Add(newAuth); //添加到数据库
            await _dbContext.SaveChangesAsync(); //保存更改到数据库

            return Ok("认证资料已上传，待审核"); //返回设置认证资料成功响应
        }

        /***************************************/
        /* 设置商家信息接口                    */
        /* 上传地址、名称、邮箱至数据库        */
        /***************************************/
        [HttpPut("SetStoreInfo")]
        public async Task<IActionResult> SetStoreInfoAsync([FromForm] SetStoreInfoDTO setStoreInfoDto)
        {
            /* 查询商家信息 */
            var store = await _dbContext.STORES.FirstOrDefaultAsync(s => s.ACCOUNT_ID == setStoreInfoDto.storeID);
            if (store == null) //用户非商家
            {
                return BadRequest("您不是商家，无法设置商家信息");
            }

            /* 更新商家信息 */
            store.ADDRESS = setStoreInfoDto.address;
            store.STORE_NAME = setStoreInfoDto.name;
            store.EMAIL = setStoreInfoDto.email;

            await _dbContext.SaveChangesAsync(); //保存更改到数据库

            return Ok("商家信息已更新"); //返回设置商家信息成功响应
        }

        /***************************************/
        /* 设置商家相关底部接口                */
        /* 用户名、密码、邮箱                  */
        /***************************************/
        [HttpPut("GetStoreInfo2")]
        public async Task<IActionResult> SetAccountInfoAsync(string storeID)
        {
            /* 查询商家账号信息 */
            var store = await _dbContext.STORES.FirstOrDefaultAsync(a => a.ACCOUNT_ID == storeID);
            if (store == null) //用户ID不存在
            {
                return NotFound("未找到该商家账号");
            }

            /* 返回商家信息 */
            var storeInfo = new GetStoreInfoDTO
            {
                storeID = storeID,
                name = store.STORE_NAME,
                password = store.PASSWORD,
                email = store.EMAIL
            };

            return Ok(storeInfo); 
        }
    }
}