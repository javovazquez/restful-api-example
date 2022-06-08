using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using RestfulApiExample.Common.Exceptions;

namespace RestfulApiExample.Repository
{
    public class CustomerRepository: Contracts.ICustomerRepository
    {
        private readonly ConcurrentDictionary<Guid, Entities.Customer> data;

        public CustomerRepository()
        {
            this.data = new ConcurrentDictionary<Guid, Entities.Customer>();
        }

        public async Task<List<Entities.Customer>> GetAllAsync(string? search = null, int pageSize = 50, int skip = 0)
        {
            if(pageSize <= 0)
            {
                throw Common.Errors.ArgumentOutOfRangeException(nameof(pageSize), $"Page size must be greater than 0");
            }

            if (skip < 0)
            {
                throw Common.Errors.ArgumentOutOfRangeException(nameof(skip), $"Skip must be greater than or equal to 0");
            }

            IEnumerable<Entities.Customer> query = this.data.Values;

            if (!String.IsNullOrEmpty(search))
            {
                query = query.Where(e => e.Name.Contains(search));
            }

            query = query.Skip(skip)
                .Take(pageSize);

            return await Task.FromResult(query.ToList());
        }

        public async Task<Entities.Customer> GetByIdAsync(Guid id)
        {
            var found = this.data.GetValueOrDefault(id);

            if(found == null)
            {
                throw Common.Errors.NotFoundException(nameof(Entities.Customer), id.ToString());
            }

            return await Task.FromResult(found);
        }

        public async Task<Entities.Customer> AddAsync(Entities.Customer customer)
        {
            if(customer == null)
            {
                throw Common.Errors.ArgumentNullException(nameof(customer));
            }

            customer.Id = Guid.NewGuid();

            this.data.TryAdd(customer.Id, customer);

            return await Task.FromResult(customer);
        }

        public async Task<Entities.Customer> UpdateAsync(Entities.Customer customer)
        {
            if(customer == null)
            {
                throw Common.Errors.ArgumentNullException(nameof(customer));
            }
            if(customer.Id == Guid.Empty)
            {
                throw Common.Errors.ArgumentNullException(nameof(customer.Id));
            }

            var comparisonCustomer = await this.GetByIdAsync(customer.Id);

            if(!this.data.TryUpdate(customer.Id, customer, comparisonCustomer))
            {
                throw Common.Errors.UpdateConflictException(nameof(Entities.Customer), customer.Id.ToString());
            }

            return customer;
        }

        public async Task<Entities.Customer> RemoveAsync(Guid customerId)
        {
            if(customerId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(customerId));
            }

            Entities.Customer customer;

            if(!this.data.Remove(customerId, out customer))
            {
                throw Common.Errors.NotFoundException(nameof(Entities.Customer), customerId.ToString());
            };

            return await Task.FromResult(customer);
        }
    }
}
