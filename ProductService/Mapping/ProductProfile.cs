using AutoMapper;
using ProductService.DTOs;
using ProductService.Models;

namespace ProductService.Mapping;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>();

        CreateMap<CreateProductRequest, Product>();
    }
}