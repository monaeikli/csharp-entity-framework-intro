using exercise.webapi.Dtos;
using exercise.webapi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace exercise.webapi.Endpoints
{
    public static class PublisherAPI
    {
        public static void ConfigurePublisherApi(this WebApplication app)
        {
            app.MapGet("/publisher", GetPublishers);
            app.MapGet("/publisher/{id}", GetPublisherById);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> GetPublishers(IPublisherRepository publisherRepository)
        {
            var publishers = await publisherRepository.GetAllPublishers();
            var dto = publishers.Select(p => new PublisherDto(
                p.Id,
                p.Name,
                p.Books.Select(b => new BookInPublisherDto(
                    b.Id,
                    b.Title,
                    new AuthorInBookDto(
                        b.Author!.Id,
                        b.Author.FirstName,
                        b.Author.LastName,
                        b.Author.Email
                    )
                )).ToList()
            ));
            return TypedResults.Ok(dto);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> GetPublisherById(int id, IPublisherRepository publisherRepository)
        {
            var publisher = await publisherRepository.GetPublisherById(id);
            if (publisher == null) return TypedResults.NotFound();

            var dto = new PublisherDto(
                publisher.Id,
                publisher.Name,
                publisher.Books.Select(b => new BookInPublisherDto(
                    b.Id,
                    b.Title,
                    new AuthorInBookDto(
                        b.Author!.Id,
                        b.Author.FirstName,
                        b.Author.LastName,
                        b.Author.Email
                    )
                )).ToList()
            );
            return TypedResults.Ok(dto);
        }
    }
}
