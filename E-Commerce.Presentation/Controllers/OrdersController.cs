using E_Commerce.Services_Abstraction;
using E_Commerce.Shared.DTOs.OrderDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Presentation.Controllers;

public class OrdersController : ApiBaseController
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }
    
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<OrderToReturnDTO>> CreateOrder(OrderDTO orderDto)
    {
        var result = await _orderService.CreateOrderAsync(orderDto, GetEmailFromToken());
        return HandleResult(result);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderToReturnDTO>>> GetOrders(string email)
    {
        var result = await _orderService.GetAllOrdersAsync(GetEmailFromToken());
        return HandleResult(result);
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrderToReturnDTO>> GetOrder(Guid id)
    {
        var result = await _orderService.GetOrderByIdAsync(id, GetEmailFromToken());
        return HandleResult(result);
    }

    [AllowAnonymous]
    [HttpGet("deliveryMethods")]
    public async Task<ActionResult<IEnumerable<DeliveryMethodDTO>>> GetDeliveryMethods()
    {
        var result = await _orderService.GetAllDeliverMothodsAsync();
        return HandleResult(result);
    }


}