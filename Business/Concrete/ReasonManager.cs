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
    public class ReasonManager : IReasonService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReasonManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public ReasonForReservation GetReason(int id)
        {
            return _unitOfWork.GetDbContext().ReasonForReservations.FirstOrDefault(x => x.Id == id);
        }
    }
}
