using DAL.Repositories.Origin;

namespace DAL.Repositories.CustomersRepository
{
    public class CustomersRepository : Repository<BL.Models.Customers>, ICustomersRepository
    {
        private BillsContext _context;
        public CustomersRepository(BillsContext context) : base(context)
        {
            _context = context;
        }
    }
}
