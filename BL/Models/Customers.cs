using System.Collections.Generic;

namespace BL.Models
{
    public class Customers
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public ICollection<Orders> Orders { get; set; }
    }
}
