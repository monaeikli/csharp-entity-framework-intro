namespace exercise.webapi.Dtos;

public record AuthorInBookDto(int Id, string FirstName, string LastName, string Email);
public record PublisherInBookDto(int Id, string Name);
public record BookDto(int Id, string Title, AuthorInBookDto Author, PublisherInBookDto Publisher);
