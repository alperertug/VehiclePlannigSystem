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
    public class ModelService : IModelService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ModelService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public List<Model> GetAllModels()
        {
            return _unitOfWork.GetDbContext().Models.Where(m => m.IsDeleted == false).ToList();
        }
    }
}
