using RestfulApiExample.Manager;
using RestfulApiExample.Manager.Contracts;
using RestfulApiExample.Repository;
using RestfulApiExample.Repository.Contracts;

namespace RestfulApiExample
{
    public static class ServicesExtension
    {

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            // Repositories
            services.AddSingleton<ICustomerRepository, CustomerRepository>();
            services.AddSingleton<IPurchaseRepository, PurchaseRepository>();

            // Managers
            services.AddTransient<ICustomerManager, CustomerManager>();
            services.AddTransient<IPurchaseManager, PurchaseManager>();
            services.AddTransient<ILoyaltyDiscountManager, LoyaltyDiscountManager>();

            return services;
        }
    }
}
