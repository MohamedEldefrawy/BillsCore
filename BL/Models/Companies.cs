using System.Collections.Generic;

namespace BL.Models
{
    public class Companies
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string TaxNumber { get; set; }
        public string LogoImagePath { get; set; }
        public string SignutreImagePath { get; set; }
        public ICollection<Users> Users { get; set; }
    }
}
