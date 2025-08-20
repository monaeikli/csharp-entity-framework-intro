using exercise.webapi.Data;
using exercise.webapi.Models;
using Microsoft.EntityFrameworkCore;

namespace exercise.webapi.Repository
{
    public class AuthorRepository : IAuthorRepository
    {
        DataContext _db;
        public AuthorRepository(DataContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Author>> GetAllAuthors()
        {
            return await _db.Authors
                .Include(a => a.Books).ThenInclude(b => b.Publisher)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Author?> GetAuthorById(int id)
        {
            return await _db.Authors
                .Include(a => a.Books).ThenInclude(b => b.Publisher)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
