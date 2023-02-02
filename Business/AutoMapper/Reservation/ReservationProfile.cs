using AutoMapper;
using Entities.ViewModels;

namespace Business.AutoMapper.Reservation
{
    public class ReservationProfile : Profile
    {
        public ReservationProfile()
        {
            CreateMap<ReservationViewModel, Entities.Concrete.Reservation>().ReverseMap();
        }
    }
}
