using System.Runtime.Serialization;

namespace Communication.Entities
{
    [DataContract]
    public class Order
    {
        [DataMember]
        public string OrderId { get; set; } = Guid.NewGuid().ToString();

        [DataMember]
        public string? UserId { get; set; }

        [DataMember]
        public List<CartItem> Items { get; set; } = [];

        [DataMember]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    }

}
