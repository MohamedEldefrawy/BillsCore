using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BL.Models
{
    public class Users
    {
        public int ID { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string UserName { get; set; }
        public string Password { get; set; }
        public int CompanyId { get; set; }
        public ICollection<Orders> Orders { get; set; }
        public Companies Company { get; set; }
    }
}
