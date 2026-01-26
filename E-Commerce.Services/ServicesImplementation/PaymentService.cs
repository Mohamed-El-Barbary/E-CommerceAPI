using AutoMapper;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.BasketModule;
using E_Commerce.Domain.Entities.OrderModule;
using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Services_Abstraction;
using E_Commerce.Shared.CommonResult;
using E_Commerce.Shared.DTOs.BasketDTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = E_Commerce.Domain.Entities.ProductModule.Product;

namespace E_Commerce.Services.ServicesImplementation
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public PaymentService(IBasketRepository basketRepository, IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<Result<BasketDTO>> CreateOrUpdatePaymentIntentAsync(string baskedId)
        {

            var skey = _configuration["Stripe:Skey"];
            if (skey is null) return Error.Failure("Failed To Obtain Secret Key Value");
            StripeConfiguration.ApiKey = skey;

            // 1- Retrive The Basket With Its Id
            var basket = await _basketRepository.GetBasketAsync(baskedId);
            if (basket is null) return Error.NotFound("Basket Not Found");
            // 2- Validate Delivery Method Inside The Basket
            if (basket.DeliveryMethodId is null) return Error.Validation("Delivery Method Is Not Selected In The Basket");
            // 3- Retrive The Delivery Method details From the database
            var deliveyrMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>()
                                            .GetByIdAsync(basket.DeliveryMethodId.Value);

            if (deliveyrMethod is null) return Error.NotFound("Delivery Method Not Found");

            basket.ShippingPrice = deliveyrMethod.Price;

            // 4- Check The Product price 
            foreach (var item in basket.Items)
            {
                var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(item.Id);
                if (product is null) return Error.NotFound("Product.NotFound");

                item.Price = product.Price;
                item.ProductName = product.Name;
                item.PictureUrl = product.PictureUrl;
            }

            long amount = (long)(basket.Items.Sum(x => x.Price * x.Quantity) * 100);
            basket.TotalPrice = amount / 100;
            // 5-Create or update payment intent with Stripe API

            var stripeService = new PaymentIntentService();

            //basket PaymentIntentId is null =>Create
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                #region Integration with External Servic
                //Integration With any external service 
                //Download Stripe Nuget Package
                // Collection of classes as DLL
                // Main class to interact with Stripe API [Create from it object]
                // Use service inside the main Object [Call function]
                #endregion
                var options = new PaymentIntentCreateOptions
                {
                    Amount = amount,
                    Currency = "usd",
                    PaymentMethodTypes = ["card"]
                };

                var paymentIntent = await stripeService.CreateAsync(options);

                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;

            }
            //basket PaymentIntentId is not null =>Update
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = amount
                };

                await stripeService.UpdateAsync(basket.PaymentIntentId, options);
            }

            await _basketRepository.CreateOrUpdateBasketAsync(basket);

            return _mapper.Map<CustomerBasket, BasketDTO>(basket);
        }
    }
}
