using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Web.Factories;

public static class ApiResponseFactory
{

    public static IActionResult GenerateApiValidationResponse(ActionContext actionContext)
    {

        var Errors = actionContext.ModelState.Where(x => x.Value.Errors.Count > 0)
            .ToDictionary(x => x.Key, x => x.Value.Errors.Select(x => x.ErrorMessage))
            .ToArray();
        
        var problem = new ProblemDetails()
        {
            Title = "Validation Errors",
            Detail = "One Or More Validation Errors occurred.",
            Status = StatusCodes.Status404NotFound,
            Extensions = {{"Errors", Errors}},
        };
        
        return new BadRequestObjectResult(problem);
    }

}