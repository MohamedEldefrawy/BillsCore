using BL.Models;
using DAL.Repositories.Origin;

namespace DAL.Repositories.UsersRepository
{
    public class UsersRepository : Repository<Users>, IUsersRepository
    {
        private BillsContext _context;
        public UsersRepository(BillsContext context) : base(context)
        {
            _context = context;
        }
    }
}
