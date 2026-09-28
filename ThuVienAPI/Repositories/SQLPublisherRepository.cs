using Microsoft.EntityFrameworkCore;
using WebAPI_simple.Data;
using WebAPI_simple.Models.Domain;
using WebAPI_simple.Models.DTO;

namespace WebAPI_simple.Repositories
{
    public class SQLPublisherRepository : IPublisherRepository
    {
        private readonly AppDbContext _dbContext;
        public SQLPublisherRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<PublisherDTO> GetAllPublishers(string? filterOn = null, string? filterQuery = null,
      string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            var allPublishers = _dbContext.Publishers.Select(p => new PublisherDTO()
            {
                Id = p.Id,
                Name = p.Name
            }).AsQueryable();

            
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("name", StringComparison.OrdinalIgnoreCase))
                {
                    allPublishers = allPublishers.Where(x => x.Name.Contains(filterQuery));
                }
            }

            
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("name", StringComparison.OrdinalIgnoreCase))
                {
                    allPublishers = isAscending ? allPublishers.OrderBy(x => x.Name) : allPublishers.OrderByDescending(x => x.Name);
                }
                else if (sortBy.Equals("id", StringComparison.OrdinalIgnoreCase))
                {
                    allPublishers = isAscending ? allPublishers.OrderBy(x => x.Id) : allPublishers.OrderByDescending(x => x.Id);
                }
            }

            
            var skipResults = (pageNumber - 1) * pageSize;
            return allPublishers.Skip(skipResults).Take(pageSize).ToList();
        }

        public PublisherNoIdDTO GetPublisherById(int id)
        {
            var publisherWithIdDomain = _dbContext.Publishers.FirstOrDefault(x => x.Id == id);
            if (publisherWithIdDomain != null)
            {
                var publisherNoIdDTO = new PublisherNoIdDTO
                {
                    Name = publisherWithIdDomain.Name,
                };
                return publisherNoIdDTO;
            }
            return null;
        }

        public AddPublisherRequestDTO AddPublisher(AddPublisherRequestDTO addPublisherRequestDTO)
        {
            var publisherDomainModel = new Publisher
            {
                Name = addPublisherRequestDTO.Name,
            };
            _dbContext.Publishers.Add(publisherDomainModel);
            _dbContext.SaveChanges();
            return addPublisherRequestDTO;
        }

        public PublisherNoIdDTO UpdatePublisherById(int id, PublisherNoIdDTO publisherNoIdDTO)
        {
            var publisherDomain = _dbContext.Publishers.FirstOrDefault(n => n.Id == id);
            if (publisherDomain != null)
            {
                publisherDomain.Name = publisherNoIdDTO.Name;
                _dbContext.SaveChanges();
            }
            return null;
        }

        public Publisher? DeletePublisherById(int id)
        {
            var publisherDomain = _dbContext.Publishers.FirstOrDefault(n => n.Id == id);
            if (publisherDomain != null)
            {
                _dbContext.Publishers.Remove(publisherDomain);
                _dbContext.SaveChanges();
            }
            return null;
        }

        public PublisherWithBooksDTO? GetBooksByPublisherId(int id)
        {
            var publisherDomain = _dbContext.Publishers
                .Include(p => p.Books)
                .FirstOrDefault(p => p.Id == id);

            if (publisherDomain == null)
            {
                return null;
            }

            var result = new PublisherWithBooksDTO
            {
                Id = publisherDomain.Id,
                Name = publisherDomain.Name,
                BookTitles = publisherDomain.Books.Select(b => b.Title).ToList()
            };

            return result;
        }
    }
}