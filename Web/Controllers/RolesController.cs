using Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AktifVehiclePlanningSystem.Controllers
{
    public class RolesController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }

        //[Authorize(Policy = "RequireManager")]
        [Authorize(Policy = Constants.Policies.RequireAdmin)]
        public IActionResult Manager() 
        { 
            return View(); 
        }

        //[Authorize(Policy = "RequireAdmin")]
        [Authorize(Roles = $"{Constants.Roles.Administrator},{Constants.Roles.Manager}")]
        public IActionResult Admin() 
        { 
            return View(); 
        } 
    }
}
