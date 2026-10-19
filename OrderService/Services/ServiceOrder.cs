using AutoMapper;
using OrderService.Clients.Interfaces;
using OrderService.DTOs;
using OrderService.Exceptions;
using OrderService.Models;
using OrderService.Repositories.Interfaces;
using OrderService.Responses;
using OrderService.Services.Interfaces;

namespace OrderService.Services;

public class ServiceOrder : IServiceOrder
{
    private readonly IOrderRepository _repository;
    private readonly IMapper _mapper;
    private readonly IProductClient _productClient;

    public ServiceOrder(
        IOrderRepository repository,
        IMapper mapper,
        IProductClient productClient)
    {
        _repository = repository;
        _mapper = mapper;
        _productClient = productClient;
    }

    public async Task<ApiResponse<List<OrderDto>>> GetAllAsync()
    {
        var orders = await _repository.GetAllAsync();

        var orderDtos =
            _mapper.Map<List<OrderDto>>(orders);

        return ApiResponse<List<OrderDto>>.Ok(
            orderDtos,
            "Orders retrieved successfully");
    }

    public async Task<ApiResponse<OrderDto>> GetByIdAsync(
        int id)
    {
        var order =
            await _repository.GetByIdAsync(id);

        if (order == null)
        {
            throw new NotFoundException($"Order with ID {id} was not found.");
        }

        var orderDto =
            _mapper.Map<OrderDto>(order);

        return ApiResponse<OrderDto>.Ok(
            orderDto,
            "Order retrieved successfully");
    }

    public async Task<ApiResponse<OrderDto>> CreateAsync(
        CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var product =
        await _productClient.GetProductByIdAsync(
            request.ProductId, cancellationToken);

        if (product == null)
        {
            throw new BadRequestException(
                $"Product with ID {request.ProductId} does not exist.");
        }

        if (product.StockQuantity < request.Quantity)
        {
            throw new BadRequestException(
                $"Insufficient stock for product {product.Id}.");
        }

        var order =
            _mapper.Map<Order>(request);

        order.Status = "Pending";

        order.CreatedAt = DateTime.UtcNow;

        var createdOrder =
            await _repository.AddAsync(order);

        var orderDto =
            _mapper.Map<OrderDto>(createdOrder);

        return ApiResponse<OrderDto>.Ok(
            orderDto,
            "Order created successfully");
    }

    public async Task<ApiResponse<OrderDto>> UpdateAsync(
        int id,
        CreateOrderRequest request)
    {
        var order =
            await _repository.GetByIdAsync(id);

        if (order == null)
        {
            throw new NotFoundException($"Order with ID {id} was not found.");
        }

        order.ProductId = request.ProductId;
        order.Quantity = request.Quantity;
        order.TotalAmount = request.TotalAmount;

        await _repository.UpdateAsync(order);

        var orderDto =
            _mapper.Map<OrderDto>(order);

        return ApiResponse<OrderDto>.Ok(
            orderDto,
            "Order updated successfully");
    }

    public async Task<ApiResponse<bool>> DeleteAsync(
        int id)
    {
        var order =
            await _repository.GetByIdAsync(id);

        if (order == null)
        {
            throw new NotFoundException($"Order with ID {id} was not found.");
        }

        await _repository.DeleteAsync(order);

        return ApiResponse<bool>.Ok(
            true,
            "Order deleted successfully");
    }
}