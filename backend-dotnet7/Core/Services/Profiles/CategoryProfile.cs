using AutoMapper;
using backend_dotnet7.Core.Dtos.Category;
using backend_dotnet7.Core.Entities;

namespace backend_dotnet7.Core.Services.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>()
               .ForMember(des => des.IconwithTitle,
               opt => opt.MapFrom(src => $"{src.Icon},{src.Title}"));

            CreateMap<CreateCategoryDto, Category>();
        }
    }
}
