using ProductService.Middleware;

namespace ProductService.Extensions;

public static class ApplicationExtensions
{
    public static IApplicationBuilder
        UseGlobalExceptionHandler(
            this IApplicationBuilder app)
    {
        return app.UseMiddleware<
            GlobalExceptionHandler>();
    }
}