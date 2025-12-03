using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Web.CustomMiddleWares;

public class ExceptionHandlerMiddleWare
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleWare> _logger;

    public ExceptionHandlerMiddleWare(RequestDelegate next, ILogger<ExceptionHandlerMiddleWare> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {

        try
        {
            await _next.Invoke(httpContext);

            if (httpContext.Response.StatusCode == StatusCodes.Status404NotFound)
            {
                var responseBody = new ProblemDetails()
                {
                    Title = "Error While Processing HTTP Request, EndPoint Not Found",
                    Detail = $"Endpoint {httpContext.Request.Path} Not Found",
                    Status = StatusCodes.Status404NotFound,
                    Instance = httpContext.Request.Path
                };
                await httpContext.Response.WriteAsJsonAsync(responseBody);
            }
            
        }
        catch (Exception ex)
        {
            // Logging
            _logger.LogError(ex, "Something went wrong");
            // Return Custom Error Response
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var problem = new ProblemDetails()
            {
                Title = "Internal Server Error",
                Detail = ex.Message,
                Status = StatusCodes.Status500InternalServerError,
                Instance = httpContext.Request.Path
            };
            await httpContext.Response.WriteAsJsonAsync(problem);
        }
        
    }
    
}