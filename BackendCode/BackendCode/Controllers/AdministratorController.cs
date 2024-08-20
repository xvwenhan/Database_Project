using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendCode.Data;
using BackendCode.DTOs.Administrator;
using Yitter.IdGenerator;


namespace Administrator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdministratorController : ControllerBase
    {
        private readonly YourDbContext _dbContext;
        

        public AdministratorController(YourDbContext context)
        {
            _dbContext = context;
        }


        [HttpPost("GetAllAuthentication")]
        public async Task<IActionResult> GetAllAuthenticationAsync([FromBody]GAAModel model)
        {

            // 获取所有申请信息
            var authentications = await _dbContext.SUBMIT_AUTHENTICATIONS
                .Where(p => p.ADMINISTRATOR_ACCOUNT_ID == model.adminId)
                .ToListAsync();

            if (authentications == null || !authentications.Any())
            {
                return NotFound("No authentication was found");
            }

            var allAuthenticationDtos = authentications.Select(p => new ShowAuthenticationDTO
            {
                StoreId =p.STORE_ACCOUNT_ID,
                Authentication =p.AUTHENTICATION,
                Status=p.STATUS,
                Photo = p.PHOTO != null ? Convert.ToBase64String(p.PHOTO) : null,

            }).ToList();
            // 返回商品信息
            return Ok(allAuthenticationDtos);
        }

        //修改
        [HttpPut("UpdateStoreAuthentication")]
        public async Task<IActionResult> UpdateStoreAuthentication([FromBody] USAModel model)
        {
            // 查找提交的认证
            var temp = await _dbContext.SUBMIT_AUTHENTICATIONS
                .FirstOrDefaultAsync(a => a.STORE_ACCOUNT_ID == model.storeId);

            // 查找商店
            var tempstore = await _dbContext.STORES
                .FirstOrDefaultAsync(b => b.ACCOUNT_ID == model.storeId);

            if (temp == null)
            {
                return NotFound("No authentication found for the given store.");
            }

            // 确保商店存在
            if (tempstore == null)
            {
                return NotFound("Store not found.");
            }

            if (model.result)
            {
                temp.STATUS = "已通过";
                temp.ADMINISTRATOR_ACCOUNT_ID = model.adminId;
                tempstore.CERTIFICATION = true;
            }
            else
            {
                temp.STATUS = "已拒绝";
                temp.ADMINISTRATOR_ACCOUNT_ID = model.adminId;
                tempstore.CERTIFICATION = false;
            }

            // 保存更改
            try
            {
                await _dbContext.SaveChangesAsync();
                return Ok($"申请已被处理，处理结果:{temp.STATUS}，处理人:{model.adminId}");
            }
            catch (DbUpdateException ex)
            {
                // 捕获数据库更新异常并返回错误消息
                return StatusCode(500, $"An error occurred while updating the database: {ex.Message}");
            }
            
        }



        //添加市集并发出邀请
        [HttpPut("AddMarket")]
        public async Task<IActionResult> AddMarket([FromForm] AMModel model)
        {
           /* Random random = new();
            int _marketId = random.Next(1, 10000000);
            string uidb = _marketId.ToString();
            uidb = "M" + uidb;*/
            var ms = new MemoryStream();
            var image = model.posterImg[0];
            await image.CopyToAsync(ms);
            var imageData = ms.ToArray();

            string uidb = YitIdHelper.NextId().ToString();
            uidb = "M" + uidb;

            _dbContext.MARKETS.Add(new BackendCode.Models.MARKET()
            {
                MARKET_ID=uidb,
                THEME=model.theme,
                START_TIME=model.startTime,
                END_TIME=model.endTime,
                DETAIL=model.detail,
                POSTERIMG=imageData,
            });
            _dbContext.SaveChanges();

            _dbContext.SET_UP_MARKETPLACES.Add(new BackendCode.Models.SET_UP_MARKETPLACE()
            {
                MARKET_ID = uidb,
                ADMINISTRATOR_ACCOUNT_ID=model.adminId,
            });
            _dbContext.SaveChanges();


            //查找有哪些符合条件的商家
            var storeIds = _dbContext.PRODUCTS
                               .Where(p => p.TAG == model.option)
                               .Select(p => p.ACCOUNT_ID)
                               .Distinct()  // 如果需要唯一的商店 ID，可以加上 Distinct()
                               .ToList();

            foreach (var storeId in storeIds)
            {
                _dbContext.MARKET_STORES.Add(new BackendCode.Models.MARKET_STORE()
                {
                    MARKET_ID = uidb,
                    STORE_ACCOUNT_ID = storeId,
                    IN_OR_NOT=false,
                }) ;
                _dbContext.SaveChanges();
            }


            // 保存更改
            try
            {
                await _dbContext.SaveChangesAsync();
                return Ok($"市集添加成功，市集id:{uidb}");
            }
            catch (DbUpdateException ex)
            {
                // 捕获数据库更新异常并返回错误消息
                return StatusCode(500, $"An error occurred while updating the database: {ex.Message}");
            }

        }


        [HttpGet("GetAllMarket")]
        public async Task<IActionResult> GetAllMarket()
        {

            // 获取所有申请信息
            var markets = _dbContext.MARKETS
                        .OrderBy(m => m.START_TIME)
                        .ToList();

            if (markets == null || !markets.Any())
            {
                return NotFound("现在还没有市集");
            }


            var allmarketDtos = markets.Select(p => new ShowMarketDTO
            {
                marketId=p.MARKET_ID,
                theme=p.THEME,
                startTime=p.START_TIME,
                endTime=p.END_TIME,
                detail=p.DETAIL,
                posterImg= Convert.ToBase64String(p.POSTERIMG),

            }).ToList();
            // 返回商品信息
            return Ok(allmarketDtos);
        }


        [HttpPut("DeleteMarket")]
        public async Task<IActionResult> DeleteMarket([FromBody]DMModel model)
        {
            DateTime currentDateTime = DateTime.Now;
            var startTime = _dbContext.MARKETS
                                    .Where(m => m.MARKET_ID == model.marketId)
                                    .Select(m => m.START_TIME)
                                    .FirstOrDefault();
            if(startTime == null)
            {
                return NotFound("不存在该市集");
            }
            if (currentDateTime >= startTime)
            {
                return Ok("删除失败！无法删除已经开始的市集");
            }
            try
            {
                // 在SET_UP_MARKETPLACES中删除找到的行
                var itemsToDelete = await _dbContext.SET_UP_MARKETPLACES
                    .Where(item => item.MARKET_ID == model.marketId)
                    .ToListAsync();

                _dbContext.SET_UP_MARKETPLACES.RemoveRange(itemsToDelete);
                await _dbContext.SaveChangesAsync();

                // 在MARKET_STORES中删除找到的行
                var itemsToDelete2 = await _dbContext.MARKET_STORES
                    .Where(item => item.MARKET_ID == model.marketId)
                    .ToListAsync();

                _dbContext.MARKET_STORES.RemoveRange(itemsToDelete2);
                await _dbContext.SaveChangesAsync();

                // 在MARKET_PRODUCTS中删除找到的行
                var itemsToDelete3 = await _dbContext.MARKET_PRODUCTS
                    .Where(item => item.MARKET_ID == model.marketId)
                    .ToListAsync();

                _dbContext.MARKET_PRODUCTS.RemoveRange(itemsToDelete3);
                await _dbContext.SaveChangesAsync();

                // 在MARKETS中删除找到的行
                var itemsToDelete4 = await _dbContext.MARKETS
                    .Where(item => item.MARKET_ID == model.marketId)
                    .ToListAsync();

                _dbContext.MARKETS.RemoveRange(itemsToDelete4);
                await _dbContext.SaveChangesAsync();


                return Ok($"删除市集成功！删除市集编号： {model.marketId}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"删除市集失败: {ex.Message}");
            }
        }


        [HttpGet("GetAllReport")]
        public async Task<IActionResult> GetAllReport()
        {
            try
            {
                var query = from report in _dbContext.REPORTS
                            join complainPost in _dbContext.COMPLAIN_POSTS on report.REPORT_ID equals complainPost.REPORT_ID
                            join post in _dbContext.POSTS on complainPost.POST_ID equals post.POST_ID
                            select new ShowReportDTO
                            {
                                reportId = report.REPORT_ID,
                                buyerAccountId = complainPost.BUYER_ACCOUNT_ID,
                                reportingTime = report.REPORT_TIME,
                                reportingReason = report.REPORT_REASON,
                                postContent = post.POST_CONTENT,
                                auditResults = report.AUDIT_RESULTS,
                            };
                var res = await query.ToListAsync();
                return Ok(res);
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"访问失败: {ex.Message}");
            }
            
        }



        //修改
        [HttpPut("AuditReport")]
        public async Task<IActionResult> AuditReport([FromBody] ARModel model)
        {
            DateTime currentDateTime = DateTime.Now;
            // 查找reportid
            var reporttoc = await _dbContext.REPORTS
                .FirstOrDefaultAsync(a => a.REPORT_ID == model.reportId);

            if (reporttoc == null)
            {
                return NotFound("不存在该条投诉记录");
            }

            if (model.auditResult=="删除")
            {
                reporttoc.AUDIT_RESULTS = model.auditResult;
                reporttoc.AUDIT_TIME = currentDateTime;
                reporttoc.ADMINISTRATOR_ACCOUNT_ID = model.adminId;
            }
            else
            {
                reporttoc.AUDIT_RESULTS = model.auditResult;
                reporttoc.AUDIT_TIME = currentDateTime;
                reporttoc.ADMINISTRATOR_ACCOUNT_ID = model.adminId;
            }

            // 保存更改
            try
            {
                await _dbContext.SaveChangesAsync();
                return Ok($"举报已被处理，处理结果:已{model.auditResult}，处理人:{model.adminId}");
            }
            catch (DbUpdateException ex)
            {
                // 捕获数据库更新异常并返回错误消息
                return StatusCode(500, $"An error occurred while updating the database: {ex.Message}");
            }

        }
    }

}

