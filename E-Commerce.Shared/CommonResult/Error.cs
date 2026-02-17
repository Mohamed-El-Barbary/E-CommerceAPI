namespace E_Commerce.Shared.CommonResult;

public class Error
{
    public string Code { get; }
    public string Description { get; }
    public ErrorType Type { get; }
    
    private Error(string code, string description, ErrorType type)
    {
        Code = code;
        Description = description;
        Type = type;
    }

    #region Static Factory Method

    public static Error Failure(
        string Code = "General.Failure",
        string Description = "General Failure Has Occurred")
        => new Error(code: Code, description: Description, type: ErrorType.Failure);

    public static Error Validation(
        string Code = "General.Validation",
        string Description = "Validation Error Has Occurred")
        => new Error(code: Code, description: Description, type: ErrorType.Validation);

    public static Error NotFound(
        string Code = "General.NotFound",
        string Description = "The Requested Resource Was Not Found")
        => new Error(code: Code, description: Description, type: ErrorType.NotFound);

    public static Error Unauthorized(
        string Code = "General.Unauthorized",
        string Description = "You Are Not Authorized To Perform This Action")
        => new Error(code: Code, description: Description, type: ErrorType.Unauthorized);

    public static Error Forbidden(
        string Code = "General.Forbidden",
        string Description = "You Do Not Have Permission To Access This Resource")
        => new Error(code: Code, description: Description, type: ErrorType.Forbidden);

    public static Error InvalidCredentials(
        string Code = "General.InvalidCredentials",
        string Description = "The Provided Credentials Are Invalid")
        => new Error(code: Code, description: Description, type: ErrorType.InvalidCredentials);
    

    #endregion
    
}