using exercise.webapi.Models;

namespace exercise.webapi.Repository
{
    public interface IBookRepository
    {
        public Task<IEnumerable<Book>> GetAllBooks();
        public Task<Book?> GetBookById(int id);
        public Task AddBook(Book book);
        public Task<Book?> UpdateBookAuthor(int bookId, int newAuthorId);
        public Task<bool> DeleteBook(int id);

    }
}
