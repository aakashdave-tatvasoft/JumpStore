using Communication.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using UserActor.Interfaces;

namespace CommunicationAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        [HttpGet("{userId}/cart")]
        public async Task<ActionResult<List<CartItem>>> GetCart(string userId)
        {
            var userActor = GetUserActorProxy(userId);
            return await userActor.GetCart();
        }

        [HttpPost("{userId}/cart")]
        public async Task<ActionResult> AddToCart(string userId, [FromBody] CartItem item)
        {
            var userActor = GetUserActorProxy(userId);
            await userActor.AddToCart(item);
            return Ok();
        }

        [HttpDelete("{userId}/cart")]
        public async Task<ActionResult> ClearCart(string userId)
        {
            var userActor = GetUserActorProxy(userId);
            await userActor.ClearCart();
            return Ok();
        }

        [HttpGet("{userId}/profile")]
        public async Task<ActionResult<UserProfile>> GetProfile(string userId)
        {
            var userActor = GetUserActorProxy(userId);
            var profile = await userActor.GetUserProfile();
            if (profile == null)
                return NotFound();

            return profile;
        }

        [HttpPut("{userId}/profile")]
        public async Task<ActionResult> UpdateProfile(string userId, [FromBody] UserProfile profile)
        {
            profile.UserId = userId; // Ensure consistency
            var userActor = GetUserActorProxy(userId);
            await userActor.UpdateUserProfile(profile);
            return Ok();
        }

        private static IUserActor GetUserActorProxy(string userId)
        {
            return ActorProxy.Create<IUserActor>(
                new ActorId(userId),
                new Uri("fabric:/JumpStore/UserActor"));
        }
    }

}
