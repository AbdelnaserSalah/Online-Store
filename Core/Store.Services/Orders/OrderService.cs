using AutoMapper;
using Store.Domain.Contracts;
using Store.Domain.Entities.Orders;
using Store.Domain.Entities.Products;
using Store.Domain.Exceptions.BadRequest;
using Store.Domain.Exceptions.NotFound;
using Store.Services.Abstractions.Orders;
using Store.Services.Specifcations.Orders;
using Store.Shared.Dtos.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Orders
{
    public class OrderService(IUnitofWork unitofWork,IMapper mapper,IBasketRepository basketRepository) : IOrderService
    {
        public async Task<OrderResponse?> CreateOrderAsync(OrderRequest request, string UserEmail)
        {
            // get order address
          var orderAddress =  mapper.Map<OrderAddress>(request.ShiptoAddress);

            // get delivery method by id
            var deliveryMethod = await unitofWork.GetRepository<int, DeliveryMethod>().GetAsync(request.DeliveryMethodId);
            if(deliveryMethod == null ) throw new DeliveryMethodNotFound(request.DeliveryMethodId);

            // get order items by basket id
             var basket = await basketRepository.GetBasketAsync(request.BasketId);
             if(basket is null) throw new BasketNotFoundException(request.BasketId);

            // convert basket items to order items
             var orderItems = new List<OrderItem>();

                foreach(var item in basket.Items)
                {
                // check price of Db the same price in basket
                  var product =await unitofWork.GetRepository<int, Product>().GetAsync(item.Id);
                   if(product == null) throw new ProductNotFoundExeception(item.Id);

                   if(product.Price != item.Price) item.Price=product.Price;


                var productInOrderItem = new ProductInOrderItem(item.Id,item.ProductName,item.ProductUrl);
                   var orderItem = new OrderItem(productInOrderItem, item.Price,item.Quantity);
                   orderItems.Add(orderItem);
                }

            // calculate subtotal
            var subtotal = orderItems.Sum(item => item.Price * item.Quantity);

            // create order 
            var order = new Order(UserEmail, orderAddress, deliveryMethod, orderItems, subtotal);
          
           await unitofWork.GetRepository<Guid, Order>().AddAsync(order);
            var result = await unitofWork.SaveChangesAsync();
            if (result <=0) throw new CreateOrderBadRequestException();


            return  mapper.Map<OrderResponse>(order); ;
        }

        public async Task<IEnumerable<DeliveryMethodResponse>> GetAllDeliveryMethodsAsync()
        {
          var deliverymethods =await unitofWork.GetRepository<int, DeliveryMethod>().GetAllAsync();
            return mapper.Map<IEnumerable<DeliveryMethodResponse>>(deliverymethods);
        }

        public async Task<OrderResponse?> GetOrderByIdForSpecificUserAsync(Guid userId, string UserEmail)
        {
            var spec = new OrderSpecification(userId, UserEmail);
          
            var order =   await unitofWork.GetRepository<Guid, Order>().GetAsync(spec);
            if(order == null) throw new OrderNotFoundException(userId);
            return mapper.Map<OrderResponse>(order);
        }

        public async Task<IEnumerable<OrderResponse>> GetOrdersForSpecificUserAsync(string UserEmail)
        {
            var spec = new OrderSpecification(UserEmail);

            var orders = await unitofWork.GetRepository<Guid, Order>().GetAllAsync(spec);
            return mapper.Map<IEnumerable<OrderResponse>>(orders);
        }
    }
}
