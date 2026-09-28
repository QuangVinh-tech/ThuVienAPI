using Microsoft.EntityFrameworkCore;
using WebAPI_simple.Data;
using WebAPI_simple.Models.Domain;
using WebAPI_simple.Models.DTO;

namespace WebAPI_simple.Repositories
{
    public class SQLAuthorRepository : IAuthorRepository
    {
        private readonly AppDbContext _dbContext;
        public SQLAuthorRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<AuthorDTO> GellAllAuthors(string? filterOn = null, string? filterQuery = null,
      string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            var allAuthors = _dbContext.Authors.Select(a => new AuthorDTO()
            {
                Id = a.Id,
                FullName = a.FullName
            }).AsQueryable();

            
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("fullName", StringComparison.OrdinalIgnoreCase))
                {
                    allAuthors = allAuthors.Where(x => x.FullName.Contains(filterQuery));
                }
            }

            
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("fullName", StringComparison.OrdinalIgnoreCase))
                {
                    allAuthors = isAscending ? allAuthors.OrderBy(x => x.FullName) : allAuthors.OrderByDescending(x => x.FullName);
                }
                else if (sortBy.Equals("id", StringComparison.OrdinalIgnoreCase))
                {
                    allAuthors = isAscending ? allAuthors.OrderBy(x => x.Id) : allAuthors.OrderByDescending(x => x.Id);
                }
            }

            
            var skipResults = (pageNumber - 1) * pageSize;
            return allAuthors.Skip(skipResults).Take(pageSize).ToList();
        }

        public AuthorNoIdDTO GetAuthorById(int id)
        {
            var authorWithIdDomain = _dbContext.Authors.FirstOrDefault(x => x.Id == id);
            if (authorWithIdDomain == null)
            {
                return null;
            }
            var authorNoIdDTO = new AuthorNoIdDTO
            {
                FullName = authorWithIdDomain.FullName,
            };
            return authorNoIdDTO;
        }

        public AddAuthorRequestDTO AddAuthor(AddAuthorRequestDTO addAuthorRequestDTO)
        {
            var authorDomainModel = new Author
            {
                FullName = addAuthorRequestDTO.FullName,
            };
            _dbContext.Authors.Add(authorDomainModel);
            _dbContext.SaveChanges();
            return addAuthorRequestDTO;
        }

        public AuthorNoIdDTO UpdateAuthorById(int id, AuthorNoIdDTO authorNoIdDTO)
        {
            var authorDomain = _dbContext.Authors.FirstOrDefault(n => n.Id == id);
            if (authorDomain != null)
            {
                authorDomain.FullName = authorNoIdDTO.FullName;
                _dbContext.SaveChanges();
            }
            return authorNoIdDTO;
        }

        public Author? DeleteAuthorById(int id)
        {
            var authorDomain = _dbContext.Authors.FirstOrDefault(n => n.Id == id);
            if (authorDomain != null)
            {
                _dbContext.Authors.Remove(authorDomain);
                _dbContext.SaveChanges();
            }
            return null;
        }
        public AuthorWithBooksDTO? GetBooksByAuthorId(int id)
        {
            // Lấy Author kèm navigation Book_Authors -> Book
            var authorDomain = _dbContext.Authors
                .Include(a => a.Book_Authors)
                    .ThenInclude(ba => ba.Book)
                .FirstOrDefault(a => a.Id == id);

            if (authorDomain == null)
            {
                return null;
            }

            var result = new AuthorWithBooksDTO
            {
                Id = authorDomain.Id,
                FullName = authorDomain.FullName,
                BookTitles = authorDomain.Book_Authors
                    .Select(ba => ba.Book.Title)
                    .ToList()
            };

            return result;
        }
    }
}