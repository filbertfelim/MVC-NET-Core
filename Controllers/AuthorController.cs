using Microsoft.AspNetCore.Mvc;
using MvcBook.Models;
using MvcBook.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly AuthorService _authorService;

        public AuthorController(AuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
            var author = await _authorService.GetAllAuthors();
            return Ok(new { data = author});
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetAuthor(int id)
        {
            var author = await _authorService.GetAuthorById(id);

            if (author == null)
            {
                return NotFound(new { message = $"Author with id {id} was not found." });
            }

            return Ok(new { data = author});
        }

        [HttpPost]
        public async Task<ActionResult<Author>> CreateAuthor(Author author)
        {
            try
            {
                if (author == null)
                {
                    return BadRequest(new { message = "Invalid author data request." });
                }

                var createdAuthor = await _authorService.CreateAuthor(author);
                return CreatedAtAction(nameof(GetAuthor), new { id = createdAuthor.AuthorId }, new { data = createdAuthor});
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the author.", details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, Author author)
        {
            if (id != author.AuthorId)
            {   
                return BadRequest(new { message = "Author ID mismatch." });
            }

            var findAuthor = await _authorService.GetAuthorById(id);
            if (findAuthor == null)
            {
                return BadRequest(new { message = "No author with such ID" });
            }
            try
            {
                await _authorService.UpdateAuthor(author);
                return Ok(new { message = $"Author with ID {id} successfully updated", data = author });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the author.", details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var findAuthor = await _authorService.GetAuthorById(id);
            if (findAuthor == null)
            {
                return BadRequest(new { message = "No author with such ID" });
            }
            await _authorService.DeleteAuthor(id);
            return NoContent();
        }
    }
}
