using AktifVehiclePlanningSystem.Core;
using DataAccess.Context;
using Microsoft.AspNetCore.Identity;

namespace AktifVehiclePlanningSystem.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public ICollection<IdentityRole> GetRoles()
        {
            return _context.Roles.ToList();
        }
    }
}
