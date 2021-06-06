using System;
using System.Collections.Generic;

namespace BL.Models
{
    public class Orders
    {
        public int ID { get; set; }
        public int CustomerId { get; set; }
        public int UserId { get; set; }
        public string Address { get; set; }
        public DateTime Date { get; set; }
        public Customers Customer { get; set; }
        public Users User { get; set; }
        public ICollection<OrderDetails> OrderDetails { get; set; }

    }
}
