using Communication.Entities;
using Communication.Interfaces;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using System.Fabric;

namespace ProductService
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class ProductService(StatefulServiceContext context) : StatefulService(context), IProductCatalog
    {
        public async Task AddProduct(Product product)
        {
            var productsDict = await StateManager.GetOrAddAsync<IReliableDictionary<int, Product>>("products");

            using var tx = StateManager.CreateTransaction();
            await productsDict.AddOrUpdateAsync(tx, product.Id, product, (_, __) => product);
            await tx.CommitAsync();
        }

        public async Task<Product?> GetProduct(int id)
        {
            var productsDict = await StateManager.GetOrAddAsync<IReliableDictionary<int, Product>>("products");

            using var tx = StateManager.CreateTransaction();
            var result = await productsDict.TryGetValueAsync(tx, id);

            return result.HasValue ? result.Value : null;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            var productsDict = await StateManager.GetOrAddAsync<IReliableDictionary<int, Product>>("products");
            var products = new List<Product>();

            using var tx = StateManager.CreateTransaction();
            var enumerable = await productsDict.CreateEnumerableAsync(tx);

            using var enumerator = enumerable.GetAsyncEnumerator();
            while (await enumerator.MoveNextAsync(default))
            {
                products.Add(enumerator.Current.Value);
            }

            return products;
        }

        // Required for Service Fabric communication
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return this.CreateServiceRemotingReplicaListeners();
        }

    }
}
