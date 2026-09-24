using WebAPI_simple.Models.Domain;
using WebAPI_simple.Models.DTO;

public interface IAuthorRepository
{
    List<AuthorDTO> GellAllAuthors();
    AuthorNoIdDTO GetAuthorById(int id);
    AddAuthorRequestDTO AddAuthor(AddAuthorRequestDTO addAuthorRequestDTO);
    AuthorNoIdDTO UpdateAuthorById(int id, AuthorNoIdDTO authorNoIdDTO);
    Author? DeleteAuthorById(int id);

   
    AuthorWithBooksDTO? GetBooksByAuthorId(int id);
}