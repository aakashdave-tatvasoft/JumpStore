using System.Runtime.Serialization;

namespace Communication.Entities
{
    [DataContract]
    public class ProductCatalogue
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string? Name { get; set; }

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public int StockQuantity { get; set; }
    }
}
