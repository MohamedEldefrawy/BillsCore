using BL.Models;
using DAL.Repositories.Origin;

namespace DAL.Repositories.OrderDetailsRepository
{
    public class OrderDetailsRepository : Repository<OrderDetails>, IOrderDetailsRepository
    {
        private BillsContext _context;
        public OrderDetailsRepository(BillsContext context) : base(context)
        {
            _context = context;
        }
    }
}
