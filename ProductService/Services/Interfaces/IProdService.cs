using ProductService.DTOs;
using ProductService.Responses;

namespace ProductService.Services.Interfaces;

public interface IProdService
{
    Task<ApiResponse<List<ProductDto>>> GetAllAsync();

    Task<ApiResponse<ProductDto>> GetByIdAsync(int id);

    Task<ApiResponse<ProductDto>> CreateAsync(
        CreateProductRequest request);

    Task<ApiResponse<ProductDto>> UpdateAsync(
        int id,
        CreateProductRequest request);

    Task<ApiResponse<bool>> DeleteAsync(int id);
}