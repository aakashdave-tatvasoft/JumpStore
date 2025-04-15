using Communication.Entities;
using Microsoft.ServiceFabric.Services.Remoting;

namespace Communication.Interfaces;

public interface IOrderProcessing : IService
{
    Task<string> ProcessOrder(Order order);
    Task<string> GetServiceInfo();
}


