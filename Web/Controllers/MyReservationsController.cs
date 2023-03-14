using Business.Abstract;
using Core.Constants;
using DataAccess.Context;
using DataAccess.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class MyReservationsController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IReservationService _reservationService;

        public MyReservationsController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IReservationService reservationService)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _reservationService = reservationService;
        }
        [Authorize(Policy = Constants.Policies.RequireUser)]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CancelReservation(int id)
        {
            _reservationService.SoftDelete(id);
            return View();
        }

        public IActionResult EarlyDeliver(int id)
        {
            _reservationService.EarlyDelivery(id);
            return View();
        }
    }
}
