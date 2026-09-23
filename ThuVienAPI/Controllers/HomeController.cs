using Microsoft.AspNetCore.Mvc;

namespace ThuVienAPI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
