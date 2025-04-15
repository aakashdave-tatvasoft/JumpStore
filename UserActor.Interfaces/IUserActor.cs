using Communication.Entities;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Remoting.FabricTransport;
using Microsoft.ServiceFabric.Services.Remoting;

[assembly: FabricTransportActorRemotingProvider(RemotingListenerVersion = RemotingListenerVersion.V2_1, RemotingClientVersion = RemotingClientVersion.V2_1)]
namespace UserActor.Interfaces
{
    public interface IUserActor : IActor
    {
        Task AddToCart(CartItem item);
        Task<List<CartItem>> GetCart();
        Task ClearCart();
        Task UpdateUserProfile(UserProfile profile);
        Task<UserProfile> GetUserProfile();
    }
}
