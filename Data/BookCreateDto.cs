using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcBook.Data
{
    public class BookCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public int AuthorId { get; set; }
        public List<int> CategoryIds { get; set; } = new List<int>();
    }
}