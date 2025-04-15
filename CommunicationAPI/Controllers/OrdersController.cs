using Communication.Entities;
using Communication.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace CommunicationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderProcessing _orderProcessing;

        public OrdersController()
        {
            _orderProcessing = ServiceProxy.Create<IOrderProcessing>(
                new Uri("fabric:/JumpStore/OrderProcessing"));
        }

        [HttpPost]
        public async Task<ActionResult<string>> PlaceOrder([FromBody] Order order)
        {
            var confirmationMessage = await _orderProcessing.ProcessOrder(order);
            return Ok(confirmationMessage);
        }

        [HttpGet("info")]
        public async Task<ActionResult<string>> GetServiceInfo()
        {
            return await _orderProcessing.GetServiceInfo();
        }
    }
}
