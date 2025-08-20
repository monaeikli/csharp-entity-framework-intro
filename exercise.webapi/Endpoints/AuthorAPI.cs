using exercise.webapi.Dtos;
using exercise.webapi.Repository;

namespace exercise.webapi.Endpoints
{
    public static class AuthorAPI
    {
        public static void ConfigureAuthorApi(this WebApplication app)
        {
            app.MapGet("/author", GetAuthors);
            app.MapGet("/author/{id}", GetAuthorById);
        }

        private static async Task<IResult> GetAuthors(IAuthorRepository authorRepository)
        {
            var authors = await authorRepository.GetAllAuthors();
            var dto = authors.Select(a => new AuthorDto(
                a.Id, a.FirstName, a.LastName, a.Email,
                a.Books.Select(b => new BookInAuthorDto(
                    b.Id, b.Title, new PublisherInBookDto(b.Publisher!.Id, b.Publisher.Name)
                )).ToList()
            ));
            return TypedResults.Ok(dto);
        }

        private static async Task<IResult> GetAuthorById(int id, IAuthorRepository authorRepository)
        {
            var a = await authorRepository.GetAuthorById(id);
            if (a == null) return TypedResults.NotFound();

            var dto = new AuthorDto(
                a.Id, a.FirstName, a.LastName, a.Email,
                a.Books.Select(b => new BookInAuthorDto(
                    b.Id, b.Title, new PublisherInBookDto(b.Publisher!.Id, b.Publisher.Name)
                )).ToList()
            );
            return TypedResults.Ok(dto);
        }
    }
}
