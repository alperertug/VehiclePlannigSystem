using AktifVehiclePlanningSystem.Core;
using Business.Abstract;
using Core.Constants;
using DataAccess.Context;
using DataAccess.Identity.Data;
using Entities.Concrete;
using Entities.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AktifVehiclePlanningSystem.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IVehicleService _vehicleService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReservationsController(ApplicationDbContext context, IVehicleService vehicleService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _vehicleService = vehicleService;
            _userManager = userManager;
        }

        // GET: Reservations
        [Authorize(Policy = Constants.Policies.RequireManager)]
        public async Task<IActionResult> Index()
        {
            var userList = await _context.Users.ToListAsync();
            var applicationDbContext = _context.Reservations.Include(r => r.Car).Include(r => r.ReasonForReservation);
            foreach (var reservation in applicationDbContext)
            {
                var lastValue = userList.FirstOrDefault(c => c.Id == reservation.UserId);
                reservation.UserId = lastValue.UserName;
            }
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Car)
                .Include(r => r.ReasonForReservation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName").OrderBy(u => u.Text);
            ViewData["CarId"] = new SelectList(_context.Cars, "Id", "Plate");
            ViewData["ReasonForReservationId"] = new SelectList(_context.ReasonForReservations, "Id", "Reason");
            return View(new Reservation { ReservationDate = DateTime.Today, DeliveryTime = DateTime.Today.AddDays(7)});
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,CarId,ReservationDate,DeliveryTime,ReasonForReservationId,Id,CreatedBy,ModifiedBy,DeletedBy,CreatedDate,ModifiedDate,DeletedDate,IsDeleted")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarId"] = new SelectList(_context.Cars, "Id", "Plate", reservation.CarId);
            ViewData["ReasonForReservationId"] = new SelectList(_context.ReasonForReservations, "Id", "Reason", reservation.ReasonForReservationId);
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["CarId"] = new SelectList(_context.Cars, "Id", "Plate", reservation.CarId);
            ViewData["ReasonForReservationId"] = new SelectList(_context.ReasonForReservations, "Id", "Reason", reservation.ReasonForReservationId);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,CarId,ReservationDate,DeliveryTime,ReasonForReservationId,Id,CreatedBy,ModifiedBy,DeletedBy,CreatedDate,ModifiedDate,DeletedDate,IsDeleted")] Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarId"] = new SelectList(_context.Cars, "Id", "Plate", reservation.CarId);
            ViewData["ReasonForReservationId"] = new SelectList(_context.ReasonForReservations, "Id", "Reason", reservation.ReasonForReservationId);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Car)
                .Include(r => r.ReasonForReservation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Reservations == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Reservations'  is null.");
            }
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }

        [Authorize(Policy = Constants.Policies.RequireUser)]
        public IActionResult CreateReservation(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userMail = User.FindFirstValue(ClaimTypes.Email);
            var today = DateTime.Now.Date;
            var nextWeek = today.AddDays(7);

            var car = _context.Cars.FirstOrDefault(e => e.Id == id);
            var reason = _context.ReasonForReservations.FirstOrDefault(e => e.Id == 1);
            ViewData["ReasonForReservationId"] = new SelectList(_context.ReasonForReservations, "Id", "Reason");
            var reservation = new Reservation()
            {
                CarId = car.Id,
                Car = car,
                UserId = userId,
                ReservationDate = today,
                DeliveryTime = nextWeek,
                ReasonForReservationId = 1,
                ReasonForReservation = reason
            };
            return View(reservation);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReservation([Bind(include: "UserId, CarId, ReservationDate, DeliveryTime, ReasonForReservationId, CreatedBy, ModifiedBy, DeletedBy, CreatedDate, ModifiedDate, DeletedDate, IsDeleted")] Reservation reservation)
        {
            var reservationList = _context.Reservations.ToList();
            foreach (var reserv in reservationList)
            {
                //if (reservation.CarId == reserv.CarId)
                //{
                    //if (reservation.ReservationDate <= reserv.DeliveryTime && reserv.ReservationDate < reservation.DeliveryTime)
                    //{
                    //    return RedirectToAction("CreateReservation", "Reservations");
                    //    //"You cannot reserve this car with selected dates";
                    //}
                    
                        if (ModelState.IsValid)
                        {
                            _context.Add(reservation);
                            await _context.SaveChangesAsync();
                            return RedirectToAction("Index", "MyReservations");
                        }

                        ViewData["ReasonForReservationId"] = new SelectList(_context.ReasonForReservations, "Id", "Reason");
                    
                
            }            

            return View(reservation);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> CreateReservation(int id, [Bind("UserId,CarId,ReservationDate,DeliveryTime,ReasonForReservationId,Id,CreatedBy,ModifiedBy,DeletedBy,CreatedDate,ModifiedDate,DeletedDate,IsDeleted")] Reservation reservation)
        //{
        //    if (id != reservation.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(reservation);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ReservationExists(reservation.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["CarId"] = new SelectList(_context.Cars, "Id", "Plate", reservation.CarId);
        //    ViewData["ReasonForReservationId"] = new SelectList(_context.ReasonForReservations, "Id", "Reason", reservation.ReasonForReservationId);
        //    return View(reservation);
        //}
    }
}


//if (ModelState.IsValid)
//{
//    _context.Add(reservation);
//    await _context.SaveChangesAsync();
//    return RedirectToAction(nameof(Index));
//}
//ViewData["CarId"] = new SelectList(_context.Cars, "Id", "Plate", reservation.CarId);
//ViewData["ReasonForReservationId"] = new SelectList(_context.ReasonForReservations, "Id", "Reason", reservation.ReasonForReservationId);
//return View(reservation);