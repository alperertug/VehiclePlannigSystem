using AktifVehiclePlanningSystem.Core;
using Business.Abstract;
using Core.Constants;
using DataAccess.Context;
using DataAccess.Identity.Data;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            var applicationDbContext = _context.Reservations.Include(r => r.Car).Include(r => r.ReasonForReservation);
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
            ViewData["CarId"] = new SelectList(_context.Cars, "Id", "Plate");
            ViewData["ReasonForReservationId"] = new SelectList(_context.ReasonForReservations, "Id", "Reason");
            return View();
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
;
            var userMail = User.FindFirstValue(ClaimTypes.Email);

            var car = _context.Cars.FirstOrDefault(e => e.Id == id);
            ViewData["ReasonForReservationId"] = new SelectList(_context.ReasonForReservations, "Id", "Reason");

            return View(new Reservation() { Car = car, UserId = userMail });
        }
    }
}
