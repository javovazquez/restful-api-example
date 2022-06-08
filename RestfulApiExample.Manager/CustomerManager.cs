using RestfulApiExample.Manager.Contracts;
using RestfulApiExample.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulApiExample.Manager
{
    public class CustomerManager : ICustomerManager
    {
        private readonly ICustomerRepository CustomerRepository;

        public CustomerManager(ICustomerRepository customerRepository)
        {
            this.CustomerRepository = customerRepository;
        }

        public async Task<Entities.Customer> CreateCustomerAsync(Entities.DTO.CreateCustomerRequest request)
        {
            if (request == null)
            {
                throw Common.Errors.ValidationException("Request body is required");
            }
            if (String.IsNullOrEmpty(request.Name))
            {
                throw Common.Errors.ValidationException("Name is required");
            }

            var customer = new Entities.Customer()
            {
                Name = request.Name
            };

            return await this.CustomerRepository.AddAsync(customer);
        }

        public async Task<Entities.Customer> UpdateCustomerAsync(Guid id, Entities.DTO.UpdateCustomerRequest request)
        {
            if (request == null)
            {
                throw Common.Errors.ValidationException("Request body is required");
            }
            if(id == Guid.Empty)
            {
                throw Common.Errors.ValidationException("Customer Id is required");
            }
            if (String.IsNullOrEmpty(request.Name))
            {
                throw Common.Errors.ValidationException("Name is required");
            }

            var customer = new Entities.Customer()
            {
                Id = id,
                Name = request.Name
            };

            return await this.CustomerRepository.UpdateAsync(customer);
        }

        public async Task DeleteCustomerAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw Common.Errors.ValidationException("Customer Id is required");
            }

            await this.CustomerRepository.RemoveAsync(id);
        }


    }
}
