using Microsoft.AspNetCore.Identity;

namespace AktifVehiclePlanningSystem.Core
{
    public interface IRoleRepository
    {
        ICollection<IdentityRole> GetRoles();
    }
}
