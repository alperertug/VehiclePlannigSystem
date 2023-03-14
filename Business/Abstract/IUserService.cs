using DataAccess.Identity.Data;

namespace Business.Abstract
{
    public interface IUserService
    {
        ICollection<ApplicationUser> GetAllUsers();
        int TotalManagers();
        int TotalUsers();
        string RolePrinter(ApplicationUser user);
    }
}
