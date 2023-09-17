using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class UpdateProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Range(100, Double.PositiveInfinity)]
        public long Price { get; set; }
        public IFormFile File { get; set; }
        public string Type { get; set; }
        public string Brand { get; set; }
        [Range(0, int.MaxValue)]
        public int QuantityInStock { get; set; }
    }
}