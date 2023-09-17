using API.DTOs;
using API.Entities;
using AutoMapper;

namespace API.RequestHelpers
{
    //bind the properties of the CreateProductDto to the Product entity class
    //this is used in the ProductsController
    //ca aide a mapper les propriétés de CreateProductDto à Product
    //pour eviter de faire product.Name = createProductDto.Name etc
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();

        }
    }
}