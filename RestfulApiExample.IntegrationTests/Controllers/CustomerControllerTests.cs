using Microsoft.AspNetCore.Builder;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using RestfulApiExample.Repository.Contracts;
using Moq;
using RestfulApiExample.Manager.Contracts;
using System.Net;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace RestfulApiExample.IntegrationTests.Controllers
{
    [TestFixture]
    public class CustomerControllerTests
    {
        private HttpClient httpClient = null!;
        private readonly Mock<ICustomerRepository> customerRepository = new();
        private readonly Mock<ICustomerManager> customerManager = new();

        public CustomerControllerTests()
        {
            var webAppFactory = new WebApplicationFactory<Program>()
               .WithWebHostBuilder(builder => {
                   builder.UseTestServer()
                       .ConfigureServices(services =>
                       {
                           //services.AddSingleton(this.customerRepository.Object);
                           //services.AddSingleton(this.customerManager.Object);
                       });
               });

            httpClient = webAppFactory.CreateDefaultClient();
        }

        #region Public

        [TestCase]
        public async Task GetAllCustomersAsync_HappyPath()
        {
            // Arrange
            string customerName = "Leire Natalie";

            var customer = await this.CreateCustomerAsync(customerName);
            
            // Act
            var response = await httpClient.GetAsync($"api/customers");
            response.EnsureSuccessStatusCode();

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var returnedJson = await response.Content.ReadAsStringAsync();

            var returnedCustomers = JsonConvert.DeserializeObject<List<Entities.DTO.CustomerResource>>(returnedJson);

            var returnedCustomer = returnedCustomers.FirstOrDefault(e => e.Id == customer.Id);

            Assert.IsNotNull(returnedCustomer);
            Assert.AreEqual(customerName, returnedCustomer.Name);

            await this.DeleteCustomerAsync(customer.Id);
        }

        [TestCase]
        public async Task GetCustomerByIdAsync_HappyPath()
        {
            // Arrange
            var customer = await this.CreateCustomerAsync("Jonh Doe");

            // Act
            var response = await httpClient.GetAsync($"api/customers/{customer.Id}");
            response.EnsureSuccessStatusCode();

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var returnedJson = await response.Content.ReadAsStringAsync();

            Console.WriteLine(returnedJson);
            var returnedCustomer = JsonConvert.DeserializeObject<Entities.DTO.CustomerResource>(returnedJson);

            Assert.AreEqual(customer.Id, returnedCustomer.Id);
            Assert.AreEqual(customer.Name, returnedCustomer.Name);

            await this.DeleteCustomerAsync(customer.Id);
        }

        [TestCase]
        public async Task GetCustomerByIdAsync_NotFound_404()
        {
            // Act
            var response = await httpClient.GetAsync($"api/customers/{Guid.NewGuid()}");

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestCase("Jonh Doe")]
        public async Task CreateCustomerAsync_HappyPath(string name)
        {
            // Arrange
            var request = new Entities.DTO.CreateCustomerRequest()
            {
                Name = name
            };

            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            var response = await httpClient.PostAsync($"api/customers", content);

            response.EnsureSuccessStatusCode();

            var result = JsonConvert.DeserializeObject<Entities.DTO.CustomerResource>(await response.Content.ReadAsStringAsync());

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(name, result.Name);
        }

        [TestCase("Jonh Doe")]
        public async Task DeleteCustomerAsync_HappyPath(string name)
        {
            // Arrange
            var customer = await this.CreateCustomerAsync("Jonh Doe");

            // Act
            var response = await httpClient.DeleteAsync($"api/customers/{customer.Id}");

            response.EnsureSuccessStatusCode();

            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        [TestCase("Jonh Doe")]
        public async Task DeleteCustomerAsync_NotFound_404(string name)
        {
            // Act
            var response = await httpClient.DeleteAsync($"api/customers/{Guid.NewGuid()}");

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        #endregion

        #region Private

        private async Task<Entities.DTO.CustomerResource> CreateCustomerAsync(string name)
        {
            var request = new Entities.DTO.CreateCustomerRequest()
            {
                Name = name
            };

            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            var response = await httpClient.PostAsync($"api/customers", content);

            response.EnsureSuccessStatusCode();

            var result = JsonConvert.DeserializeObject<Entities.DTO.CustomerResource>(await response.Content.ReadAsStringAsync());

            return result;
        }

        private async Task DeleteCustomerAsync(Guid id)
        {
            var response = await httpClient.DeleteAsync($"api/customers/{id}");

            response.EnsureSuccessStatusCode();
        }

        #endregion

    }
}
