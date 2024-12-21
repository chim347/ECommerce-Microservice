using OrderApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.DTOs.Conversions
{
    public static class OrderConversion
    {
        public static Order ToEntity(OrderDTO order)
        {
            return new Order
            {
                Id = order.Id,
                ProductId = order.ProductId,
                ClientId = order.ClientId,
                PurchaseQuantity = order.PurchaseQuantity,
                OrderedDate = order.OrderedDate,
            };
        }

        public static (OrderDTO?, IEnumerable<OrderDTO>?) FromEntity(Order? order, IEnumerable<Order> orders)
        {
            // return single 
            if (order is not null || orders is null)
            {
                var singleOrder = new OrderDTO(
                    order!.Id,
                    order.ProductId,
                    order.ClientId,
                    order.PurchaseQuantity,
                    order.OrderedDate);

                return (singleOrder, null);
            }

            // return list 
            if(order is null || orders is not null)
            {
                var _orders = orders!.Select(o => new OrderDTO(
                    o!.Id,
                    o.ProductId,
                    o.ClientId,
                    o.PurchaseQuantity,
                    o.OrderedDate));

                return (null, _orders);
            }

            return (null, null);
        }
    }
}
