namespace OrderService.DTOs;

public class CreateOrderRequest
{
    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal TotalAmount { get; set; }
}