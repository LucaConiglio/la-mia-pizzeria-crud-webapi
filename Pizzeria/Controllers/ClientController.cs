using Microsoft.AspNetCore.Mvc;

namespace Pizzeria.Controllers
{
    public class ClientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
