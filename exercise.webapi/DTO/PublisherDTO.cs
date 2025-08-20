namespace exercise.webapi.Dtos;

public record BookInPublisherDto(int Id, string Title, AuthorInBookDto Author);

public record PublisherDto(int Id, string Name, List<BookInPublisherDto> Books);
