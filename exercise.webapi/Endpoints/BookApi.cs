using exercise.webapi.Data;
using exercise.webapi.Dtos;
using exercise.webapi.Models;
using exercise.webapi.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace exercise.webapi.Endpoints
{
    public static class BookApi
    {
        public static void ConfigureBooksApi(this WebApplication app)
        {
            app.MapGet("/books", GetBooks);
            app.MapGet("/books/{id}", GetBookById);
            app.MapPost("/books", AddBook);
            app.MapPut("/books/{id}/author/{authorId}", UpdateBookAuthor);
            app.MapDelete("/books/{id}", DeleteBook);

        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> GetBooks(IBookRepository bookRepository)
        {
            var books = await bookRepository.GetAllBooks();
            var dto = books.Select(b => new BookDto(
                b.Id,
                b.Title,
                new AuthorInBookDto(
                    b.Author!.Id,
                    b.Author.FirstName,
                    b.Author.LastName,
                    b.Author.Email
                ),
                new PublisherInBookDto(
                    b.Publisher!.Id,
                    b.Publisher.Name
                )
            ));
            return TypedResults.Ok(dto);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> GetBookById(int id, IBookRepository bookRepository)
        {
            var book = await bookRepository.GetBookById(id); 
            if (book == null) return TypedResults.NotFound();

            var dto = new BookDto(
                book.Id,
                book.Title,
                new AuthorInBookDto(
                    book.Author!.Id,
                    book.Author.FirstName,
                    book.Author.LastName,
                    book.Author.Email
                ),
                new PublisherInBookDto(
                    book.Publisher!.Id,
                    book.Publisher.Name
                )
            );
            return TypedResults.Ok(dto);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> AddBook(Book book, IBookRepository bookRepository)
        {
            if (book == null || string.IsNullOrWhiteSpace(book.Title) || book.AuthorId <= 0)
                return TypedResults.BadRequest("Invalid book data.");

            var existingBook = await bookRepository.GetBookById(book.Id);
            if (existingBook != null)
                return TypedResults.Conflict("A book with this ID already exists.");

            await bookRepository.AddBook(book);
            return TypedResults.Created($"/books/{book.Id}", book);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        private static async Task<IResult> UpdateBookAuthor(int id, int authorId, IBookRepository bookRepository)
        {
            var updated = await bookRepository.UpdateBookAuthor(id, authorId);
            if (updated == null) return TypedResults.NotFound();

            var dto = new BookDto(
                updated.Id,
                updated.Title,
                new AuthorInBookDto(
                    updated.Author!.Id, 
                    updated.Author.FirstName, 
                    updated.Author.LastName, 
                    updated.Author.Email
                ),
                new PublisherInBookDto(
                    updated.Publisher!.Id, 
                    updated.Publisher.Name
                )
            );
            return TypedResults.Ok(dto);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> DeleteBook(int id, IBookRepository bookRepository)
        {
            var deleted = await bookRepository.DeleteBook(id);
            return deleted ? TypedResults.NoContent() : TypedResults.NotFound();
        }
    }
}
