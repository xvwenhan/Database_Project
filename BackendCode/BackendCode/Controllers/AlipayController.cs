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
//using BackendCode.PCPayment;

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

        #region 发起支付
        /// <summary>
        /// 发起支付请求
        /// </summary>
        /// <param name="tradeno">外部订单号，商户网站订单系统中唯一的订单号</param>
        /// <param name="subject">订单名称</param>
        /// <param name="totalAmout">付款金额</param>
        /// <param name="itemBody">商品描述</param>
        /// <returns></returns>
        [HttpPost]
        public void PayRequest(string tradeno, string subject, string totalAmout, string itemBody)
        {
            // 组装业务参数model
            AlipayTradePagePayModel model = new AlipayTradePagePayModel
            {
                Body = itemBody,
                Subject = subject,
                TotalAmount = totalAmout,
                OutTradeNo = tradeno,
                ProductCode = "FAST_INSTANT_TRADE_PAY"
            };

            AlipayTradePagePayRequest request = new AlipayTradePagePayRequest();
            // 设置同步回调地址
            request.SetReturnUrl($"http://{Request.Host}/Pay/Callback");
            // 设置异步通知接收地址
            //request.SetNotifyUrl("");
            // 将业务model载入到request
            request.SetBizModel(model);

            var response = _alipayService.SdkExecute(request);
            Console.WriteLine($"订单支付发起成功，订单号：{tradeno}");
            //跳转支付宝支付
            Response.Redirect(_alipayService.Options.Gatewayurl + "?" + response.Body);
        }

        #endregion
        
  


    }
        
}