using AutoMapper;
using Entities.ViewModels;

namespace Business.AutoMapper.Model
{
    public class ModelProfile : Profile
    {
        public ModelProfile()
        {
            CreateMap<ModelsViewModel, Entities.Concrete.Model>().ReverseMap();
        }
    }
}
