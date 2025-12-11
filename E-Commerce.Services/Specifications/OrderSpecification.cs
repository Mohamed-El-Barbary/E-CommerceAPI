using E_Commerce.Domain.Entities.OrderModule;

namespace E_Commerce.Services.Specifications;

internal class OrderSpecification : BaseSpecifications<Order, Guid>
{
    public OrderSpecification(string email) : base(o => o.UserEmail == email)
    {
        AddInclude(o => o.Items);
        AddInclude(o => o.DeliveryMethod);
        AddOrderByDescending(o => o.OrderDate);
    }
    
    public OrderSpecification(Guid id,string email) : base(o => o.UserEmail == email&& o.Id == id )
    {
        AddInclude(o => o.Items);
        AddInclude(o => o.DeliveryMethod);
    }
    
}