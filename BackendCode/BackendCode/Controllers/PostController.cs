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


namespace BackendCode.Controllers
{
    public class PostController : Controller
    {
        private readonly YourDbContext _context;
        public string filePath = "./Services/post_id.txt";
        public IdGenerator idGenerator;

        public string filePath2 = "./Services/image_id.txt";
        public IdGenerator idGenerator2;
        public PostController(YourDbContext context)
        {
            _context = context;
            idGenerator = new IdGenerator(filePath);
            idGenerator2 = new IdGenerator(filePath2);
        }
        // 发布帖子(仅上传文本版) 现在用不了
        [HttpPost("make_post")]
        [Authorize]
        public async Task<IActionResult> CreatePost111([FromBody] PostModel post)
        {
            //从cookie中提取用户ID
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            POST newpost = new POST()
            {
                POST_ID = idGenerator.GetNextId(),
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
                POST_ID = idGenerator.GetNextId(),
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
                            IMAGE_ID=idGenerator2.GetNextId(),
                            IMAGE = imageData
                        };
                        _context.POST_IMAGES.Add(postImage);
                    }
                }
                await _context.SaveChangesAsync();
            }

            return Ok(new { message="帖子上传成功！",postId = newPost.POST_ID }); // 返回信息中含有帖子ID
        }








        // 删除帖子 不需要进行身份验证（方便管理员删） 用户只能删自己的帖子，所以不会出问题
        [HttpDelete("delete_post")]
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
        [HttpPost("add_comment")]
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



/*[HttpPost("make_post")]
[Authorize]
public async Task<IActionResult> CreatePost([FromForm] PostModel post)
{
    // 确保模型有效
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    // 从cookie中提取用户ID
    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (userId == null)
    {
        return Unauthorized();
    }

    // 创建新的帖子
    var newPost = new POST
    {
        POST_ID = Guid.NewGuid().ToString(), // 使用 Guid 生成唯一的 POST_ID
        RELEASE_TIME = DateTime.UtcNow,
        POST_TITLE = post.PostTitle,
        POST_CONTENT = post.PostContent,
        NUMBER_OF_LIKES = 0,
        NUMBER_OF_COMMENTS = 0,
        ACCOUNT_ID = userId.ToString(),
        Images = new List<PostImage>() // 初始化图片集合
    };

    // 处理图片上传
    if (post.Images != null && post.Images.Any())
    {
        foreach (var image in post.Images)
        {
            using (var memoryStream = new MemoryStream())
            {
                await image.CopyToAsync(memoryStream);
                var imageData = memoryStream.ToArray();

                var postImage = new PostImage
                {
                    PostId = newPost.POST_ID,
                    ImageData = imageData,
                    ImageContentType = image.ContentType
                };

                newPost.Images.Add(postImage);
            }
        }
    }

    _context.POSTS.Add(newPost);
    await _context.SaveChangesAsync();

    return Ok(newPost); // 返回信息中含有帖子ID和图片信息
}
*/