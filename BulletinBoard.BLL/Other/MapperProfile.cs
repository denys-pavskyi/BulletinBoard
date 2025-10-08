using AutoMapper;
using BulletinBoard.BLL.Models.DtoModels;
using BulletinBoard.BLL.Models.Requests;
using BulletinBoard.DAL.Entities;

namespace BulletinBoard.BLL.Other;

public class MapperProfile: Profile
{

    public MapperProfile()
    {

        CreateMap<User, UserDto>()
            .ReverseMap();

        CreateMap<Post, PostDto>()
            .ForMember(dto => dto.CategoryId, obj => obj.MapFrom(src => src.SubCategory.Category.Id))
            .ForMember(dto => dto.SubcategoryName, obj => obj.MapFrom(src => src.SubCategory.Name))
            .ForMember(dto => dto.CategoryName, obj => obj.MapFrom(src => src.SubCategory.Category.Name))
            .ReverseMap();

        CreateMap<CreateNewPostRequest, Post>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));

        CreateMap<Category, CategoryDto>()
            .ReverseMap();

        CreateMap<Subcategory, SubcategoryDto>()
            .ReverseMap();

    }


}