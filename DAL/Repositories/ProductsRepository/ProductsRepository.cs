using BL.Models;
using DAL.Repositories.Origin;

namespace DAL.Repositories.ProductsRepository
{
    public class ProductsRepository : Repository<Products>, IProductsRepository
    {
        private BillsContext _context;

        public ProductsRepository(BillsContext context) : base(context)
        {
            _context = context;
        }

    }
}
