using E_Commerce.Domain.Entities.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Specifications.OrderSpecifications
{
    internal class OrderWithPaymentIntentSpecifications : BaseSpecifications<Order, Guid>
    {

        public OrderWithPaymentIntentSpecifications(string paymentIntent) : base(x => x.PaymentIntentId == paymentIntent)
        {

        }

    }
}
