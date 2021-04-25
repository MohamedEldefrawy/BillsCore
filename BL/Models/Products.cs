using System.Collections.Generic;

namespace BL.Models
{
    public class Products
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
