using System;

namespace BillsDesktopApp.Dtos.OrdersDtos
{
    public class ShowOrderDTO
    {
        public int OrderID { get; set; }
        public string UserName { get; set; }
        public string CustomerName { get; set; }
        public string CompanyName { get; set; }
        public DateTime OrderDate { get; set; }
        public string Address { get; set; }

    }
}
