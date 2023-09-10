using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } // ProductName
        public string PictureUrl { get; set; } // ProductPictureUrl
        public long Price { get; set; } // ProductPrice
        public int Quantity { get; set; } // Quantity

    }
}