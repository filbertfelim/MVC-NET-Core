using Microsoft.AspNetCore.Mvc;
using MvcBook.Models;
using MvcBook.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var category = await _categoryService.GetAllCategories();
            return Ok(new { data = category});
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _categoryService.GetCategoryById(id);

            if (category == null)
            {
                return NotFound(new { message = $"Category with id {id} was not found." });
            }

            return Ok(new { data = category});
        }

        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory(Category category)
        {
            var createdCategory = await _categoryService.CreateCategory(category);
            return CreatedAtAction(nameof(GetCategory), new { id = createdCategory.CategoryId }, new { data = createdCategory});
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, Category category)
        {
            if (id != category.CategoryId)
            {
                return BadRequest(new { message = "Category ID mismatch." });
            }

            
            var findCategory = await _categoryService.GetCategoryById(id);
            if (findCategory == null)
            {
                return BadRequest(new { message = "No category with such ID" });
            }
            try
            {
                await _categoryService.UpdateCategory(category);
                return Ok(new { message = $"Category with ID {id} successfully updated", data = category });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the category.", details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var findCategory = await _categoryService.GetCategoryById(id);
            if (findCategory == null)
            {
                return BadRequest(new { message = "No category with such ID" });
            }
            await _categoryService.DeleteCategory(id);
            return NoContent();
        }
    }
}
