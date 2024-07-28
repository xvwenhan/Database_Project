using BackendCode.Data;
using BackendCode.DTOs;
using BackendCode.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using BackendCode.Services;
using System.Reflection.Emit;


using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.Design;


namespace BackendCode.Controllers
{
    public class PostController : Controller
    {
        private readonly YourDbContext _context;
        public string filePath = "./Services/post_id.txt";
        public IdGenerator postIdGenerator;

        public string filePath2 = "./Services/image_id.txt";
        public IdGenerator imageIdGenerator;

        public string filePath3 = "./Services/comment_id.txt";
        public IdGenerator commentIdGenerator;

        public string filePath4 = "./Services/report_id.txt";
        public IdGenerator reportIdGenerator;
        public PostController(YourDbContext context)
        {
            _context = context;
            postIdGenerator = new IdGenerator(filePath);
            imageIdGenerator = new IdGenerator(filePath2);
            commentIdGenerator = new IdGenerator(filePath3,10);//id为10位
            reportIdGenerator = new IdGenerator(filePath4);
        }

        //发布帖子 （同时传文本和不定数量的图片）（注意得先登录才能成功调试此接口）
        [HttpPost("create_post")]
        [Authorize]
        public async Task<IActionResult> CreatePost([FromForm] PostModel post)
        {
            // 从cookie中提取用户ID
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            // 创建新帖子
            var newPost = new POST()
            {
                POST_ID = postIdGenerator.GetNextId(),
                RELEASE_TIME = DateTime.UtcNow,
                POST_TITLE = post.PostTitle,
                POST_CONTENT = post.PostContent,
                NUMBER_OF_LIKES = 0,
                NUMBER_OF_COMMENTS = 0,
                ACCOUNT_ID = userId.ToString()
            };

            _context.POSTS.Add(newPost);
            await _context.SaveChangesAsync();

            // 处理图片上传
            //.Any检查集合中是否有任何元素（防止是空集）
            if (post.Images != null && post.Images.Any())
            {
                foreach (var image in post.Images)
                {
                    using (var ms = new MemoryStream())
                    {
                        await image.CopyToAsync(ms);
                        var imageData = ms.ToArray();

                        var postImage = new POST_IMAGE
                        {
                            POST_ID = newPost.POST_ID,
                            IMAGE_ID=imageIdGenerator.GetNextId(),
                            IMAGE = imageData
                        };
                        _context.POST_IMAGES.Add(postImage);
                    }
                }
                await _context.SaveChangesAsync();
            }

            return Ok(new { message="帖子上传成功！",postId = newPost.POST_ID }); // 返回信息中含有帖子ID
        }

        //删除帖子 不需要进行身份验证（方便管理员删） 用户只能删自己的帖子，所以不会出问题
        [HttpDelete("delete_post")]
        [Authorize]
        public async Task<IActionResult> DeletePost(string postId)
        {
            // 从cookie中提取用户ID
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            // 查找帖子
            var post = await _context.POSTS.FindAsync(postId);
            if (post == null)
            {
                return NotFound(new { message = "帖子未找到。" });
            }

            // 确认用户是否有权限删除该帖子
            if (post.ACCOUNT_ID != userId)
            {
                return Forbid();
            }
            //已经换成级联删除ON DELETE CASCADE;
/*            // 删除相关图片
            var images = _context.POST_IMAGES.Where(img => img.POST_ID == postId);
            _context.POST_IMAGES.RemoveRange(images);*/
/*
            // 删除帖子下的所有评论
            var comments = _context.COMMENT_POSTS.Where(c => c.POST_ID == postId);
            _context.COMMENT_POSTS.RemoveRange(comments);

            // 删除帖子下的所有点赞记录
            var likes = _context.LIKE_POSTS.Where(l => l.POST_ID == postId);
            _context.LIKE_POSTS.RemoveRange(likes);*/

            // 删除帖子
            _context.POSTS.Remove(post);

            await _context.SaveChangesAsync();

            return Ok(new { message = "帖子删除成功。",operator_id=userId});
        }

        //为帖子添加评论
        [HttpPost("add_comment_to_post")]
        [Authorize]
        public async Task<IActionResult> AddCommentToPost([FromBody] CommentModel comment)
        {
            //从cookie中提取用户ID
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            var post = await _context.POSTS.FindAsync(comment.PostId);
            if (post == null)
            {
                return NotFound(new {message="您评论的帖子不存在！"});
            }
            post.NUMBER_OF_COMMENTS++;//原帖子增加评论数
            COMMENT_POST newcomment = new COMMENT_POST()
            {
                COMMENT_ID=commentIdGenerator.GetNextId(),
                BUYER_ACCOUNT_ID = userId,
                POST_ID = comment.PostId,
                EVALUATION_TIME = DateTime.UtcNow,
                EVALUATION_COMTENT =comment.CommentContext
            };
            _context.COMMENT_POSTS.Add(newcomment);
            await _context.SaveChangesAsync();
            return Ok(new { message = "评论帖子成功！", comment_id = newcomment.COMMENT_ID });
        }

