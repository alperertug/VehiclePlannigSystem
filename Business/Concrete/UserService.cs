using Business.Abstract;
using DataAccess.Identity.Data;
using DataAccess.UnitOfWorks;

namespace Business.Concrete
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public ICollection<ApplicationUser> GetAllUsers()
        {
            return _unitOfWork.GetDbContext().Users.ToList();
        }

        public string RolePrinter(ApplicationUser user)
        {            
            var roles = _unitOfWork.GetDbContext().UserRoles.ToList();
            var isExist = false;

            foreach (var role in roles)
            {
                if (user.Id == role.UserId)
                {
                    isExist = true;
                }
            }

            if (isExist == false)
                return "This user has no role!";

            var roleIds = _unitOfWork.GetDbContext().UserRoles.Where(f => f.UserId == user.Id).ToList();
            var roleNames = new List<string>();
            foreach (var role in roleIds)
            {
                roleNames.Add(_unitOfWork.GetDbContext().Roles.FirstOrDefault(x => x.Id == role.RoleId).Name);
            }

            var roleStr = "";
            foreach (var roleName in roleNames)
            {
                roleStr += roleName.ToString() + ", ";
            }


            return roleStr.Remove((roleStr.Length - 2), 2);            
        }

        public int TotalManagers()
        {
            var managerId = _unitOfWork.GetDbContext().Roles.FirstOrDefault(m => m.Name == "Manager").Id;
            return _unitOfWork.GetDbContext().UserRoles.Where(a => a.RoleId == managerId).Count();
        }

        public int TotalUsers()
        {
            var userId = _unitOfWork.GetDbContext().Roles.FirstOrDefault(m => m.Name == "User").Id;
            return _unitOfWork.GetDbContext().UserRoles.Where(a => a.RoleId == userId).Count();
        }
    }
}
