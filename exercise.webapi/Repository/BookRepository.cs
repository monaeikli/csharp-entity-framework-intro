using exercise.webapi.Data;
using exercise.webapi.Models;
using Microsoft.EntityFrameworkCore;

namespace exercise.webapi.Repository
{
    public class BookRepository : IBookRepository
    {
        DataContext _db;

        public BookRepository(DataContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            return await _db.Books.Include(b => b.Author).Include(b => b.Publisher).ToListAsync();

        }
        public async Task<Book?> GetBookById(int id)
        {
            return await _db.Books.Include(b => b.Author).Include(b => b.Publisher).FirstOrDefaultAsync(b => b.Id == id);


        }
        public async Task AddBook(Book book)
        {
            if (book == null)
            {
                throw new ArgumentNullException(nameof(book));
            }
            _db.Books.Add(book);
            await _db.SaveChangesAsync();
        }
        public async Task<Book?> UpdateBookAuthor(int bookId, int newAuthorId)
        {
            var book = await _db.Books.FirstOrDefaultAsync(b => b.Id == bookId);
            if (book == null) return null;

            book.AuthorId = newAuthorId;
            await _db.SaveChangesAsync();

            await _db.Entry(book).Reference(b => b.Author).LoadAsync();
            await _db.Entry(book).Reference(b => b.Publisher).LoadAsync();
            return book;
        }
        public async Task<bool> DeleteBook(int id)
        {
            var book = await _db.Books.FindAsync(id);
            if (book == null) return false;

            _db.Books.Remove(book);
            await _db.SaveChangesAsync();
            return true;
        }

    }
}
