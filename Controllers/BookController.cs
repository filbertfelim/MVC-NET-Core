using Microsoft.AspNetCore.Mvc;
using MvcBook.Data;
using MvcBook.Models;
using MvcBook.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MvcBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly BookService _bookService;

        public BookController(BookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            var books = await _bookService.GetAllBooks();
            return Ok(new {data = books});
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var books = await _bookService.GetBookById(id);

            if (books == null)
            {
                 return NotFound(new { message = $"Book with id {id} was not found." });
            }

            return Ok(new {data = books});
        }

        [HttpPost]
        public async Task<ActionResult<Book>> CreateBook(BookCreateDto bookCreateDto)
        {
            try
            {
                if (bookCreateDto == null)
                {
                    return BadRequest(new { message = "Invalid book data request." });
                }
                var book = new Book
                {
                    Title = bookCreateDto.Title,
                    AuthorId = bookCreateDto.AuthorId
                };
                var createdBook = await _bookService.CreateBook(book, bookCreateDto.CategoryIds);
                return CreatedAtAction(nameof(GetBook), new { id = createdBook.BookId }, new { data = createdBook });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the book.", details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, BookCreateDto bookUpdateDto)
        {
            try
            {
                var findBook = await _bookService.GetBookById(id);
                if (findBook == null)
                {
                    return BadRequest(new { message = "No book with such ID" });
                }
                var book = new Book
                {
                    BookId = id,
                    Title = bookUpdateDto.Title,
                    AuthorId = bookUpdateDto.AuthorId
                };
                var updatedBook = await _bookService.UpdateBook(book, bookUpdateDto.CategoryIds);
                return Ok(new { message = $"Book with ID {id} successfully updated", data = updatedBook });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the book.", details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var findBook = await _bookService.GetBookById(id);
            if (findBook == null)
            {
                return BadRequest(new { message = "No book with such ID" });
            }
            await _bookService.DeleteBook(id);
            return NoContent();
        }
    }
}
