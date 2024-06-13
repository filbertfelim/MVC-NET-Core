using Microsoft.EntityFrameworkCore;
using MvcBook.Data;
using MvcBook.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MvcBook.Repositories
{
    public class AuthorRepository : IRepository<Author>
    {
        private readonly BookContext _context;

        public AuthorRepository(BookContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Author>> GetAll()
        {
            return await _context.Authors.ToListAsync();
        }

        public async Task<Author> GetById(int id)
        {
            try
            {
                return await _context.Authors.FindAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving author with ID {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<Author> Create(Author author)
        {
            try
            {
                _context.Authors.Add(author);
                await _context.SaveChangesAsync();
                return author;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the author to the database.", ex);
            }
        }

        public async Task<Author> Update(Author author)
        {
            try
            {
                var existingAuthor = await _context.Authors.FindAsync(author.AuthorId);
                if (existingAuthor == null)
                {
                    throw new Exception("Author not found");
                }
                _context.Entry(existingAuthor).State = EntityState.Detached;

                _context.Attach(author);
                _context.Entry(author).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return author;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the author in the database.", ex);
            }
        }

        public async Task Delete(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author != null)
            {
                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();
            }
        }
    }
}
