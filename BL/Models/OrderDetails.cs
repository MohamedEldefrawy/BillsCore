using System.Collections.Generic;

namespace BL.Models
{
    public class OrderDetails
    {
        public int ID { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public Orders Order { get; set; }
        public Products Product { get; set; }
    }
}
