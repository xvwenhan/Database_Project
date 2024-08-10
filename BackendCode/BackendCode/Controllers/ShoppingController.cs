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

        /***************************************/
        /* 获取店铺所有商品分类接口            */
        /* 给出店铺ID-storeId                  */
        /* 返回店铺所有自定义分类名称列表      */
        /***************************************/
        [HttpGet("GetStoreTags")]
        public async Task<IActionResult> GetStoreTagsAsync(string storeId)
        {
            /* 查询商家账号信息 */
            var store = await _dbContext.STORES.FirstOrDefaultAsync(a => a.ACCOUNT_ID == storeId);
            if (store == null) //用户ID不存在
            {
                return NotFound("未找到该商家账号");
            }

            /* 查询店铺的所有自定义分类名称 */
            var categoryNames = await _dbContext.PRODUCTS
                .Where(c => c.ACCOUNT_ID == storeId)
                .Select(c => c.STORE_TAG)
                .Distinct() //确保分类名称唯一
                .ToListAsync();

            /* 返回分类名称列表 */
            return Ok(categoryNames);
        }

        /***************************************/
        /* 获取商品详情信息接口                */
        /* 传入{userid,productid}              */
        /* 返回商品的详细信息                  */
        /***************************************/
        [HttpGet("GetProductDetails")]
        public async Task<IActionResult> GetProductDetailsAsync(string userId, string productId)
        {
            /* 查询商品信息 */
            var product = await _dbContext.PRODUCTS.FirstOrDefaultAsync(a => a.PRODUCT_ID == productId);
            if (product == null) //商品ID不存在
            {
                return NotFound("未找到该商品信息");
            }

            /* 查询买家信息 */
            var buyer = await _dbContext.BUYERS.FirstOrDefaultAsync(a => a.ACCOUNT_ID == userId);
            if (buyer == null) //买家ID不存在
            {
                return NotFound("未找到买家信息");
            }

            /* 查询商家信息 */
            var store = await _dbContext.STORES.FirstOrDefaultAsync(a => a.ACCOUNT_ID == product.ACCOUNT_ID);
            if (store == null) //买家ID不存在
            {
                return NotFound("未找到商家信息");
            }

            /* 查询市集相关信息 */
            var marketProduct = await _dbContext.MARKET_PRODUCTS.FirstOrDefaultAsync(mp => mp.PRODUCT_ID == productId);

            /* 计算折扣价格 */
            decimal discountPrice = marketProduct != null ? marketProduct.DISCOUNT_PRICE : product.PRODUCT_PRICE;

            /* 查询用户是否收藏该商品 */
            var isProductStared = await _dbContext.BUYER_PRODUCT_BOOKMARKS
                .AnyAsync(bpb => bpb.BUYER_ACCOUNT_ID == userId && bpb.PRODUCT_ID == productId); //检查商品是否被收藏

            /* 查询店铺是否收藏该商品 */
            var isStoreStared = await _dbContext.BUYER_STORE_BOOKMARKS
                .AnyAsync(bsb => bsb.BUYER_ACCOUNT_ID == userId && bsb.STORE_ACCOUNT_ID == store.ACCOUNT_ID); //检查店铺是否被收藏

            /* 创建商品详情DTO */
            var productDetails = new ProductDetailsDTO
            {
                Name = product.PRODUCT_NAME, //商品名称
                Picture = product.PRODUCT_PIC, //商品图片
                Price = product.PRODUCT_PRICE, //商品价格
                Description = product.DESCRIBTION, //商品描述
                StoreName = store.STORE_NAME, //店铺名称
                StoreId = store.ACCOUNT_ID, //店铺ID
                DiscountPrice = discountPrice, //折扣价格
                FromWhere = store.ADDRESS, //发货地
                Score = store.STORE_SCORE, //店铺评分
                IsProductStared = isProductStared, //商品是否被收藏
                IsStoreStared = isStoreStared //店铺是否被收藏
            };

            return Ok(productDetails); //返回商品详情信息
        }

        /***************************************/
        /* 获取商品详情信息接口-商品图片       */
        /***************************************/
        [HttpGet("GetProductPic")]
        public async Task<IActionResult> GetProductPicAsync(string productId)
        {
            /* 查询商品信息 */
            var product = await _dbContext.PRODUCTS.FirstOrDefaultAsync(a => a.PRODUCT_ID == productId);
            if (product == null) //商品ID不存在
            {
                return NotFound("未找到该商品信息");
            }

            /* 获取产品图片的BLOB数据 */
            var pictureResult = product.PRODUCT_PIC;
            if (pictureResult == null)
            {
                return NotFound("未找到该商品图片");
            }

            return File(product.PRODUCT_PIC, "image/jpeg"); //返回商品图片
        }

        /***************************************/
        /* 获取买家消费积分接口                */
        /* 传入买家ID-buyerId                  */
        /* 返回买家的当前积分                  */
        /***************************************/
        [HttpGet("GetBuyerCredits")]
        public async Task<IActionResult> GetBuyerCreditsAsync(string buyerId)
        {
            /* 查询买家信息 */
            var buyer = await _dbContext.BUYERS.FirstOrDefaultAsync(a => a.ACCOUNT_ID == buyerId);
            if (buyer == null) //买家ID不存在
            {
                return NotFound("未找到该买家信息");
            }

            return Ok(buyer.TOTAL_CREDITS); //返回买家的总积分
        }

        /********************************/
        /* 传入订单评分和评价内容       */
        /* 传入 orderId, score, remark  */
        /* 更新订单评分和评价内容       */
        /********************************/
        [HttpPut("OrderRemark")]
        public async Task<IActionResult> OrderRemarkAsync([FromForm] OrderRemarkDTO orderRemarkDto)
        {
            /* 验证订单是否存在 */
            var order = await _dbContext.ORDERS.FirstOrDefaultAsync(o => o.ORDER_ID == orderRemarkDto.orderId);
            if (order == null) //订单不存在
            {
                return NotFound("未找到订单");
            }

            /* 更新订单评分和评价内容 */
            order.SCORE = orderRemarkDto.score; //订单评分
            order.REMARK = orderRemarkDto.remark; //评价内容
            
            await _dbContext.SaveChangesAsync(); //保存更改到数据库

            return Ok("订单评价已提交"); //返回成功响应
        }
    }
}