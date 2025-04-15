using Communication.Entities;
using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    public interface IStatefulnterface: IService
    {
       Task<string> GetServiceDetails();
       Task<Product> GetProduct(int id);
       Task AddProduct(Product product);
       Task<Product> GetFromQueue();
       Task AddToQueue(Product product);
    }
}
