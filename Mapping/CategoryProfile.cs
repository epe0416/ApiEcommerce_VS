using ApiEcommerce_VS.Models;
using ApiEcommerce_VS.Models.Dtos;
using AutoMapper;

namespace ApiEcommerce_VS.Mapping
{
    public class CategoryProfile:Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CreateCategoryDto>().ReverseMap();
        }
    }
}
