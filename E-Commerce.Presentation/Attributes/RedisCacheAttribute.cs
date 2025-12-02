using System.Text;
using E_Commerce.Services_Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace E_Commerce.Presentation.Attributes;

internal class RedisCacheAttribute : ActionFilterAttribute
{
    private readonly int _durationInMinutes;

    public RedisCacheAttribute(int durationInMinutes = 5)
    {
        _durationInMinutes = durationInMinutes;
    }
    
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        
        // Get Cache Service from DI Container
        var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();

        // Create CacheKey Based on Request And Query String
        var cacheKey = CreateCacheKey(context.HttpContext.Request);
        
        // Check If Cached data Exists
        var cacheValue = await cacheService.GetAsync(cacheKey);
        if (cacheValue != null)
        {
            context.Result = new ContentResult()
            {
                Content = cacheValue,
                ContentType = "application/json",
                StatusCode = StatusCodes.Status200OK
            };
            
            return;
        }
        
        // If Not Exists, Execute the Endpoint and Store the Result in Cache is 200 Ok 
        var executedContext = await next.Invoke();

        if (executedContext.Result is ObjectResult objectResult)
        {
           await cacheService.SetAsync(cacheKey , objectResult.Value!, TimeSpan.FromMinutes(_durationInMinutes));
        }
        
    }

    private string CreateCacheKey(HttpRequest request)
    {
        StringBuilder key = new StringBuilder();

        key.Append(request.Path);
        foreach (var item in request.Query.OrderBy(x => x.Key))
        {
            key.Append($"{item.Key}-{item.Value}");
        }

        return key.ToString();
    }
}