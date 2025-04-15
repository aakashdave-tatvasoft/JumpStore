using System.Runtime.Serialization;

namespace Communication.Entities
{
    [DataContract]
    public class CartItem
    {
        [DataMember]
        public int ProductId { get; set; }

        [DataMember]
        public int Quantity { get; set; }
    }
}
