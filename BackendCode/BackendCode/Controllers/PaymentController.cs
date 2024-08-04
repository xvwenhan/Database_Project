using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendCode.Data;
using BackendCode.Models;
using BackendCode.DTOs.Payment;
using Alipay.AopSdk.AspnetCore;
using Alipay.AopSdk.Core.Domain;
using Alipay.AopSdk.Core.Request;
using Alipay.AopSdk.Core.Response;
using Alipay.AopSdk.Core;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace BackendCode.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly YourDbContext _dbContext;

        public PaymentController(YourDbContext context)
        {
            _dbContext = context;
        }

        /********************************/
        /* 创建退货订单接口             */
        /* 传入订单ID和退货理由         */
        /* 在数据库建立退货信息         */
        /********************************/
        [HttpPost("AddReturn")]
        public async Task<IActionResult> AddReturnAsync([FromForm] ReturnRequestDTO returnRequestDto)
        {
            /* 检查订单是否存在 */
            var order = await _dbContext.ORDERS.FirstOrDefaultAsync(o => o.ORDER_ID == returnRequestDto.orderID);
            if (order == null) //订单不存在
            {
                return NotFound("订单不存在，无法退货");
            }

            /* 检查订单状态是否允许退货 */
            if (order.ORDER_STATUS == "待退货" || order.ORDER_STATUS == "已退货")
            {
                return BadRequest("订单已经申请退货");
            }

            /* 创建退货记录对象，并设置退货信息 */
            var returnRecord = new RETURN
            {
                ORDER_ID = order.ORDER_ID,
                RETURN_TIME = DateTime.Now,
                RETURN_REASON = returnRequestDto.ReturnReason,
                RETURN_STATUS = "待处理"
            };

            order.ORDER_STATUS = "待退货"; //更新订单状态为“待退货” 等待商家处理

            _dbContext.RETURNS.Add(returnRecord); //将退货记录对象添加到数据库上下文

            await _dbContext.SaveChangesAsync(); //保存数据库上下文中的更改到数据库

            return Ok(returnRecord); //返回退货信息
        }

        /********************************/
        /* 商家同意退货更新钱包接口     */
        /* 更新买家和商家钱包           */
        /* 返回买家和商家钱包余额       */
        /********************************/
        [HttpPut("HandleReturn")]
        public async Task<IActionResult> HandleReturnAsync(string returnID)
        {
            /* 检查退货记录是否存在 */
            var returnRecord = await _dbContext.RETURNS.FirstOrDefaultAsync(r => r.ORDER_ID == returnID);
            if (returnRecord == null) //退货记录不存在
            {
                return NotFound("退货记录不存在");
            }

            /* 找到对应订单 */
            var order = await _dbContext.ORDERS.FirstOrDefaultAsync(o => o.ORDER_ID == returnRecord.ORDER_ID);
            if (order == null) //未找到订单信息
            {
                return NotFound("相应订单不存在");
            }

            returnRecord.RETURN_STATUS = "已同意"; //更新退货状态

            /* 买家钱包 */
            var buyerWallet = await _dbContext.WALLETS.FirstOrDefaultAsync(w => w.ACCOUNT_ID == order.BUYER_ACCOUNT_ID);
            if (buyerWallet == null)
            {
                return NotFound("买家钱包不存在");
            }

            /* 卖家钱包 */
            var sellerWallet = await _dbContext.WALLETS.FirstOrDefaultAsync(w => w.ACCOUNT_ID == order.STORE_ACCOUNT_ID);
            if (sellerWallet == null)
            {
                return NotFound("卖家钱包不存在");
            }

            /* 分别更新钱包余额 */            
            buyerWallet.BALANCE += order.ACTUAL_PAY; //将订单实付金额退还给买家
            sellerWallet.BALANCE -= order.ACTUAL_PAY; //从卖家钱包中扣除订单实付金额

            order.ORDER_STATUS = "已退货"; //更新订单状态
           
            await _dbContext.SaveChangesAsync(); //保存数据库上下文中的更改到数据库

            /* 返回买家和商家钱包余额 */ 
           
            var buyerWalletBalance = new WalletBalanceDTO //买家余额
            {
                AccountId = buyerWallet.ACCOUNT_ID,
                Balance = buyerWallet.BALANCE
            };
            
            var sellerWalletBalance = new WalletBalanceDTO //商家余额
            {
                AccountId = sellerWallet.ACCOUNT_ID,
                Balance = sellerWallet.BALANCE
            };

            var walletBalances = new List<WalletBalanceDTO> { buyerWalletBalance, sellerWalletBalance };

            return Ok(walletBalances); //返回钱包余额
        }

        /********************************/
        /* 创建订单信息接口             */
        /* 生成订单信息 存入数据库      */
        /* 更新买家钱包余额             */
        /********************************/
        [HttpPost("AddOrders")]
        public async Task<IActionResult> AddOrdersAsync([FromForm] OrderPaymentDTO paymentDto)
        {
            string orderId = Guid.NewGuid().ToString(); //随机生成订单号

            /* 检查订单号是否已存在 */
            var existingOrder = await _dbContext.ORDERS.FirstOrDefaultAsync(o => o.ORDER_ID == orderId);
            //如果订单号已存在，重复生成
            while (existingOrder != null) 
            {
                orderId = Guid.NewGuid().ToString();
                existingOrder = await _dbContext.ORDERS.FirstOrDefaultAsync(o => o.ORDER_ID == orderId);
            }

            /* 找到对应商品 */
            var product = await _dbContext.PRODUCTS.FirstOrDefaultAsync(o => o.PRODUCT_ID == paymentDto.ProductId);
            //商品不存在
            while (product == null)
            {
                return NotFound("未找到商品");
            }

            /* 获取买家钱包信息 */
            var buyerWallet = await _dbContext.WALLETS.FirstOrDefaultAsync(w => w.ACCOUNT_ID == paymentDto.BuyerId);
            if (buyerWallet == null) //买家钱包不存在
            {
                return NotFound("未找到买家钱包");
            }

            /* 获取商家钱包信息 */
            var sellerWallet = await _dbContext.WALLETS.FirstOrDefaultAsync(w => w.ACCOUNT_ID == paymentDto.StoreId);
            if (sellerWallet == null) //如果商家钱包不存在
            {
                return NotFound("未找到商家钱包");
            }

            /* 检查买家钱包余额是否足够 */
            if (buyerWallet.BALANCE < paymentDto.ActualPay)
            {
                return BadRequest("买家钱包余额不足，请及时充值");
            }

            //更新买家钱包余额：从买家钱包中扣除交易金额
            buyerWallet.BALANCE -= paymentDto.ActualPay;

            //更新卖家钱包余额：将交易金额添加到卖家钱包
            sellerWallet.BALANCE += paymentDto.ActualPay;

            product.SALE_OR_NOT = true; //修改商品售出状态

            /* 创建订单对象，并设置订单信息 */
            var order = new ORDERS
            {
                ORDER_ID = orderId, //订单ID
                PRODUCT_ID = paymentDto.ProductId, //商品ID
                BUYER_ACCOUNT_ID = paymentDto.BuyerId, //买家ID
                STORE_ACCOUNT_ID = paymentDto.StoreId, //商家ID
                TOTAL_PAY = product.PRODUCT_PRICE, //订单金额
                ACTUAL_PAY = paymentDto.ActualPay, //实付金额
                ORDER_STATUS = "待发货", //订单状态
                CREATE_TIME = DateTime.Now, //创建时间
                //BONUS_CREDITS = 0, //积分
                RETURN_OR_NOT = false //是否退货
            };

            _dbContext.ORDERS.Add(order); //将订单对象添加到数据库上下文

            await _dbContext.SaveChangesAsync(); //保存数据库上下文中的更改到数据库

            return Ok(order); //返回订单信息
        }

        /********************************/
        /* 买家充值时更新钱包余额接口   */
        /********************************/
        [HttpPost("RechargeWallet")]
        public async Task<IActionResult> RechargeWalletAsync([FromForm] RechargeDTO rechargeDto)
        {
            /* 获取买家钱包信息 */
            var wallet = await _dbContext.WALLETS.FirstOrDefaultAsync(w => w.ACCOUNT_ID == rechargeDto.BuyerId);
            if (wallet == null)
            {
                return NotFound("未找到钱包");
            }

            wallet.BALANCE += rechargeDto.Amount; //更新钱包余额

            await _dbContext.SaveChangesAsync(); //保存更改

            var rechargedWallet = new RechargedWalletDTO
            {
                BuyerId = wallet.ACCOUNT_ID,
                Balance = wallet.BALANCE
            };

            return Ok(rechargedWallet); //返回充值成功的响应，包括充值后的钱包余额
        }

        /********************************/
        /* 查看用户钱包余额接口         */
        /********************************/
        [HttpPost("GetWalletBalance")]
        public async Task<IActionResult> GetWalletBalanceAsync(string accountID)
        {
            /* 获取买家钱包信息 */
            var wallet = await _dbContext.WALLETS.FirstOrDefaultAsync(w => w.ACCOUNT_ID == accountID);
            if (wallet == null)
            {
                return NotFound("未找到钱包");
            }

            return Ok(wallet.BALANCE); //返回钱包余额
        }
    }

    /*
    [ApiController]
    [Route("api/[controller]")]
    public class ChargeController : ControllerBase
    {
        private readonly IAopClient _aopClient;

        public ChargeController(IAopClient aopClient)
        {
            _aopClient = aopClient;
        }

       
        // 生成充值二维码接口           
        
        [HttpPost]
        public async Task<IActionResult> ScanCodeGen([FromForm] ChargeScanOrderDTO chargeScanOrderDto)
        {
            /* 检查充值信息 
            if (chargeScanOrderDto == null || chargeScanOrderDto.Charge <= 0)
            {
                return BadRequest("充值金额不正确");
            }

            AlipayTradePrecreateModel model = new AlipayTradePrecreateModel
            {
                OutTradeNo = chargeScanOrderDto.AccountId + "_" + System.Guid.NewGuid().ToString(), //生成唯一的订单号
                TotalAmount = chargeScanOrderDto.Charge.ToString("F2"),
                Subject = "钱包充值",
                Body = "为账户钱包充值",
                //可添加其他参数
            };

            // 序列化 model 到 JSON 字符串
            string bizContent = JsonConvert.SerializeObject(model);

            AlipayTradePrecreateRequest request = new AlipayTradePrecreateRequest
            {
                BizContent = bizContent // 赋值序列化后的 JSON 字符串给 BizContent
            };

            AlipayTradePrecreateResponse response = await _aopClient.ExecuteAsync(request);

            if (response != null && response.QrCode != null)
            {
                // 返回生成的二维码字符串
                return Ok(new { QrCode = response.QrCode });
            }
            else
            {
                // 处理错误情况
                return BadRequest("生成二维码失败：" + response.Msg + "，" + response.SubMsg);
            }
        }
    }*/
}