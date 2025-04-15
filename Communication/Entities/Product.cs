using System.Runtime.Serialization;

namespace Communication.Entities
{
    [DataContract]
    public class Product
    {
       [DataMember]
       public int Id { get; set; }

       [DataMember]
       public string? Name { get; set; }

       [DataMember]
       public string? Type { get; set; }

    }
}
