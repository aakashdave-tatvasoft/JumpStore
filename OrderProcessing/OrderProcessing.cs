using Communication.Entities;
using Communication.Interfaces;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using System.Fabric;

namespace OrderProcessing
{
    internal sealed class OrderProcessing(StatelessServiceContext context) : StatelessService(context), IOrderProcessing
    {

        public Task<string> ProcessOrder(Order order)
        {
            // In a real implementation, this would handle payment processing,
            // inventory updates, etc. For this demo, we'll just return a simple message.
            string confirmation = $"Order {order.OrderId} for user {order.UserId} processed at {DateTime.UtcNow}";
            return Task.FromResult(confirmation);
        }

        public Task<string> GetServiceInfo()
        {
            string serviceInfo = $"Service: {Context.ServiceName}, " +
                                $"Instance: {Context.InstanceId}, " +
                                $"Node: {Context.NodeContext.NodeName}";
            return Task.FromResult(serviceInfo);
        }

        // Required for Service Fabric communication
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return this.CreateServiceRemotingInstanceListeners();
        }
    }
}
