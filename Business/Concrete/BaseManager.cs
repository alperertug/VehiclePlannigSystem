using Business.Abstract;
using DataAccess.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class BaseManager<T> : IBaseService<T> where T : class
    {
        private readonly IUnitOfWork _unitOfWork;

        public BaseManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void DeleteObject(T obj)
        {
            _unitOfWork.GetDbContext().Remove(obj);
            _unitOfWork.GetDbContext().SaveChanges();
        }
    }
}
