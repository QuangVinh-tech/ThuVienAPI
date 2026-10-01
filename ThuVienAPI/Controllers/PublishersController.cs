using Microsoft.AspNetCore.Mvc;
using WebAPI_simple.Data;
using WebAPI_simple.Models.DTO;
using WebAPI_simple.Repositories;

namespace WebAPI_simple.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IPublisherRepository _publisherRepository;
        public PublishersController(AppDbContext dbContext, IPublisherRepository publisherRepository)
        {
            _dbContext = dbContext;
            _publisherRepository = publisherRepository;
        }

        [HttpGet("get-all-publisher")]
        public IActionResult GetAllPublisher([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
       [FromQuery] string? sortBy, [FromQuery] bool isAscending = true,
       [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 100)
        {
            var allPublishers = _publisherRepository.GetAllPublishers(filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize);
            return Ok(allPublishers);
        }

        [HttpGet("get-publisher-by-id/{id}")]
        public IActionResult GetPublisherById(int id)
        {
            var publisherWithId = _publisherRepository.GetPublisherById(id);
            return Ok(publisherWithId);
        }

        [HttpPost("add-publisher")]
        public IActionResult AddPublisher([FromBody] AddPublisherRequestDTO addPublisherRequestDTO)
        {
            var publisherAdd = _publisherRepository.AddPublisher(addPublisherRequestDTO);
            return Ok(publisherAdd);
        }

        [HttpPut("update-publisher-by-id/{id}")]
        public IActionResult UpdatePublisherById(int id, [FromBody] PublisherNoIdDTO publisherDTO)
        {
            var publisherUpdate = _publisherRepository.UpdatePublisherById(id, publisherDTO);
            return Ok(publisherUpdate);
        }

        [HttpDelete("delete-publisher-by-id/{id}")]
        public IActionResult DeletePublisherById(int id)
        {
            var publisherDelete = _publisherRepository.DeletePublisherById(id);
            if (publisherDelete == null)
            {
                return NotFound(new { message = "Không tìm thấy NXB" });
            }
            return Ok(publisherDelete);
        }

        [HttpGet("{id}/books")]
        public IActionResult GetBooksByPublisherId(int id)
        {
            var result = _publisherRepository.GetBooksByPublisherId(id);
            if (result == null)
            {
                return NotFound(new { message = "Không tìm thấy NXB" });
            }
            return Ok(result);
        }
    }
}