using System.Text.Json.Serialization;

namespace MvcBook.Models
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string Name { get; set; } = string.Empty;
        [JsonIgnore]
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}