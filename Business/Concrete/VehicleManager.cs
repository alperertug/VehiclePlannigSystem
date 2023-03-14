using AutoMapper;
using Business.Abstract;
using DataAccess.UnitOfWorks;
using Entities.Concrete;
using Entities.ViewModels;

namespace Business.Concrete
{
    public class VehicleManager : IVehicleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VehicleManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<VehicleViewModel>> GetAllVehiclesWithModelNonDeletedAsync()
        { 
            var vehicles = await _unitOfWork.GetRepository<Car>().GetAllAsync(x => !x.IsDeleted, x => x.Model, x => x.Color);
            var map = _mapper.Map<List<VehicleViewModel>>(vehicles);
            return map;
        }  
        
        public async Task<List<Car>> GetAllCarsAsync()
        {
            return await _unitOfWork.GetRepository<Car>().GetAllAsync(x => !x.IsDeleted);    
        }

        //public async Task<Car> GetVehicleByIdAsync(int id)
        //{
        //    return await _unitOfWork.GetRepository<Car>().GetByIdAsync(id);
            
        //}

        public async Task<VehicleViewModel> GetVehicleByIdAsync(int id)
        {

            var vehicle = await _unitOfWork.GetRepository<Car>().GetAsync(x => x.Id == id);
            var map = _mapper.Map<VehicleViewModel>(vehicle);

            return map;
        }

        public Car GetVehicle(int id)
        {
            return _unitOfWork.GetDbContext().Cars.FirstOrDefault(i => i.Id == id);
        }
    }
}
