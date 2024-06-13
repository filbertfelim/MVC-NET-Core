using Microsoft.EntityFrameworkCore;
using MvcBook.Data;
using MvcBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcBook.Repositories
{
    public class BookRepository : IRepository<Book>
    {
        private readonly BookContext _context;

        public BookRepository(BookContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAll()
        {
            return await _context.Books
                .Include(b => b.Author)
                .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
                .ToListAsync();
        }

        public async Task<Book> GetById(int id)
        {
            try
            {
                return await _context.Books
                    .Include(b => b.Author)
                    .Include(b => b.BookCategories)
                    .ThenInclude(bc => bc.Category)
                    .FirstOrDefaultAsync(b => b.BookId == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving book with ID {id}: {ex.Message}");
                throw;
            }
        }
        public async Task<Book> Create(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return book;
        }

        public async Task<Book> Update(Book book)
        {
            var existingBook = await _context.Books
                .Include(b => b.BookCategories)
                .FirstOrDefaultAsync(b => b.BookId == book.BookId);

            if (existingBook != null)
            {
                _context.Entry(existingBook).CurrentValues.SetValues(book);
                existingBook.BookCategories.Clear();

                foreach (var categoryId in book.BookCategories.Select(bc => bc.CategoryId))
                {
                    var bookCategory = new BookCategory { BookId = book.BookId, CategoryId = categoryId };
                    existingBook.BookCategories.Add(bookCategory);
                }

                await _context.SaveChangesAsync();
            }

            return existingBook;
        }

        public async Task Delete(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }
    }
}
