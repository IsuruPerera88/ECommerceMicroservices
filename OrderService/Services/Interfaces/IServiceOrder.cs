using OrderService.DTOs;
using OrderService.Responses;

namespace OrderService.Services.Interfaces;

public interface IServiceOrder
{
    Task<ApiResponse<List<OrderDto>>> GetAllAsync();

    Task<ApiResponse<OrderDto>> GetByIdAsync(int id);

    Task<ApiResponse<OrderDto>> CreateAsync(
        CreateOrderRequest request,
        CancellationToken cancellationToken);

    Task<ApiResponse<OrderDto>> UpdateAsync(
        int id,
        CreateOrderRequest request);

    Task<ApiResponse<bool>> DeleteAsync(int id);
}