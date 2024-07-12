using BackendCode.Data;
using BackendCode.DTOs;
using BackendCode.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace BackendCode.Controllers
{
    public class PostController : Controller
    {
        private readonly YourDbContext _context;

        public PostController(YourDbContext context)
        {
            _context = context;
        }
        // 发布帖子
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePost([FromBody] PostModel post)
        {
            //从cookie中提取用户ID
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            POST newpost = new POST()
            {
                POST_ID = $"{DateTime.Now}",
                RELEASE_TIME = DateTime.UtcNow,
                POST_TITLE = post.PostTitle,
                POST_CONTENT = post.PostContent,
                NUMBER_OF_LIKES = 0,
                NUMBER_OF_COMMENTS = 0,
                ACCOUNT_ID = userId.ToString()
            };
            _context.POSTS.Add(newpost);
            await _context.SaveChangesAsync();
            return Ok(newpost);//返回信息中含有帖子ID
        }

        // 删除帖子 不需要进行身份验证（方便管理员删） 用户只能删自己的帖子，所以不会出问题
        [HttpDelete("{PostId}")]
        [Authorize]
        public async Task<IActionResult> DeletePost(int PostId)
        {
            var post = await _context.POSTS.FindAsync(PostId);
            if (post == null)
            {
                return NotFound();
            }
            _context.POSTS.Remove(post);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // 添加评论
        [HttpPost("{PostId}/add_comment")]
        public async Task<IActionResult> AddComment([FromBody] CommentModel comment)
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
                return NotFound();
            }
            post.NUMBER_OF_COMMENTS++;//增加
            COMMENT_POST newcomment = new COMMENT_POST()
            {
                BUYER_ACCOUNT_ID = userId,
                POST_ID = comment.PostId,
                EVALUATION_TIME = DateTime.UtcNow,
                EVALUATION_COMTENT =comment.CommentContext
            };
            _context.COMMENT_POSTS.Add(newcomment);
            await _context.SaveChangesAsync();
            return Ok(comment);
        }

        //删除评论
        [HttpDelete("{PostId}/delete_comment")]
        public async Task<IActionResult> DeleteComment([FromBody] CommentDeleteModel comment_delete_info)
        {
            //从cookie中提取用户ID
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            if (userId!= comment_delete_info.CommentPeopleId)//判断逻辑还得再加个不是管理员
                return BadRequest("无权限删除此评论！");
            COMMENT_POST comment_to_delete = await _context
                .COMMENT_POSTS.FindAsync(comment_delete_info.CommentPeopleId, comment_delete_info.PostId);
            /////////////////////这个地方写查询写的可能不对
            var post = await _context.POSTS.FindAsync(comment_delete_info.PostId);
            if (post == null)
            {
                return NotFound();
            }
            post.NUMBER_OF_COMMENTS--;//减少
            await _context.SaveChangesAsync();
            return Ok(1);
        }

        // 获取自己写的帖子（要不要获取帖子下的评论？）
        [HttpGet("get_post")]
        public async Task<IActionResult> GetPost()
        {
            //从cookie中提取用户ID
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            var post = await _context.POSTS.FirstOrDefaultAsync(p => p.ACCOUNT_ID == userId);
            //目前查询：只查得到一条，语法后面再改
            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }



    }
}
