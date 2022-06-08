using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestfulApiExample.Entities.Utilities;
using RestfulApiExample.Manager.Contracts;
using RestfulApiExample.Repository.Contracts;

namespace RestfulApiExample.Controllers
{
    [Route("api/purchases")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        #region Dependencies

        private readonly IPurchaseRepository PurchaseRepository;
        private readonly IPurchaseManager PurchaseManager;
        private readonly LinkGenerator LinkGenerator;
        private readonly ILogger<PurchaseController> Logger;

        #endregion

        #region RouteNames

        public const string GetPurchasesRouteName = "GetPurchases";
        public const string GetPurchaseRouteName = "GetPurchase";
        public const string CreatePurchaseRouteName = "CreatePurchase";

        #endregion

        #region Constructor

        public PurchaseController(
            IPurchaseRepository purchaseRepository,
            IPurchaseManager purchaseManager,
            LinkGenerator linkGenerator,
            ILogger<PurchaseController> logger)
        {
            this.PurchaseRepository = purchaseRepository;
            this.PurchaseManager = purchaseManager;
            this.LinkGenerator = linkGenerator;
            this.Logger = logger;
        }

        #endregion

        #region Public

        [HttpGet]
        [Route("", Name = GetPurchasesRouteName)]
        public async Task<IActionResult> GetPurchasesAsync(Guid? customerId = null, int pageSize = 50, int skip = 0)
        {
            try
            {
                var purchases = await this.PurchaseRepository.GetAllAsync(customerId, pageSize, skip);
                var resources = this.MapToPurchaseResources(purchases);

                return Ok(resources);
            }
            catch (Common.Exceptions.NotFoundException notFound)
            {
                this.Logger.LogError(notFound, notFound.Message);
                return NotFound(new { message = notFound.Message });
            }
            catch (Exception error)
            {
                this.Logger.LogError(error, $"An error ocurred getting purchases");
                return StatusCode(503, new { message = "Service unavailable" });
            }
        }

        [HttpGet]
        [Route("{id}", Name = GetPurchaseRouteName)]
        public async Task<IActionResult> GetPurchaseAsync(Guid id)
        {
            try
            {
                var purchase = await this.PurchaseRepository.GetByIdAsync(id);
                var resource = this.MapToPurchaseResource(purchase);

                return Ok(resource);
            }
            catch (Common.Exceptions.NotFoundException notFound)
            {
                this.Logger.LogError(notFound, notFound.Message);
                return NotFound(new { message = notFound.Message });
            }
            catch (Exception error)
            {
                this.Logger.LogError(error, $"An error ocurred getting purchase {id}");
                return StatusCode(503, new { message = "Service unavailable" });
            }
        }

        [HttpPost]
        [Route("", Name = CreatePurchaseRouteName)]
        public async Task<IActionResult> CreatePurchase([FromBody] Entities.DTO.CreatePurchaseRequest request)
        {
            try
            {
                var purchase = await this.PurchaseManager.CreatePurchaseAsync(request);
                var resource = this.MapToPurchaseResource(purchase);

                return this.Ok(resource);
            }
            catch (Common.Exceptions.ValidationException error)
            {
                this.Logger.LogError(error, error.Message);
                return this.BadRequest(new { message = error.Message });
            }
            catch (Exception error)
            {
                this.Logger.LogError(error, $"An error ocurred creating purchase");
                return StatusCode(503, new { message = "Service unavailable" });
            }
        }


        #endregion

        #region Private

        private List<Entities.DTO.PurchaseResource> MapToPurchaseResources(List<Entities.Purchase> purchases)
        {
            List<Entities.DTO.PurchaseResource> resources = new List<Entities.DTO.PurchaseResource>();
            foreach (var purchase in purchases)
            {
                resources.Add(this.MapToPurchaseResource(purchase));
            }
            return resources;
        }

        private Entities.DTO.PurchaseResource MapToPurchaseResource(Entities.Purchase purchase)
        {
            return new Entities.DTO.PurchaseResource()
            {
                Id = purchase.Id,
                CustomerId = purchase.CustomerId,
                Cost = purchase.Cost,
                Discount = purchase.Discount,
                Links = this.CreatePurchaseLinks(purchase)
            };
        }

        private List<Link> CreatePurchaseLinks(Entities.Purchase purchase)
        {
            var links = new List<Link>()
            {
                new Link(
                    this.LinkGenerator.GetUriByName(
                        HttpContext,
                        GetPurchaseRouteName,
                        values: new { id = purchase.Id}),
                    "self",
                    HttpMethods.Get
                    ),
            };

            return links;
        }

        #endregion

    }
}
