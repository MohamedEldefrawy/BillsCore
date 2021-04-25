using System.Collections.Generic;

namespace BL.Models
{
    public class Companies
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<Orders> Orders { get; set; }
    }
}
