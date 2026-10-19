namespace OrderService.DTOs;

public class ProductApiResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public ProductDto? Data { get; set; }
}