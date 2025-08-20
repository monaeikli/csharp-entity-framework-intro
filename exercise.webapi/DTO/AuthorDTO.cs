namespace exercise.webapi.Dtos;

public record BookInAuthorDto(int Id, string Title, PublisherInBookDto Publisher);
public record AuthorDto(int Id, string FirstName, string LastName, string Email, List<BookInAuthorDto> Books);
