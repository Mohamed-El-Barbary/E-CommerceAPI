using AutoMapper;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.BasketModule;
using E_Commerce.Domain.Entities.OrderModule;
using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Services_Abstraction;
using E_Commerce.Shared.CommonResult;
using E_Commerce.Shared.DTOs.OrderDTOs;

namespace E_Commerce.Services;

public class OrderService : IOrderService
{
    private readonly IMapper _mapper;
    private readonly IBasketRepository _basketRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IMapper mapper, IBasketRepository basketRepository, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _basketRepository = basketRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<OrderToReturnDTO>> CreateOrderAsync(OrderDTO orderDto, string email)
    {
        var orderAddress = _mapper.Map<OrderAddress>(orderDto.Address);

        var basket = await _basketRepository.GetBasketAsync(orderDto.BasketId);
        if (basket is null)
            return Error.NotFound("Basket.NotFound", $"The Basket With Id {orderDto.BasketId} Is Not Found");

        List<OrderItem> orderItems = new List<OrderItem>();
        foreach (var item in basket.Items)
        {
            var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(item.Id);
            if (product is null)
                return Error.NotFound("Product.NotFound", $"The Product With Id {item.Id} Is Not Found");
            orderItems.Add(CreateOrderItem(product, item));
        }

        var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(orderDto.DeliveryMethodId);
        if (deliveryMethod is null)
            return Error.NotFound("DeliverMethod.NotFound",
                $"The Deliver Method With Id {orderDto.DeliveryMethodId} Is Not Found");

        var subTotal = orderItems.Sum(i => i.Price * i.Quantity);

        var order = new Order()
        {
            Address = orderAddress,
            DeliveryMethod = deliveryMethod,
            Items = orderItems,
            SubTotal = subTotal,
            UserEmail = email
        };

        await _unitOfWork.GetRepository<Order, Guid>().AddAsync(order);
        int result = await _unitOfWork.SaveChangesAsync();
        if (result == 0) return Error.Failure("Order.Failure", "Order Can Not Be Created");

        return _mapper.Map<OrderToReturnDTO>(order);
    }

    private static OrderItem CreateOrderItem(Product product, BasketItem item)
    {
        return new OrderItem()
        {
            Product = new ProductItemOrdered()
            {
                ProductId = product.Id,
                ProductName = product.Name,
                PictureUrl = product.PictureUrl
            },
            Price = product.Price,
            Quantity = item.Quantity
        };
    }
}