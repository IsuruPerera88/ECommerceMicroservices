using OrderService.Middleware;

namespace OrderService.Extensions;

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