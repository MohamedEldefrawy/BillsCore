using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DAL
{
    public class ContextFactory : IDesignTimeDbContextFactory<BillsContext>
    {
        public ContextFactory()
        {


        }

        private IConfiguration Configuration => new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json")
             .Build();



        public BillsContext CreateDbContext(string[] args)
        {

            var builder = new DbContextOptionsBuilder<BillsContext>();
            builder.UseSqlServer(Configuration.GetConnectionString("MyConn"));

            return new BillsContext(builder.Options);
        }
    }
}
