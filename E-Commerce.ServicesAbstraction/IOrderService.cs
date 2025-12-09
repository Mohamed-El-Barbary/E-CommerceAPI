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
    
    // Get All Order For Specific User By Mail 
    Task<Result<IEnumerable<OrderToReturnDTO>>> GetAllOrdersAsync(string email);
    
    // Get Specific Order For Specific User By Id and Email
    Task<Result<OrderToReturnDTO>> GetOrderByIdAsync(Guid id, string email);
}