        //回复评论（即评论的评论）注意：仅支持二级评论，不支持给帖子评论的评论再添加评论
        [HttpPost("add_comment_to_comment")]
        [Authorize]
        public async Task<IActionResult> AddCommentToComment([FromBody] CommentModel2 comment)
        {
            //从cookie中提取用户ID
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            var target_comment = await _context.COMMENT_POSTS.FindAsync(comment.CommentId);
            if (target_comment == null)
            {
                return NotFound(new { message = "您想回复的评论不存在！" });
            }
            var target_post = await _context.POSTS.FindAsync(target_comment.POST_ID);
            if (target_post == null)
                return NotFound(new { message = "原帖子不存在！" });
            target_post.NUMBER_OF_COMMENTS++;//原帖子增加评论数
            COMMENT_COMMENT newcomment = new COMMENT_COMMENT()
            {
                COMMENT_ID = commentIdGenerator.GetNextId(),
                ACCOUNT_ID = userId,
                COMMENTED_COMMENT_ID = comment.CommentId,
                COMMENT_TIME = DateTime.UtcNow,
                COMMENT_COMTENT = comment.CommentContext
            };
            _context.COMMENT_COMMENTS.Add(newcomment);
            await _context.SaveChangesAsync();
            return Ok(new {message="评论回复成功！",comment_id= newcomment.COMMENT_ID});
        }

        //删除评论
        [HttpDelete("/delete_comment_to_post")]
        [Authorize]
        public async Task<IActionResult> DeleteCommentToPost([FromBody] CommentDeleteModel comment_delete_info)
        {
            //从cookie中提取用户ID
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            var userRole = User.FindFirst("UserRole")?.Value;

            var comment = await _context.COMMENT_POSTS
            .Include(c => c.POST)
            .FirstOrDefaultAsync(c => c.COMMENT_ID == comment_delete_info.CommentId);
            //测试测试测试！！！！！！！！！
            //这里让帖主也可以删除自己帖子下方的评论
            if (userId!= comment.BUYER_ACCOUNT_ID&&userRole!="管理员"&&userId!=comment.POST.ACCOUNT_ID)
                return BadRequest("无权限删除此评论！");
            _context.COMMENT_POSTS.Remove(comment);//删除评论
            var post = await _context.POSTS.FindAsync(comment.POST_ID);
            if (post == null)
            {
                return NotFound();
            }
            post.NUMBER_OF_COMMENTS--;//原帖子减少评论数
            await _context.SaveChangesAsync();
            return Ok(new {message="评论删除成功！",operator_id=userId});
        }

        //删除评论的评论
        [HttpDelete("/delete_comment_to_post")]
        [Authorize]
        public async Task<IActionResult> DeleteCommentToComment([FromBody] CommentDeleteModel comment_delete_info)
        {
            //从cookie中提取用户ID
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            var userRole = User.FindFirst("UserRole")?.Value;

            var comment = await _context.COMMENT_COMMENTS
            .FirstOrDefaultAsync(c => c.COMMENT_ID == comment_delete_info.CommentId);
            if (comment == null)
                return NotFound(new {message="要删除的评论不存在！"});
            //也可以使用 .FirstOrDefaultAsync（）方法逐级查询，但效率不如单个 LINQ 查询高
            var post = await (from cc in _context.COMMENT_COMMENTS
                              join cp in _context.COMMENT_POSTS on cc.COMMENTED_COMMENT_ID equals cp.COMMENT_ID
                              join p in _context.POSTS on cp.POST_ID equals p.POST_ID
                              where cc.COMMENT_ID == comment_delete_info.CommentId
                              select p).FirstOrDefaultAsync();
            if (post == null)
                return NotFound(new { message = "评论所在的帖子不存在！" });
            string postOwner = post.ACCOUNT_ID;
            //测试测试测试！！！！！！！！！
            //这里让帖主也可以删除自己帖子下方的评论
            if (userId != comment.ACCOUNT_ID && userRole != "管理员" && userId!= postOwner)
                return BadRequest("无权限删除此评论！");
            _context.COMMENT_COMMENTS.Remove(comment);//删除评论

            post.NUMBER_OF_COMMENTS--;//原帖子减少评论数
            await _context.SaveChangesAsync();
            return Ok(new { message = "评论删除成功！", operator_id = userId });
        }

        //点赞帖子
        [HttpPost("like_post")]
        public async Task<IActionResult> LikePost([FromBody] LikePostModel like)
        {
            //从cookie中提取用户ID
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            // 检查是否已经点赞
            var existingLike = await _context.LIKE_POSTS
                .FirstOrDefaultAsync(e => e.BUYER_ACCOUNT_ID == userId && e.POST_ID == like.PostId);

            if (existingLike != null)
                return BadRequest(new { message = "您已经点赞过该帖子。" });

            var post = await _context.POSTS
                .FirstOrDefaultAsync(e => e.POST_ID == like.PostId);
            if (post == null)
                return NotFound(new { message = "您喜欢的帖子不存在！" });
            post.NUMBER_OF_LIKES++;//原帖子增加点赞数

            // 创建新的点赞记录
            var likePost = new LIKE_POST
            {
                BUYER_ACCOUNT_ID = userId,
                POST_ID = like.PostId
            };
            _context.LIKE_POSTS.Add(likePost);

            await _context.SaveChangesAsync();

            return Ok(new { message = "帖子点赞成功。" });
        }

