using DAL.Repositories.Origin;

namespace DAL.Repositories.OrdersRepository
{
    public class OrdersRepository : Repository<BL.Models.Orders>, IOrdersRepository
    {
        private BillsContext _context;
        public OrdersRepository(BillsContext context) : base(context)
        {
            _context = context;
        }
    }
}
