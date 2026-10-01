using Microsoft.AspNetCore.Mvc;
using Library_web.Models.DTO;
using System.Text;
using System.Text.Json;
using System.Net.Mime;

namespace Library_web.Controllers
{
    public class PublishersController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public PublishersController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            List<publisherDTO> response = new List<publisherDTO>();
            try
            {
                var client = httpClientFactory.CreateClient();
                var httpResponseMess = await client.GetAsync("https://localhost:7177/api/Publishers/get-all-publisher");
                httpResponseMess.EnsureSuccessStatusCode();
                response.AddRange(await httpResponseMess.Content.ReadFromJsonAsync<IEnumerable<publisherDTO>>());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(response);
        }

        public async Task<IActionResult> listPublisher(int id)
        {
            publisherDTO response = new publisherDTO();
            try
            {
                var client = httpClientFactory.CreateClient();
                var httpResponseMess = await client.GetAsync("https://localhost:7177/api/Publishers/get-publisher-by-id/" + id);
                httpResponseMess.EnsureSuccessStatusCode();
                response = await httpResponseMess.Content.ReadFromJsonAsync<publisherDTO>();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(response);
        }

        [HttpGet]
        public IActionResult addPublisher()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> addPublisher(addPublisherDTO addPublisherDTO)
        {
            try
            {
                var client = httpClientFactory.CreateClient();
                var httpRequestMess = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://localhost:7177/api/Publishers/add-publisher"),
                    Content = new StringContent(JsonSerializer.Serialize(addPublisherDTO), Encoding.UTF8,
                        MediaTypeNames.Application.Json)
                };
                var httpResponseMess = await client.SendAsync(httpRequestMess);
                httpResponseMess.EnsureSuccessStatusCode();
                return RedirectToAction("Index", "Publishers");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> editPublisher(int id)
        {
            publisherDTO responsePublisher = new publisherDTO();
            var client = httpClientFactory.CreateClient();
            var httpResponseMess = await client.GetAsync("https://localhost:7177/api/Publishers/get-publisher-by-id/" + id);
            httpResponseMess.EnsureSuccessStatusCode();
            responsePublisher = await httpResponseMess.Content.ReadFromJsonAsync<publisherDTO>();
            ViewBag.Publisher = responsePublisher;
            ViewBag.Id = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> editPublisher([FromRoute] int id, publisherNoIdDTO publisherDTO)
        {
            try
            {
                var client = httpClientFactory.CreateClient();
                var httpRequestMess = new HttpRequestMessage()
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri("https://localhost:7177/api/Publishers/update-publisher-by-id/" + id),
                    Content = new StringContent(JsonSerializer.Serialize(publisherDTO), Encoding.UTF8,
                        MediaTypeNames.Application.Json)
                };
                var httpResponseMess = await client.SendAsync(httpRequestMess);
                httpResponseMess.EnsureSuccessStatusCode();
                return RedirectToAction("Index", "Publishers");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View();
        }

        public async Task<IActionResult> delPublisher([FromRoute] int id)
        {
            try
            {
                var client = httpClientFactory.CreateClient();
                var httpResponseMess = await client.DeleteAsync("https://localhost:7177/api/Publishers/delete-publisher-by-id/" + id);
                httpResponseMess.EnsureSuccessStatusCode();
                return RedirectToAction("Index", "Publishers");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View("Index");
        }
    }
}