using Microsoft.AspNetCore.Mvc;
using ProductService.DTOs;
using ProductService.Responses;
using ProductService.Services.Interfaces;

namespace ProductService.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProdService _productService;

    public ProductsController(
        IProdService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<ProductDto>>>>
        GetProducts()
    {
        var response =
            await _productService.GetAllAsync();

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<ProductDto>>>
        GetProduct(int id)
    {
        var response =
            await _productService.GetByIdAsync(id);

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<ProductDto>>>
        CreateProduct(CreateProductRequest request)
    {
        var response =
            await _productService.CreateAsync(request);

        return Created(
            $"/api/products/{response.Data?.Id}",
            response);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<ProductDto>>>
        UpdateProduct(
            int id,
            CreateProductRequest request)
    {
        var response =
            await _productService.UpdateAsync(
                id,
                request);

        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<bool>>>
        DeleteProduct(int id)
    {
        var response =
            await _productService.DeleteAsync(id);

        return Ok(response);
    }
}