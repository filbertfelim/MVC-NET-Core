using Microsoft.EntityFrameworkCore;
using MvcBook.Models;
using System.Linq;

namespace MvcBook.Data
{
    public static class DbInitializer
    {
        public static void Initialize(BookContext context)
        {
            context.Database.Migrate();

            // Look for any authors.
            if (context.Authors.Any())
            {
                return;   // DB has been seeded
            }

            var authors = new Author[]
            {
                new Author{Name="J.K. Rowling"},
                new Author{Name="George R.R. Martin"},
                new Author{Name="J.R.R. Tolkien"}
            };

            foreach (Author a in authors)
            {
                context.Authors.Add(a);
            }
            context.SaveChanges();

            var categories = new Category[]
            {
                new Category{Name="Fantasy"},
                new Category{Name="Science Fiction"},
                new Category{Name="Non-Fiction"}
            };

            foreach (Category c in categories)
            {
                context.Categories.Add(c);
            }
            context.SaveChanges();

            var books = new Book[]
            {
                new Book{Title="Harry Potter and the Philosopher's Stone", AuthorId=1},
                new Book{Title="A Game of Thrones", AuthorId=2},
                new Book{Title="The Hobbit", AuthorId=3}
            };

            foreach (Book b in books)
            {
                context.Books.Add(b);
            }
            context.SaveChanges();

            var bookCategories = new BookCategory[]
            {
                new BookCategory{BookId=1, CategoryId=1},
                new BookCategory{BookId=2, CategoryId=1},
                new BookCategory{BookId=3, CategoryId=1},
                new BookCategory{BookId=1, CategoryId=2},
                new BookCategory{BookId=1, CategoryId=3}
            };

            foreach (BookCategory bc in bookCategories)
            {
                context.BookCategories.Add(bc);
            }
            context.SaveChanges();
        }
    }
}
