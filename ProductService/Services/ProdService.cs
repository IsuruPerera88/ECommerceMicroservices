using AutoMapper;
using ProductService.DTOs;
using ProductService.Exceptions;
using ProductService.Models;
using ProductService.Repositories.Interfaces;
using ProductService.Responses;
using ProductService.Services.Interfaces;

namespace ProductService.Services;

public class ProdService : IProdService
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public ProdService(
        IProductRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<ProductDto>>> GetAllAsync()
    {
        var products = await _repository.GetAllAsync();

        var productDtos = _mapper.Map<List<ProductDto>>(products);

        return ApiResponse<List<ProductDto>>.Ok(
            productDtos,
            "Products retrieved successfully");
    }

    public async Task<ApiResponse<ProductDto>> GetByIdAsync(int id)
    {
        var product = await _repository.GetByIdAsync(id);

        if (product == null)
        {
            throw new NotFoundException($"Product with ID {id} was not found.");
        }

        var productDto = _mapper.Map<ProductDto>(product);

        return ApiResponse<ProductDto>.Ok(
            productDto,
            "Product retrieved successfully");
    }

    public async Task<ApiResponse<ProductDto>> CreateAsync(
        CreateProductRequest request)
    {
        var product = _mapper.Map<Product>(request);

        product.CreatedAt = DateTime.UtcNow;

        var createdProduct =
            await _repository.AddAsync(product);

        var productDto =
            _mapper.Map<ProductDto>(createdProduct);

        return ApiResponse<ProductDto>.Ok(
            productDto,
            "Product created successfully");
    }

    public async Task<ApiResponse<ProductDto>> UpdateAsync(
        int id,
        CreateProductRequest request)
    {
        var product = await _repository.GetByIdAsync(id);

        if (product == null)
        {
            throw new NotFoundException($"Product with ID {id} was not found.");
        }

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.StockQuantity = request.StockQuantity;

        await _repository.UpdateAsync(product);

        var productDto =
            _mapper.Map<ProductDto>(product);

        return ApiResponse<ProductDto>.Ok(
            productDto,
            "Product updated successfully");
    }

    public async Task<ApiResponse<bool>> DeleteAsync(int id)
    {
        var product = await _repository.GetByIdAsync(id);

        if (product == null)
        {
            throw new NotFoundException($"Product with ID {id} was not found.");
        }

        await _repository.DeleteAsync(product);

        return ApiResponse<bool>.Ok(
            true,
            "Product deleted successfully");
    }
}