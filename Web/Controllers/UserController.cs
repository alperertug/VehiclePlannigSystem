using AktifVehiclePlanningSystem.Core;
using AktifVehiclePlanningSystem.Core.ViewModels;
using DataAccess.Context;
using DataAccess.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AktifVehiclePlanningSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _dbContext;

        public UserController(IUnitOfWork unitOfWork, SignInManager<ApplicationUser> signInManager, ApplicationDbContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _signInManager = signInManager;
            _dbContext = dbContext;
        }

        [Authorize(Policy = "RequireAdmin")]
        public IActionResult Index()
        {
            var users = _unitOfWork.User.GetUsers();
            return View(users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = _unitOfWork.User.GetUser(id);
            var roles = _unitOfWork.Role.GetRoles();

            var userRoles = await _signInManager.UserManager.GetRolesAsync(user);

            var roleItems = roles.Select(role =>
            new SelectListItem(
                role.Name,
                role.Id,
                userRoles.Any(ur => ur.Contains(role.Name))
                )
            ).ToList();

            var vm = new EditUserViewModel
            {
                User = user,
                Roles = roleItems
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> OnPostAsync(EditUserViewModel data)
        {
            var user = _unitOfWork.User.GetUser(data.User.Id);
            if(user == null)
            {
                return NotFound();
            }

            var userRolesInDb = await _signInManager.UserManager.GetRolesAsync(user);

            //Loop through the roles in ViewModel
            //Check if the Role is Assigned In Db
            //  If Assigned -> Do Nothing
            //  If Not Assigned -> Add Role


            var rolesToAdd = new List<string>();
            var rolesToDelete = new List<string>();


            foreach (var role in data.Roles)
            {
                var assignedInDb = userRolesInDb.FirstOrDefault(ur => ur == role.Text);
                if (role.Selected)
                {
                    if (assignedInDb == null)
                    {
                        rolesToAdd.Add(role.Text);                       
                    }
                }
                else
                {
                    if (assignedInDb != null)
                    {
                        rolesToDelete.Add(role.Text);                     
                    }
                }
            }

            if (rolesToAdd.Any())
            {
                await _signInManager.UserManager.AddToRolesAsync(user, rolesToAdd);
            }

            if (rolesToDelete.Any())
            {
                await _signInManager.UserManager.RemoveFromRolesAsync(user, rolesToDelete);
            }

            user.FirstName = data.User.FirstName;
            user.LastName = data.User.LastName;
            user.UserName = data.User.UserName;
            user.Email = data.User.Email;
            user.PhoneNumber = data.User.PhoneNumber;
            

            _unitOfWork.User.UpdateUser(user);
            //return RedirectToAction("Edit", new { id = user.Id });
            return RedirectToAction("Index", "User");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind(include:"FirstName,LastName,UserName,Email")] ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Add(user);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null || _dbContext.Users == null)
            {
                return NotFound();
            }

            var selectedUser = _dbContext.Users
                .FirstOrDefault(m => m.Id == id);
            if (selectedUser == null)
            {
                return NotFound();
            }

            return View(selectedUser);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_dbContext.Users == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ReasonForReservations'  is null.");
            }
            var userToBeDeleted = await _dbContext.Users.FindAsync(id);
            if (userToBeDeleted != null)
            {
                _dbContext.Users.Remove(userToBeDeleted);
            }

            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReasonForReservationExists(string id)
        {
            return _dbContext.Users.Any(e => e.Id == id);
        }
    }
}
