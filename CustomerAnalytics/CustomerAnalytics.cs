using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Communication;

namespace CustomerAnalytics
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class CustomerAnalytics : StatelessService, IStatelessInterface
    {
        public CustomerAnalytics(StatelessServiceContext context) : base(context)
        {
        }
        public async Task<string> GetServiceDetails()
        {
            string serviceName = this.Context.ServiceName.ToString();
            string partitionId = this.Context.PartitionId.ToString();
            long instanceId = this.Context.InstanceId;

            return $"ServiceName: {serviceName}, PartitionId: {partitionId}, InstanceId: {instanceId}";
        }

        /// <summary>
        /// Creates listeners for handling client requests using Service Remoting.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return this.CreateServiceRemotingInstanceListeners();
        }

        /// <summary>
        /// Main entry point for the service instance.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            try
            {
                long iterations = 0;

                while (true)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    ServiceEventSource.Current.ServiceMessage(this.Context, "AnalyticsService-Working-{0}", ++iterations);

                    // Simulate some analytics processing
                    await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                // Graceful shutdown
                ServiceEventSource.Current.ServiceMessage(this.Context, "Service shutdown requested.");
                throw;
            }
            catch (Exception ex)
            {
                ServiceEventSource.Current.ServiceMessage(this.Context, $"Error in RunAsync: {ex.Message}");
                throw;
            }
        }
    }
}