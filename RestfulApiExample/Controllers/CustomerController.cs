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
        #region Dependencies

        private readonly ICustomerRepository CustomerRepository;
        private readonly ICustomerManager CustomerManager;
        private readonly LinkGenerator LinkGenerator;
        private readonly ILogger<CustomerController> Logger;

        #endregion

        #region RouteNames

        public const string GetCustomersRouteName = "GetCustomers";
        public const string GetCustomerRouteName = "GetCustomer";
        public const string CreateCustomerRouteName = "CreateCustomer";
        public const string UpdateCustomerRouteName = "UpdateCustomer";
        public const string DeleteCustomerRouteName = "DeleteCustomer";
        public const string GetPurchasesByCustomerRouteName = "GetPurchasesByCustomer";

        #endregion

        #region Constructor

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

        #endregion

        #region Public

        [HttpGet]
        [Route("", Name = GetCustomersRouteName)]
        public async Task<IActionResult> GetAllCustomersAsync(string? search = null, int pageSize = 50, int skip = 0)
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
        [Route("{id}", Name = GetCustomerRouteName)]
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
        [Route("", Name = CreateCustomerRouteName)]
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
        [Route("{id}", Name = UpdateCustomerRouteName)]
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
                return StatusCode(503, new { message = "Service unavailable" });
            }
        }

        [HttpDelete]
        [Route("{id}", Name = DeleteCustomerRouteName)]
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

        [HttpGet]
        [Route("{id}/purchases", Name = GetPurchasesByCustomerRouteName)]
        public IActionResult GetPurchases([FromRoute] Guid id, int pageSize = 50, int skip = 0)
        {
            try
            {
                return this.RedirectToRoute(PurchaseController.GetPurchasesRouteName, new { customerId = id, pageSize = pageSize, skip = skip });
            }
            catch (Common.Exceptions.NotFoundException notFound)
            {
                this.Logger.LogError(notFound, notFound.Message);
                return NotFound(new { message = notFound.Message });
            }
            catch (Exception error)
            {
                this.Logger.LogError(error, $"An error ocurred getting purchases by customer {id}");
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
                        GetCustomerRouteName,
                        values: new { id = customer.Id}),
                    "self",
                    HttpMethods.Get
                    ),
                new Link(
                    this.LinkGenerator.GetUriByName(
                        HttpContext,
                        UpdateCustomerRouteName,
                        values: new { id = customer.Id}),
                    "update_customer",
                    HttpMethods.Put
                    ),
                new Link(
                    this.LinkGenerator.GetUriByName(
                        HttpContext,
                        DeleteCustomerRouteName,
                        values: new { id = customer.Id}),
                    "delete_customer",
                    HttpMethods.Delete
                    ),
                new Link(
                    this.LinkGenerator.GetUriByName(
                        HttpContext,
                        GetPurchasesByCustomerRouteName,
                        values: new { id = customer.Id}),
                    "get_customer_purchases",
                    HttpMethods.Get
                    ),
            };

            return links;
        }

        #endregion

    }
}
