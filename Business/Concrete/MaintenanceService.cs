using Business.Abstract;
using DataAccess.UnitOfWorks;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class MaintenanceService : IMaintenanceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MaintenanceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public List<Maintenance> GetMaintenances()
        {
            return _unitOfWork.GetDbContext().Maintenances.Where(x => x.IsDeleted == false).ToList();
        }
    }
}
