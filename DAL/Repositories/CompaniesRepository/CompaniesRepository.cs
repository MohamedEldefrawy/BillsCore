using BL.Models;
using DAL.Repositories.Origin;

namespace DAL.Repositories.CompaniesRepository
{
    public class CompaniesRepository : Repository<Companies>, ICompaniesRepository
    {
        private readonly BillsContext _context;

        public CompaniesRepository(BillsContext context) : base(context)
        {
            _context = context;
        }
    }
}
