using AutoMapper;
using BusinessLogic.DTOs.AuthDto;
using BusinessLogic.DTOs.ProductDto;
using BusinessLogic.DTOs.ProductsDto;
using Entities.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Mapper
{
    public class Mapper: AutoMapper.Profile
    {
        public Mapper()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();

            CreateMap<ProductImages, ProductImagesDto>();
            CreateMap<ProductImagesDto, ProductImages>();

            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();

            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

        }
    }
}
