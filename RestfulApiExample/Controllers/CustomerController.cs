using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestfulApiExample.Entities.Utilities;
using RestfulApiExample.Manager.Contracts;
using RestfulApiExample.Repository.Contracts;

namespace RestfulApiExample.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository CustomerRepository;
        private readonly ICustomerManager CustomerManager;
        private readonly LinkGenerator LinkGenerator;
        private readonly ILogger<CustomerController> Logger;
        
        public CustomerController(
            ICustomerRepository customerRepository, 
            ICustomerManager customerManager, 
            LinkGenerator linkGenerator, 
            ILogger<CustomerController> logger)
        {
            this.CustomerRepository = customerRepository;
            this.CustomerManager = customerManager;
            this.LinkGenerator = linkGenerator;
            this.Logger = logger;
        }

        #region Public

        [HttpGet]
        [Route("", Name = "GetCustomers")]
        public async Task<IActionResult> GetAllCustomersAsync(string? search = null, int pageSize = 50, int skip = 50)
        {
            try
            {
                var customers = await this.CustomerRepository.GetAllAsync(search, pageSize, skip);

                var resources = this.MapToCustomerResources(customers);

                return Ok(resources);
            }
            catch (Exception error)
            {
                this.Logger.LogError(error, "An error ocurred searching customers");
                return StatusCode(503, new { message = "Service unavailable" });
            }
        }

        [HttpGet]
        [Route("{id}", Name = "GetCustomer")]
        public async Task<IActionResult> GetCustomerByIdAsync([FromRoute]Guid id)
        {
            try
            {
                var customer = await this.CustomerRepository.GetByIdAsync(id);

                var resource = this.MapToCustomerResource(customer);

                return Ok(resource);
            }
            catch(Common.Exceptions.NotFoundException notFound)
            {
                this.Logger.LogError(notFound, notFound.Message);
                return NotFound(new { message = notFound.Message });
            }
            catch(Exception error)
            {
                this.Logger.LogError(error, $"An error ocurred getting customer {id}");
                return StatusCode(503, new { message = "Service unavailable" });
            }
        }

        [HttpPost]
        [Route("", Name = "CreateCustomer")]
        public async Task<IActionResult> CreateCustomerAsync([FromBody]Entities.DTO.CreateCustomerRequest request)
        {
            try
            {
                var customer = await this.CustomerManager.CreateCustomerAsync(request);
                var resource = this.MapToCustomerResource(customer);

                return this.Ok(resource);
            }
            catch(Common.Exceptions.ValidationException error)
            {
                this.Logger.LogError(error, error.Message);
                return this.BadRequest(new { message = error.Message });
            }
            catch(Exception error)
            {
                this.Logger.LogError(error, $"An error ocurred creating customer");
                return StatusCode(503, new { message = "Service unavailable" });
            }
        }

        [HttpPut]
        [Route("{id}", Name = "UpdateCustomer")]
        public async Task<IActionResult> UpdateCustomerAsync([FromRoute]Guid id, [FromBody]Entities.DTO.UpdateCustomerRequest request)
        {
            try
            {
                var customer = await this.CustomerManager.UpdateCustomerAsync(id, request);
                var resource = this.MapToCustomerResource(customer);

                return this.Ok(resource);
            }
            catch (Common.Exceptions.NotFoundException notFound)
            {
                this.Logger.LogError(notFound, notFound.Message);
                return NotFound(notFound.Message);
            }
            catch (Exception error)
            {
                this.Logger.LogError(error, $"An error ocurred updating customer {id}");
                return StatusCode(503, "Service unavailable");
            }
        }

        [HttpDelete]
        [Route("{id}", Name = "DeleteCustomer")]
        public async Task<IActionResult> DeleteCustomerAsync([FromRoute]Guid id)
        {
            try
            {
                await this.CustomerManager.DeleteCustomerAsync(id);
                return NoContent();
            }
            catch (Common.Exceptions.NotFoundException notFound)
            {
                this.Logger.LogError(notFound, notFound.Message);
                return NotFound(new { message = notFound.Message });
            }
            catch (Exception error)
            {
                this.Logger.LogError(error, $"An error ocurred deleting customer {id}");
                return StatusCode(503, new { message = "Service unavailable" });
            }
        }

        #endregion

        #region Private

        private List<Entities.DTO.CustomerResource> MapToCustomerResources(List<Entities.Customer> customers)
        {
            List<Entities.DTO.CustomerResource> resources = new List<Entities.DTO.CustomerResource>();
            foreach (var customer in customers)
            {
                resources.Add(this.MapToCustomerResource(customer));
            }
            return resources;
        }

        private Entities.DTO.CustomerResource MapToCustomerResource(Entities.Customer customer)
        {
            return new Entities.DTO.CustomerResource()
            {
                Id = customer.Id,
                Name = customer.Name,
                Links = this.CreateCustomerLinks(customer)
            };
        }

        private List<Link> CreateCustomerLinks(Entities.Customer customer)
        {
            var links = new List<Link>()
            {
                new Link(
                    this.LinkGenerator.GetUriByName(
                        HttpContext,
                        "GetCustomer",
                        values: new { id = customer.Id}),
                    "self",
                    HttpMethods.Get
                    ),
                new Link(
                    this.LinkGenerator.GetUriByName(
                        HttpContext,
                        "UpdateCustomer",
                        values: new { id = customer.Id}),
                    "update_customer",
                    HttpMethods.Put
                    ),
                new Link(
                    this.LinkGenerator.GetUriByName(
                        HttpContext,
                        "DeleteCustomer",
                        values: new { id = customer.Id}),
                    "delete_customer",
                    HttpMethods.Delete
                    ),
            };

            return links;
        }

        #endregion

    }
}
