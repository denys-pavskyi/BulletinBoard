using AutoMapper;
using BulletinBoard.BLL.Models.DtoModels;
using BulletinBoard.DAL.Entities;

namespace BulletinBoard.BLL.Other;

public class MapperProfile: Profile
{

    public MapperProfile()
    {

        CreateMap<User, UserDto>()
            .ReverseMap();

        CreateMap<Announcement, AnnouncementDto>()
            .ReverseMap();

        CreateMap<Category, CategoryDto>()
            .ReverseMap();

        CreateMap<Subcategory, SubcategoryDto>()
            .ReverseMap();

    }


}