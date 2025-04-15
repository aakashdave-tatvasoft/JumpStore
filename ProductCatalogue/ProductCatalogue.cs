using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Communication;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Communication.Entities;
using Microsoft.ServiceFabric.Data;

namespace ProductCatalogue
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class ProductCatalogue(StatefulServiceContext context) : StatefulService(context), IStatefulnterface
    {

        public async Task<Product> GetFromQueue()
        {
            IReliableStateManager stateManager = this.StateManager;
            var productQueue = await stateManager.GetOrAddAsync<IReliableQueue<Product>>("productQueue");
            using var tx = stateManager.CreateTransaction();
            var product = await productQueue.TryDequeueAsync(tx);
            return product.Value;
        }

        public async Task AddToQueue(Product product)
        {
            IReliableStateManager stateManager = this.StateManager;
            var productQueue = await stateManager.GetOrAddAsync<IReliableQueue<Product>>("productQueue");
            using var tx = stateManager.CreateTransaction();
            await productQueue.EnqueueAsync(tx, product);
            await tx.CommitAsync();
        }

        public async Task AddProduct(Product product)
        {
            IReliableStateManager stateManager = this.StateManager;
            IReliableDictionary<int, Product> productDict = await stateManager.GetOrAddAsync<IReliableDictionary<int, Product>>("productDict");
            using var tx = stateManager.CreateTransaction();
            await productDict.AddOrUpdateAsync(tx, product.Id, product, (k,v) => product);
            await tx.CommitAsync();
        }

        public async Task<Product> GetProduct(int id)
        {
            IReliableStateManager stateManager = this.StateManager;
            IReliableDictionary<int, Product> productDict = await stateManager.GetOrAddAsync<IReliableDictionary<int, Product>>("productDict");
            using var tx = stateManager.CreateTransaction();
            var product = await productDict.TryGetValueAsync(tx, id);
            return product.Value;
        }

        public async Task<string> GetServiceDetails()
        {
            string serviceName = this.Context.ServiceName.ToString();
            string partitionId = this.Context.PartitionId.ToString();
            long replicaID = this.Context.ReplicaId;

            return $"ServiceName: {serviceName}, PartitionId: {partitionId}, ReplicaID: {replicaID}";
        }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return this.CreateServiceRemotingReplicaListeners();
        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following sample code with your own logic 
            //       or remove this RunAsync override if it's not needed in your service.

            var myDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, long>>("myDictionary");

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                using (var tx = this.StateManager.CreateTransaction())
                {
                    var result = await myDictionary.TryGetValueAsync(tx, "Counter");

                    ServiceEventSource.Current.ServiceMessage(this.Context, "Current Counter Value: {0}",
                        result.HasValue ? result.Value.ToString() : "Value does not exist.");

                    await myDictionary.AddOrUpdateAsync(tx, "Counter", 0, (key, value) => ++value);

                    // If an exception is thrown before calling CommitAsync, the transaction aborts, all changes are 
                    // discarded, and nothing is saved to the secondary replicas.
                    await tx.CommitAsync();
                }

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }
    }
}
