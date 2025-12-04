namespace E_Commerce.Services.Exceptions
{
    public abstract class NotFoundExceptions(string message) : Exception(message);

    public sealed class ProductNotFoundException(int id) : NotFoundExceptions($"Product with id {id} not found.");

    public sealed class BasketNotFoundException(string id) : NotFoundExceptions($"Basket with id {id} not found.");
    
}


