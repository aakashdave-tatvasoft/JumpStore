using Communication.Entities;
using Microsoft.ServiceFabric.Services.Remoting;

namespace Communication.Interfaces;
public interface IProductCatalog : IService
{
    Task AddProduct(Product product);
    Task<Product?> GetProduct(int id);
    Task<List<Product>> GetAllProducts();
}