        //取消点赞帖子
        [HttpPost("unlike_post")]
        [Authorize]
        public async Task<IActionResult> UnlikePost([FromBody] LikePostModel unlike)
        {
            // 从cookie中提取用户ID
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            // 检查是否存在点赞记录
            var existingLike = await _context.LIKE_POSTS
                .FirstOrDefaultAsync(e => e.BUYER_ACCOUNT_ID == userId && e.POST_ID == unlike.PostId);

            if (existingLike == null)
            {
                return BadRequest(new { message = "您尚未点赞该帖子。" });
            }

            var post = await _context.POSTS
                .FirstOrDefaultAsync(e => e.POST_ID == unlike.PostId);
            if (post == null)
            {
                return NotFound(new { message = "您取消点赞的帖子不存在！" });
            }
            post.NUMBER_OF_LIKES--;// 原帖子减少点赞数

            // 删除点赞记录
            _context.LIKE_POSTS.Remove(existingLike);

            await _context.SaveChangesAsync();

            return Ok(new { message = "取消点赞帖子成功。" });
        }

        //举报帖子
        [HttpPost("report_post")]
        [Authorize]
        public async Task<IActionResult> ReportPost([FromBody] ReportPostModel reportModel)
        {
            // 从cookie中提取用户ID
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            // 检查帖子是否存在
            var post = await _context.POSTS.FirstOrDefaultAsync(e => e.POST_ID == reportModel.PostId);
            if (post == null)
            {
                return NotFound(new { message = "被举报的帖子不存在！" });
            }

            //创建新的举报
            var report = new REPORT
            {
                REPORT_ID = reportIdGenerator.GetNextId(),
                REPORT_REASON = reportModel.ReportReason, // 假设 reportModel 包含 ReportReason 字段
                REPORT_TIME = DateTime.UtcNow
            };
            _context.REPORTS.Add(report);
            //加入举报信息
            var complain = new COMPLAIN_POST
            {
                BUYER_ACCOUNT_ID = userId,
                POST_ID = reportModel.PostId,
                REPORT_ID = report.REPORT_ID
            };
            _context.COMPLAIN_POSTS.Add(complain);
            await _context.SaveChangesAsync();

            return Ok(new { message = "帖子举报成功。" });
        }

        //举报评论 注意只能举报一级评论
        [HttpPost("report_comment")]
        [Authorize]
        public async Task<IActionResult> ReportComment([FromBody] ReportCommentModel reportModel)
        {
            // 从cookie中提取用户ID
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            // 检查评论是否存在
            var comment = await _context.COMMENT_POSTS.FirstOrDefaultAsync(e => e.COMMENT_ID == reportModel.CommentId);
            if (comment == null)
            {
                return NotFound(new { message = "想举报的评论不存在！" });
            }

            // 创建新的举报
            var report = new REPORT
            {
                REPORT_ID = reportIdGenerator.GetNextId(),
                REPORT_REASON = reportModel.ReportReason,
                REPORT_TIME = DateTime.UtcNow
            };
            _context.REPORTS.Add(report);

            // 加入举报信息
            var complain = new COMPLAIN_COMMENT
            {
                ACCOUNT_ID = userId,
                COMMENT_ID = reportModel.CommentId,
                REPORT_ID = report.REPORT_ID
            };
            _context.COMPLAIN_COMMENTS.Add(complain);

            await _context.SaveChangesAsync();

            return Ok(new { message = "评论举报成功。" });
        }

        // 获取自己写的帖子
        [HttpGet("get_my_posts")]
        [Authorize]
        public async Task<IActionResult> GetMyPost()
        {
            //从cookie中提取用户ID
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            // 查询该用户写的所有帖子
            var posts = await _context.POSTS
                .Where(p => p.ACCOUNT_ID == userId)
                .ToListAsync();

            if (posts == null || !posts.Any())
            {
                return NotFound(new { message = "未找到该用户的帖子。" });
            }

            return Ok(new {message="已找到所有帖子",target_posts=posts});
        }

        // 获取某人写的所有帖子
        [HttpGet("get_posts")]
        public async Task<IActionResult> GetPostsByUserId(string userId)
        {
            // 检查用户ID是否为空
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new { message = "用户ID不能为空。" });
            }

            // 查询该用户写的所有帖子
            var posts = await _context.POSTS
                .Where(p => p.ACCOUNT_ID == userId)
                .ToListAsync();

            if (posts == null || !posts.Any())
            {
                return NotFound(new { message = "未找到该用户的帖子。" });
            }

            return Ok(new { message = "已找到所有帖子", target_posts = posts });
        }

        //获取所有帖子
        //获取某帖子的所有评论

    }
}

