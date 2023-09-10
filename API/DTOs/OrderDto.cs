using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities.OrderAggregate;
/*
    Ce code définit une classe OrderDto. DTO signifie "Data Transfer Object" (Objet de transfert de données). Un DTO est souvent utilisé pour regrouper des données et les transférer entre des couches/logiques différentes dans une application, par exemple entre une logique métier et une interface utilisateur, ou entre un serveur et un client dans le cas d'une API.
*/
namespace API.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string BuyerId { get; set; }
        public ShippingAddress ShippingAddress { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
        public long Subtotal { get; set; }
        public long DeliveryFee { get; set; }
        public string OrderStatus { get; set; }
        public long Total { get; set; }
    }
}