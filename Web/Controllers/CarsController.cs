using Business.Abstract;
using Core.Constants;
using DataAccess.Context;
using DataAccess.Repositories.Abstract;
using DataAccess.UnitOfWorks;
using Entities.Concrete;
using Entities.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AktifVehiclePlanningSystem.Controllers
{
    public class CarsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public CarsController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Cars
        [Authorize(Policy = Constants.Policies.RequireManager)]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Cars.Include(c => c.Color).Include(c => c.Model).OrderBy(b => b.Model.BrandName);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Cars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cars == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .Include(c => c.Color)
                .Include(c => c.Model)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // GET: Cars/Create
        public async Task<IActionResult> Create()
        {            
            var selectModelBrand = _context.Models;

            ViewData["ColorId"] = new SelectList(_context.Colors, "Id", "ColorName").OrderBy(x => x.Text);
            //ViewData["ModelId"] = new SelectList(_context.Models, "Id", "ModelName").OrderBy(x => x.Value);
            ViewData["ModelId"] = selectModelBrand.Select(i => new SelectListItem
            {
                Text = i.BrandName + " --> " + i.ModelName,
                Value = i.Id.ToString()
            }).Distinct().ToList<SelectListItem>().OrderBy(x => x.Text);

            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Plate,ModelId,ColorId,ProductionYear,PersonCapacity,LoadLimit,Title,ImageFile,Id,CreatedBy,ModifiedBy,DeletedBy,CreatedDate,ModifiedDate,DeletedDate,IsDeleted")] Car car)
        {
            
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(car.ImageFile.FileName);
                string extension = Path.GetExtension(car.ImageFile.FileName);
                car.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await car.ImageFile.CopyToAsync(fileStream);
                }


                _context.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ColorId"] = new SelectList(_context.Colors, "Id", "ColorName", car.ColorId);
            ViewData["ModelId"] = new SelectList(_context.Models, "Id", "ModelName", car.ModelId);

            return View(car);
        }

        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cars == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            //Models call
            var selectModelBrand = _context.Models;
            ViewData["ColorId"] = new SelectList(_context.Colors, "Id", "ColorName", car.ColorId).OrderBy(x => x.Value);
            //ViewData["ModelId"] = new SelectList(_context.Models, "Id", "ModelName", car.ModelId);
            //TestViewData
            ViewData["ModelId"] = selectModelBrand.Select(i => new SelectListItem
            {
                Text = i.BrandName + " --> " + i.ModelName,
                Value = i.Id.ToString()
            }).Distinct().ToList<SelectListItem>().OrderBy(x => x.Text);
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Plate,ModelId,ColorId,ProductionYear,PersonCapacity,LoadLimit,Title,ImageFile,Id,CreatedBy,ModifiedBy,DeletedBy,CreatedDate,ModifiedDate,DeletedDate,IsDeleted")] Car carToUpdate)
        {
            Car carInDb = _context.Cars.Find(id);

            if (id != carInDb.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (carToUpdate.ImageFile != null)
                {
                    DeleteCustom(id);
                    try
                    {
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        string fileName = Path.GetFileNameWithoutExtension(carToUpdate.ImageFile.FileName);
                        string extension = Path.GetExtension(carToUpdate.ImageFile.FileName);
                        carToUpdate.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await carToUpdate.ImageFile.CopyToAsync(fileStream);
                        }

                        carInDb.Plate = carToUpdate.Plate;
                        carInDb.ModelId = carToUpdate.ModelId;
                        carInDb.ColorId = carToUpdate.ColorId;
                        carInDb.ProductionYear = carToUpdate.ProductionYear;
                        carInDb.PersonCapacity = carToUpdate.PersonCapacity;
                        carInDb.LoadLimit = carToUpdate.LoadLimit;
                        carInDb.Title = carToUpdate.Title;
                        carInDb.ImageName = carToUpdate.ImageName;
                        _context.Update(carInDb);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CarExists(carToUpdate.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                else
                {
                    try
                    {                        
                        carInDb.Plate = carToUpdate.Plate;
                        carInDb.ModelId = carToUpdate.ModelId;
                        carInDb.ColorId = carToUpdate.ColorId;
                        carInDb.ProductionYear = carToUpdate.ProductionYear;
                        carInDb.PersonCapacity = carToUpdate.PersonCapacity;
                        carInDb.LoadLimit = carToUpdate.LoadLimit;
                        carInDb.Title = carToUpdate.Title;
                        _context.Update(carInDb);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CarExists(carToUpdate.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                
                return RedirectToAction(nameof(Index));
            }
            ViewData["ColorId"] = new SelectList(_context.Colors, "Id", "ColorName", carInDb.ColorId);
            ViewData["ModelId"] = new SelectList(_context.Models, "Id", "ModelName", carInDb.ModelId);
            return View(carInDb);
        }

        // GET: Cars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cars == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .Include(c => c.Color)
                .Include(c => c.Model)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cars == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Cars'  is null.");
            }
            var car = await _context.Cars.FindAsync(id);
            if (car != null)
            {
                var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "image", car.ImageName);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
                _context.Cars.Remove(car);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private void DeleteCustom(int id)
        {
            //if (_context.Cars == null)
            //{
            //    return Problem("Entity set 'ApplicationDbContext.Cars'  is null.");
            //}
            Car car = _context.Cars.Find(id);

            if (car != null)
            {
                var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "image", car.ImageName);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

            }

        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.Id == id);
        }
    }
}
