using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using BackendCode.Data;
using Alipay.AopSdk.AspnetCore;
using Alipay.AopSdk.F2FPay.Business;
using Alipay.AopSdk.F2FPay.Domain;
using Alipay.AopSdk.F2FPay.Model;
using System;
using System.Collections.Generic;
using System.DrawingCore;
using System.DrawingCore.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using QRCoder;
using Alipay.AopSdk.Core.Domain;
using Alipay.AopSdk.Core.Request;
using BackendCode.DTOs.Payment;
using Microsoft.EntityFrameworkCore;

namespace BackendCode.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlipayController : ControllerBase
    {
        private readonly YourDbContext _dbContext;
        private readonly AlipayService _alipayService;

        public AlipayController(YourDbContext context, AlipayService alipayService)
        {
            _dbContext = context;
            _alipayService = alipayService;
        }

        //支付宝支付
        [HttpPost]
        public async Task<IActionResult> PayRequest([FromForm] AlipayDTO alipayDTO)
        {
            var order = await _dbContext.ORDERS.FirstOrDefaultAsync(o => o.ORDER_ID == alipayDTO.orderID);
            var product = await _dbContext.PRODUCTS.FirstOrDefaultAsync(o => o.PRODUCT_ID == order.PRODUCT_ID);

            //组装业务参数model
            AlipayTradePagePayModel model = new AlipayTradePagePayModel
            {
                Body = product.DESCRIBTION,        //商品描述
                Subject = product.PRODUCT_NAME,    //商品名称
                TotalAmount = alipayDTO.actualPay, //付款价格
                OutTradeNo = alipayDTO.orderID,    //订单号
                ProductCode = "FAST_INSTANT_TRADE_PAY"
            };

            AlipayTradePagePayRequest request = new AlipayTradePagePayRequest();

            //设置同步回调地址
            request.SetReturnUrl(alipayDTO.returnUrl);
           
            //将业务model载入到request
            request.SetBizModel(model);

            var response = _alipayService.SdkExecute(request);

            //跳转支付宝支付
            return Ok(_alipayService.Options.Gatewayurl + "?" + response.Body);
        }
    }
}