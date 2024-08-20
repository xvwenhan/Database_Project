﻿//有关管理员的DTO
namespace BackendCode.DTOs.Administrator
{
    //管理员查看所有商家认证申请
    public class ShowAuthenticationDTO
    {
        public string? StoreId { get; set; }
        public string? Authentication { get; set; }
        public string? Status { get; set; }
        public string? Photo { get; set; }

    }

    public class ShowMarketDTO
    {
        public string? marketId { get; set; }
        public string? theme { get; set; }
        public DateTime? startTime { get; set; }
        public DateTime? endTime { get; set; }
        public string? detail { get; set; }
        public string? posterImg { get; set; }
    }

    public class ShowReportDTO
    {
        public string? reportId { get; set; }
        public string? buyerAccountId { get; set; }//举报人ID
        public DateTime? reportingTime { get; set; }//举报时间
        public DateTime? postTime { get; set; }//帖子发布时间
        public string? reportingReason { get; set; }//举报原因
        public string? postContent { get; set; }//帖子内容
        public string? postTitle { get; set; }//帖子标题
        public string? auditResults { get; set; }//审核结果
        public List<string>? postImages { get; set; }//帖子图片
    }

    //传入的数据结构
    public class GAAModel
    {
        public string? adminId { get; set; }
    }

    public class USAModel
    {
        public string? storeId { get; set; }
        public bool result { get; set; }
        public string? adminId { get; set; }
    }

    public class AMModel
    {
        public string? theme { get; set; }
        public string? option { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public string? detail { get; set; }
        public List<IFormFile>? posterImg { get; set; }
        public string? adminId { get; set; }
    }
    public class DMModel
    {
        public string? marketId { get; set; }
    }
    public class ARModel
    {
        public string? reportId { get; set; }
        public string? auditResult { get; set; }
        public string? adminId { get; set; }
    }
    
}
