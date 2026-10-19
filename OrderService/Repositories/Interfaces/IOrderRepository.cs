using OrderService.Models;

namespace OrderService.Repositories.Interfaces;

public interface IOrderRepository
{
    Task<List<Order>> GetAllAsync();

    Task<Order?> GetByIdAsync(int id);

    Task<Order> AddAsync(Order order);

    Task UpdateAsync(Order order);

    Task DeleteAsync(Order order);
}