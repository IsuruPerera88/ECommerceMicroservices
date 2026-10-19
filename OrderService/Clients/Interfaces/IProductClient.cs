using OrderService.DTOs;

namespace OrderService.Clients.Interfaces;

public interface IProductClient
{
    Task<ProductDto?> GetProductByIdAsync(
        int productId,
        CancellationToken cancellationToken = default);
}