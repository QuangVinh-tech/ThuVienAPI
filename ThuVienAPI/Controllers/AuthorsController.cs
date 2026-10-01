using Microsoft.AspNetCore.Mvc;
using WebAPI_simple.Data;
using WebAPI_simple.Models.DTO;
using WebAPI_simple.Repositories;

namespace WebAPI_simple.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IAuthorRepository _authorRepository;
        public AuthorsController(AppDbContext dbContext, IAuthorRepository authorRepository)
        {
            _dbContext = dbContext;
            _authorRepository = authorRepository;
        }

            

        [HttpGet("get-all-author")]
        public IActionResult GetAllAuthor([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
      [FromQuery] string? sortBy, [FromQuery] bool isAscending = true,
      [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 100)
        {
            var allAuthors = _authorRepository.GellAllAuthors(filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize);
            return Ok(allAuthors);
        }

        [HttpPost("add-author")]
        public IActionResult AddAuthors([FromBody] AddAuthorRequestDTO addAuthorRequestDTO)
        {
            var authorAdd = _authorRepository.AddAuthor(addAuthorRequestDTO);
            return Ok(authorAdd);
        }

        [HttpPut("update-author-by-id/{id}")]
        public IActionResult UpdateBookById(int id, [FromBody] AuthorNoIdDTO authorDTO)
        {
            var authorUpdate = _authorRepository.UpdateAuthorById(id, authorDTO);
            return Ok(authorUpdate);
        }

        [HttpDelete("delete-author-by-id/{id}")]
        public IActionResult DeleteBookById(int id)
        {
            var authorDelete = _authorRepository.DeleteAuthorById(id);
            if (authorDelete == null)
            {
                return NotFound(new { message = "Không tìm thấy tác giả" });
            }
            return Ok(authorDelete);
        }

        [HttpGet("{id}/books")]
        public IActionResult GetBooksByAuthorId(int id)
        {
            var result = _authorRepository.GetBooksByAuthorId(id);
            if (result == null)
            {
                return NotFound(new { message = "Không tìm thấy tác giả" });
            }
            return Ok(result);
        }
    }
}