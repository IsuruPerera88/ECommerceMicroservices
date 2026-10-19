using Microsoft.AspNetCore.Mvc;
using OrderService.DTOs;
using OrderService.Responses;
using OrderService.Services.Interfaces;

namespace OrderService.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly IServiceOrder _orderService;

    public OrdersController(
        IServiceOrder orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<
        ActionResult<ApiResponse<List<OrderDto>>>>
        GetOrders()
    {
        var response =
            await _orderService.GetAllAsync();

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<
        ActionResult<ApiResponse<OrderDto>>>
        GetOrder(int id)
    {
        var response =
            await _orderService.GetByIdAsync(id);

        return Ok(response);
    }

    [HttpPost]
    public async Task<
        ActionResult<ApiResponse<OrderDto>>>
        CreateOrder(
            CreateOrderRequest request,
            CancellationToken cancellationToken)
    {
        var response =
            await _orderService.CreateAsync(request, cancellationToken);

        return Created(
            $"/api/orders/{response.Data?.Id}",
            response);
    }

    [HttpPut("{id:int}")]
    public async Task<
        ActionResult<ApiResponse<OrderDto>>>
        UpdateOrder(
            int id,
            CreateOrderRequest request)
    {
        var response =
            await _orderService.UpdateAsync(
                id,
                request);

        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<
        ActionResult<ApiResponse<bool>>>
        DeleteOrder(int id)
    {
        var response =
            await _orderService.DeleteAsync(id);

        return Ok(response);
    }
}