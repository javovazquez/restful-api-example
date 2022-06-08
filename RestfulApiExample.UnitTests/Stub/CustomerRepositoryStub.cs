using RestfulApiExample.Entities;
using RestfulApiExample.Repository.Contracts;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulApiExample.UnitTests.Stub
{
    public class CustomerRepositoryStub : ICustomerRepository
    {
        private readonly ConcurrentDictionary<Guid, Entities.Customer> customers = new ConcurrentDictionary<Guid, Customer>();
        
        public Task<Customer> AddAsync(Customer customer)
        {
            customer.Id = Guid.NewGuid();
            this.customers[customer.Id] = customer;

            return Task.FromResult(customer);
        }

        public Task<List<Customer>> GetAllAsync(string? search = null, int pageSize = 50, int skip = 0)
        {
            // TODO: Not implemented
            return Task.FromResult(this.customers.Values.ToList());
        }

        public Task<Customer> GetByIdAsync(Guid id)
        {
            Entities.Customer customer; 
            if(!this.customers.TryGetValue(id, out customer))
            {
                throw Common.Errors.NotFoundException(nameof(Customer), id.ToString());
            }
            return Task.FromResult(customer);
        }

        public Task<Customer> RemoveAsync(Guid customerId)
        {
            Entities.Customer customer;
            this.customers.Remove(customerId, out customer);
            return Task.FromResult(customer);
        }

        public Task<Customer> UpdateAsync(Customer customer)
        {
            var comparisonCustomer = this.customers[customer.Id];
            this.customers.TryUpdate(customer.Id, customer, comparisonCustomer);
            return Task.FromResult(customer);
        }
    }
}
