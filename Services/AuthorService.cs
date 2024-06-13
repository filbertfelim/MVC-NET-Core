using MvcBook.Models;
using MvcBook.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MvcBook.Services
{
    public class AuthorService
    {
        private readonly IRepository<Author> _authorRepository;

        public AuthorService(IRepository<Author> authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<IEnumerable<Author>> GetAllAuthors()
        {
            return await _authorRepository.GetAll();
        }

        public async Task<Author> GetAuthorById(int id)
        {
            return await _authorRepository.GetById(id);
        }

        public async Task<Author> CreateAuthor(Author author)
        {
            return await _authorRepository.Create(author);
        }

        public async Task UpdateAuthor(Author author)
        {
            await _authorRepository.Update(author);
        }

        public async Task DeleteAuthor(int id)
        {
            await _authorRepository.Delete(id);
        }
    }
}
