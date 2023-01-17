using Microsoft.AspNetCore.Mvc;

namespace AktifVehiclePlanningSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
