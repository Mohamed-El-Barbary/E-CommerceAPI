namespace E_Commerce.Services.Exceptions
{
    public abstract class NotFoundExceptions(string message) : Exception(message);

    public sealed class ProductNotFoundExceptions(int id) : NotFoundExceptions($"Product with id {id} not found.");

    public sealed class BasketNotFoundExceptions(string id) : NotFoundExceptions($"Product with id {id} not found.");
    
}


