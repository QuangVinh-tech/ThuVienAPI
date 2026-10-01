using Microsoft.AspNetCore.Mvc;
using Library_web.Models.DTO;
using System.Text;
using System.Text.Json;
using System.Net.Mime;

namespace Library_web.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public AuthorsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

       
        public async Task<IActionResult> Index()
        {
            List<authorDTO> response = new List<authorDTO>();
            try
            {
                var client = httpClientFactory.CreateClient();
                var httpResponseMess = await client.GetAsync("https://localhost:7177/api/Authors/get-all-author");
                httpResponseMess.EnsureSuccessStatusCode();
                response.AddRange(await httpResponseMess.Content.ReadFromJsonAsync<IEnumerable<authorDTO>>());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(response);
        }

        
        public async Task<IActionResult> listAuthor(int id)
        {
            authorDTO response = new authorDTO();
            try
            {
                var client = httpClientFactory.CreateClient();
                var httpResponseMess = await client.GetAsync("https://localhost:7177/api/Authors/get-author-by-id/" + id);
                httpResponseMess.EnsureSuccessStatusCode();
                response = await httpResponseMess.Content.ReadFromJsonAsync<authorDTO>();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(response);
        }

       
        [HttpGet]
        public IActionResult addAuthor()
        {
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> addAuthor(addAuthorDTO addAuthorDTO)
        {
            try
            {
                var client = httpClientFactory.CreateClient();
                var httpRequestMess = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://localhost:7177/api/Authors/add-author"),
                    Content = new StringContent(JsonSerializer.Serialize(addAuthorDTO), Encoding.UTF8,
                        MediaTypeNames.Application.Json)
                };
                var httpResponseMess = await client.SendAsync(httpRequestMess);
                httpResponseMess.EnsureSuccessStatusCode();
                return RedirectToAction("Index", "Authors");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View();
        }

       
        [HttpGet]
        public async Task<IActionResult> editAuthor(int id)
        {
            authorDTO responseAuthor = new authorDTO();
            var client = httpClientFactory.CreateClient();
            var httpResponseMess = await client.GetAsync("https://localhost:7177/api/Authors/get-author-by-id/" + id);
            httpResponseMess.EnsureSuccessStatusCode();
            responseAuthor = await httpResponseMess.Content.ReadFromJsonAsync<authorDTO>();
            ViewBag.Author = responseAuthor;
            ViewBag.Id = id;
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> editAuthor([FromRoute] int id, authorNoIdDTO authorDTO)
        {
            try
            {
                var client = httpClientFactory.CreateClient();
                var httpRequestMess = new HttpRequestMessage()
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri("https://localhost:7177/api/Authors/update-author-by-id/" + id),
                    Content = new StringContent(JsonSerializer.Serialize(authorDTO), Encoding.UTF8,
                        MediaTypeNames.Application.Json)
                };
                var httpResponseMess = await client.SendAsync(httpRequestMess);
                httpResponseMess.EnsureSuccessStatusCode();
                return RedirectToAction("Index", "Authors");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View();
        }

        
        public async Task<IActionResult> delAuthor([FromRoute] int id)
        {
            try
            {
                var client = httpClientFactory.CreateClient();
                var httpResponseMess = await client.DeleteAsync("https://localhost:7177/api/Authors/delete-author-by-id/" + id);
                httpResponseMess.EnsureSuccessStatusCode();
                return RedirectToAction("Index", "Authors");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View("Index");
        }
    }
}