using Microsoft.AspNetCore.Mvc;
using E_Tickets.Models;


namespace New_Eticket.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NotFoundPage()
        {
            return View();
        }
    }
}
