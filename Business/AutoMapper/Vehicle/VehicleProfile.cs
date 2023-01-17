using AutoMapper;
using Entities.Concrete;
using Entities.ViewModels;

namespace Business.AutoMapper.Vehicle
{
    public class VehicleProfile : Profile
    {
        public VehicleProfile()
        {
            CreateMap<VehicleViewModel, Car>().ReverseMap();            
        }
    }
}
