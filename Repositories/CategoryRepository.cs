using Microsoft.EntityFrameworkCore;
using MvcBook.Data;
using MvcBook.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MvcBook.Repositories
{
    public class CategoryRepository : IRepository<Category>
    {
        private readonly BookContext _context;

        public CategoryRepository(BookContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetById(int id)
        {
            try
            {
                return await _context.Categories.FindAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving category with ID {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<Category> Create(Category category)
        {
            try
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                return category;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the category to the database.", ex);
            }
        }

        public async Task<Category> Update(Category category)
        {
            try
            {
                var existingCategory = await _context.Categories.FindAsync(category.CategoryId);
                if (existingCategory == null)
                {
                    throw new Exception("Category not found");
                }
                _context.Entry(existingCategory).State = EntityState.Detached;

                _context.Attach(category);
                _context.Entry(category).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return category;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the category in the database.", ex);
            }
        }

        public async Task Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}
