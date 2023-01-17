using AutoMapper;
using Entities.ViewModels;

namespace Business.AutoMapper.Color
{
    public class ColorProfile : Profile
    {
        public ColorProfile()
        {
            CreateMap<ColorViewModel, Entities.Concrete.Color>().ReverseMap();
        }
    }
}
