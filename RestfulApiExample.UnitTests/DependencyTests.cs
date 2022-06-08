using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestfulApiExample;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace RestfulApiExample.UnitTests
{
    [TestFixture]
    public class DependencyTests
    {
        [TestCase]
        public void AllDependenciesRegistered()
        {
            var builder = WebApplication.CreateBuilder();

            builder.Services.AddServices();

            var webApp = builder.Build();

            var services = webApp.Services;

            // Repositories
            services.GetRequiredService<RestfulApiExample.Repository.Contracts.ICustomerRepository>();
            services.GetRequiredService<RestfulApiExample.Repository.Contracts.IPurchaseRepository>();

            // Managers
            services.GetRequiredService<RestfulApiExample.Manager.Contracts.ICustomerManager>();
            services.GetRequiredService<RestfulApiExample.Manager.Contracts.ILoyaltyDiscountManager>();
            services.GetRequiredService<RestfulApiExample.Manager.Contracts.IPurchaseManager>();
        }
    }
}
