using MvcBook.Models;
using MvcBook.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MvcBook.Services
{
    public class BookService
    {
        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<Author> _authorRepository;
        private readonly IRepository<Category> _categoryRepository;

        public BookService(IRepository<Book> bookRepository, IRepository<Category> categoryRepository, IRepository<Author> authorRepository)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
            _authorRepository = authorRepository;
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            return await _bookRepository.GetAll();
        }

        public async Task<Book> GetBookById(int id)
        {
            return await _bookRepository.GetById(id);
        }

        public async Task<Book> CreateBook(Book book, List<int> categoryIds)
        {
            
            if (!await AreValidCategoryIds(categoryIds) && !await IsValidAuthorId(book.AuthorId))
            {
                throw new Exception("One or more category IDs and provided author ID are invalid.");
            }
            if (!await AreValidCategoryIds(categoryIds))
            {
                throw new Exception("One or more category IDs are invalid.");
            }
            if (!await IsValidAuthorId(book.AuthorId))
            {
                throw new Exception("The provided author ID is invalid.");
            }
            book.BookCategories = new List<BookCategory>();
            foreach (var categoryId in categoryIds)
            {
                book.BookCategories.Add(new BookCategory { BookId = book.BookId, CategoryId = categoryId });
            }

            return await _bookRepository.Create(book);
        }

        public async Task<Book> UpdateBook(Book book, List<int> categoryIds)
        {
            if (!await AreValidCategoryIds(categoryIds) && !await IsValidAuthorId(book.AuthorId))
            {
                throw new Exception("One or more category IDs and provided author ID are invalid.");
            }
            if (!await AreValidCategoryIds(categoryIds))
            {
                throw new Exception("One or more category IDs are invalid.");
            }
            if (!await IsValidAuthorId(book.AuthorId))
            {
                throw new Exception("The provided author ID is invalid.");
            }
            book.BookCategories = new List<BookCategory>();
            foreach (var categoryId in categoryIds)
            {
                book.BookCategories.Add(new BookCategory { BookId = book.BookId, CategoryId = categoryId });
            }
            return await _bookRepository.Update(book);            
        }

        public async Task DeleteBook(int id)
        {
            await _bookRepository.Delete(id);
        }

        private async Task<bool> AreValidCategoryIds(List<int> categoryIds)
        {
            var validCategoryIds = await _categoryRepository.GetAll();
            return categoryIds.All(id => validCategoryIds.Any(c => c.CategoryId == id));
        }

        private async Task<bool> IsValidAuthorId(int authorId)
        {
            var author = await _authorRepository.GetById(authorId);
            return author != null;
        }
    }
}
