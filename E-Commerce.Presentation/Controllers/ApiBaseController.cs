using System.Security.Claims;
using E_Commerce.Shared.CommonResult;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace E_Commerce.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApiBaseController : ControllerBase
{

    // Handle Result Without Value
    protected IActionResult HandleResult(Result result)
    {
        if (result.IsSuccess)
            return NoContent();
        else
            return HandleProblem(result.Errors);
    }

    protected ActionResult<TValue> HandleResult<TValue>(Result<TValue> result)
    {
        if (result.IsSuccess)
            return Ok(result.Value);
        else
            return HandleProblem(result.Errors);
    }

    protected string GetEmailFromToken() => User.FindFirstValue(ClaimTypes.Email)!;

    protected void SetRefreshTokenCookie(string refreshToken, DateTime expires)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = expires.ToLocalTime(),
        };

        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }

    private ActionResult HandleProblem(IReadOnlyList<Error> errors)
    {
        // If No Errors Are Provided , Return 500 Error
        if (errors.Count == 0)
            return Problem(statusCode: StatusCodes.Status500InternalServerError, title: "An un Expected Error has occured");
        // If All Errors are Validation Errors , Handle Them As Validation Problem
        if (errors.All(e => e.Type == ErrorType.Validation))
            return HandleValidationProblem(errors);
        // If There's Only One Error , Handle It As A Single Error Problem
        return HandleSingleProblem(errors[0]);

    }

    private ActionResult HandleSingleProblem(Error error)
    {
        return Problem(
            title: error.Code,
            detail: error.Description,
            type: error.Type.ToString(),
            statusCode: MapErrorTypeToStatusCode(error.Type)
        );
    }

    private static int MapErrorTypeToStatusCode(ErrorType errorType) => errorType switch
    {
        ErrorType.NotFound => StatusCodes.Status404NotFound,
        ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
        ErrorType.Forbidden => StatusCodes.Status403Forbidden,
        ErrorType.Validation => StatusCodes.Status400BadRequest,
        ErrorType.InvalidCredentials => StatusCodes.Status401Unauthorized,
        ErrorType.Failure => StatusCodes.Status500InternalServerError,
        _ => StatusCodes.Status500InternalServerError
    };

    private ActionResult HandleValidationProblem(IReadOnlyList<Error> errors)
    {
        var modelState = new ModelStateDictionary();

        foreach (var error in errors)
            modelState.AddModelError(error.Code, error.Description);

        return ValidationProblem(modelState);
    }

}