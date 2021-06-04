using DAL.Repositories.CompaniesRepository;
using DAL.Repositories.CustomersRepository;
using DAL.Repositories.OrderDetailsRepository;
using DAL.Repositories.OrdersRepository;
using DAL.Repositories.Origin;
using DAL.Repositories.ProductsRepository;
using DAL.Repositories.UsersRepository;
using DAL.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

namespace DAL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependency(this IServiceCollection services)
        {
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<ICompaniesRepository, CompaniesRepository>();
            services.AddTransient<ICustomersRepository, CustomersRepository>();
            services.AddTransient<IOrderDetailsRepository, OrderDetailsRepository>();
            services.AddTransient<IOrdersRepository, OrdersRepository>();
            services.AddTransient<IProductsRepository, ProductsRepository>();
            services.AddTransient<IUsersRepository, UsersRepository>();
            services.AddTransient<IUnitOfWork, DAL.UnitOfWork.UnitOfWork>();
            return services;
        }
    }
}
