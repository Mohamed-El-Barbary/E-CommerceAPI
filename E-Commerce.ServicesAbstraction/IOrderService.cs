using E_Commerce.Shared.CommonResult;
using E_Commerce.Shared.DTOs.OrderDTOs;

namespace E_Commerce.Services_Abstraction;

public interface IOrderService
{
    // Create Order 
    // OrderDto , Email => OrderToReturnDto
    Task<Result<OrderToReturnDTO>> CreateOrderAsync(OrderDTO orderDto, string email);
    
    // Get Delivery Methods
    Task<Result<IEnumerable<DeliveryMethodDTO>>> GetAllDeliverMothodsAsync();
}