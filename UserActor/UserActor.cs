using Communication.Entities;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using UserActor.Interfaces;

namespace UserActor
{
    [StatePersistence(StatePersistence.Persisted)]
    internal class UserActor(ActorService actorService, ActorId actorId) : Actor(actorService, actorId), IUserActor
    {
        protected override Task OnActivateAsync()
        {
            return base.OnActivateAsync();
        }

        // IUserActor implementation
        public async Task AddToCart(CartItem item)
        {
            var cart = await StateManager.GetOrAddStateAsync<List<CartItem>>("cart", []);

            // Check if item already exists in cart
            int existingItemIndex = cart.FindIndex(i => i.ProductId == item.ProductId);
            if (existingItemIndex >= 0)
            {
                cart[existingItemIndex].Quantity += item.Quantity;
            }
            else
            {
                cart.Add(item);
            }

            await StateManager.SetStateAsync("cart", cart);
        }

        public Task<List<CartItem>> GetCart()
        {
            return StateManager.GetOrAddStateAsync<List<CartItem>>("cart", []);
        }

        public Task ClearCart()
        {
            return StateManager.SetStateAsync("cart", new List<CartItem>());
        }

        public Task UpdateUserProfile(UserProfile profile)
        {
            return StateManager.SetStateAsync("profile", profile);
        }

        public Task<UserProfile> GetUserProfile()
        {
            return StateManager.GetOrAddStateAsync<UserProfile>("profile", null);
        }
    }
}
