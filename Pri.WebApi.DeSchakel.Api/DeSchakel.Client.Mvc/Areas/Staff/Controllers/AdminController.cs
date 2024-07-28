using Microsoft.AspNetCore.Mvc;

namespace DeSchakel.Client.Mvc.Areas.Staff.Controllers
{

    public class AdminController : Controller
    {
        [Area("Staff")]

        public IActionResult Index()
        {
            return View();
        }
    }
}
