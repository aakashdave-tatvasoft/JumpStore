using Communication;
using Communication.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace CommunicationAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommunicationController : ControllerBase
    {
        [HttpGet]
        [Route("stateless")]
        public async Task<string> StatelessGet()
        {
            var statelessProxy = ServiceProxy.Create<IStatelessInterface>(
                    new Uri("fabric:/JumpStore/CustomerAnalytics")
                );

            string serviceName = await statelessProxy.GetServiceDetails();
            return serviceName;
        }

        [HttpGet]
        [Route("stateful")]
        public async Task<string> StatefulGet(
            [FromQuery] string region)
        {
            var statefulProxy = ServiceProxy.Create<IStatefulnterface>(
                    new Uri("fabric:/JumpStore/ProductCatalogue"),
                    new Microsoft.ServiceFabric.Services.Client.ServicePartitionKey(region.ToUpperInvariant()));
            var serviceName = await statefulProxy.GetServiceDetails();
            return serviceName;
        }

        [HttpPost]
        [Route("product")]
        public async Task AddProduct(Product product)
        {
            var partitionId = product.Id % 3;
            var statefulProxy = ServiceProxy.Create<IStatefulnterface>(
                    new Uri("fabric:/JumpStore/ProductCatalogue"),
                    new Microsoft.ServiceFabric.Services.Client.ServicePartitionKey(partitionId));
            await statefulProxy.AddProduct(product);
        }

        [HttpGet]
        [Route("product")]
        public async Task<Product> GetProduct(int id)
        {
            var partitionId = id % 3;
            var statefulProxy = ServiceProxy.Create<IStatefulnterface>(
                    new Uri("fabric:/JumpStore/ProductCatalogue"),
                    new Microsoft.ServiceFabric.Services.Client.ServicePartitionKey(partitionId));
            Product product = await statefulProxy.GetProduct(id);
            return product;
        }

        [HttpGet]
        [Route("productQueue")]
        public async Task<Product> GetFromQueue(int partitionId)
        {
            var statefulProxy = ServiceProxy.Create<IStatefulnterface>(
                    new Uri("fabric:/JumpStore/ProductCatalogue"),
                    new Microsoft.ServiceFabric.Services.Client.ServicePartitionKey(partitionId));
            Product product = await statefulProxy.GetFromQueue();
            return product;
        }

        [HttpPost]
        [Route("productQueue")]
        public async Task AddToQueue(
                [FromQuery] int partitionId,
                [FromBody] Product product
            )
        {
            var statefulProxy = ServiceProxy.Create<IStatefulnterface>(
                    new Uri("fabric:/JumpStore/ProductCatalogue"),
                    new Microsoft.ServiceFabric.Services.Client.ServicePartitionKey(partitionId));
            await statefulProxy.AddToQueue(product);
        }
    }
}
