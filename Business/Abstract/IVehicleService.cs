using Entities.Concrete;
using Entities.ViewModels;

namespace Business.Abstract
{
    public interface IVehicleService
    {
        Task<List<VehicleViewModel>> GetAllVehiclesWithModelNonDeletedAsync();
        Task<List<Car>> GetAllCarsAsync();
        Car GetVehicle(int id);
        Task<VehicleViewModel> GetVehicleByIdAsync(int id);
    }
}
