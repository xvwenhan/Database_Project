using BackendCode.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using BackendCode.DTOs.Store;
using BackendCode.Models;
using Alipay.AopSdk.Core.Domain;

namespace ClassificationController.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassificationController : ControllerBase
    {
        private readonly YourDbContext _dbContext;
        private readonly ILogger<ClassificationController> _logger;

        public ClassificationController(YourDbContext dbContext, ILogger<ClassificationController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        [HttpGet("GetCategoryByName")]
        public async Task<IActionResult> GetCategoryByName(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
            {
                return BadRequest("Category name is required.");
            }

            try
            {
                var category = await _dbContext.CATEGORYS
                    .Where(c => c.CATEGORY_NAME == categoryName)
                    .Select(c => new
                    {
                        c.CATEGORY_PIC,
                        c.CATEGORY_DESCRIPTION
                    })
                    .FirstOrDefaultAsync();

                if (category == null)
                {
                    return NotFound("Category not found.");
                }

                return Ok(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting category by name.");
                return StatusCode(500, "Internal server error.");
            }
        }

    }
}
