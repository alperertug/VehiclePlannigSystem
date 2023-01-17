using Core.Constants;
using DataAccess.Context;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AktifVehiclePlanningSystem.Controllers
{
    public class ReasonForReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReasonForReservationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ReasonForReservations
        [Authorize(Policy = Constants.Policies.RequireManager)]
        public async Task<IActionResult> Index()
        {
            return View(await _context.ReasonForReservations.ToListAsync());
        }

        // GET: ReasonForReservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ReasonForReservations == null)
            {
                return NotFound();
            }

            var reasonForReservation = await _context.ReasonForReservations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reasonForReservation == null)
            {
                return NotFound();
            }

            return View(reasonForReservation);
        }

        // GET: ReasonForReservations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ReasonForReservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Reason,Id,CreatedBy,ModifiedBy,DeletedBy,CreatedDate,ModifiedDate,DeletedDate,IsDeleted")] ReasonForReservation reasonForReservation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reasonForReservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reasonForReservation);
        }

        // GET: ReasonForReservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ReasonForReservations == null)
            {
                return NotFound();
            }

            var reasonForReservation = await _context.ReasonForReservations.FindAsync(id);
            if (reasonForReservation == null)
            {
                return NotFound();
            }
            return View(reasonForReservation);
        }

        // POST: ReasonForReservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Reason,Id,CreatedBy,ModifiedBy,DeletedBy,CreatedDate,ModifiedDate,DeletedDate,IsDeleted")] ReasonForReservation reasonForReservation)
        {
            if (id != reasonForReservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reasonForReservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReasonForReservationExists(reasonForReservation.Id))
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
            return View(reasonForReservation);
        }

        // GET: ReasonForReservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ReasonForReservations == null)
            {
                return NotFound();
            }

            var reasonForReservation = await _context.ReasonForReservations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reasonForReservation == null)
            {
                return NotFound();
            }

            return View(reasonForReservation);
        }

        // POST: ReasonForReservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ReasonForReservations == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ReasonForReservations'  is null.");
            }
            var reasonForReservation = await _context.ReasonForReservations.FindAsync(id);
            if (reasonForReservation != null)
            {
                _context.ReasonForReservations.Remove(reasonForReservation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReasonForReservationExists(int id)
        {
            return _context.ReasonForReservations.Any(e => e.Id == id);
        }
    }
}
