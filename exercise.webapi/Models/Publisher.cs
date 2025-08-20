namespace exercise.webapi.Models
{
    public class Publisher
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
