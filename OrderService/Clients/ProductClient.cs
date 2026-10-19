using System.Net;
using System.Net.Http.Json;
using OrderService.Clients.Interfaces;
using OrderService.DTOs;
using OrderService.Exceptions;

namespace OrderService.Clients;

public class ProductClient : IProductClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ProductClient> _logger;

    public ProductClient(
        HttpClient httpClient,
        ILogger<ProductClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<ProductDto?> GetProductByIdAsync(
        int productId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response =
                await _httpClient.GetAsync(
                    $"api/products/{productId}",
                    cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogWarning(
                    "Product {ProductId} was not found",
                    productId);

                return null;
            }

            if (response.StatusCode ==
                HttpStatusCode.InternalServerError)
            {
                _logger.LogError(
                    "ProductService returned 500 for Product {ProductId}",
                    productId);

                throw new BadRequestException(
                    "Unable to verify product because ProductService is unavailable.");
            }

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(
                    "ProductService returned status code {StatusCode}",
                    response.StatusCode);

                throw new BadRequestException(
                    "Unable to retrieve product information.");
            }

            var apiResponse =
                await response.Content
                    .ReadFromJsonAsync<
                        ProductApiResponse>(
                        cancellationToken);

            return apiResponse?.Data;
        }
        catch (TaskCanceledException)
            when (!cancellationToken.IsCancellationRequested)
        {
            _logger.LogError(
                "Request to ProductService timed out for Product {ProductId}",
                productId);

            throw new BadRequestException(
                "ProductService request timed out.");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(
                ex,
                "Unable to communicate with ProductService");

            throw new BadRequestException(
                "ProductService is currently unavailable.");
        }
    }
}