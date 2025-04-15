using System.Runtime.Serialization;

namespace Communication.Entities
{
    [DataContract]
    public class UserProfile
    {
        [DataMember]
        public string? UserId { get; set; }

        [DataMember]
        public string? Name { get; set; }

        [DataMember]
        public string? Email { get; set; }

        [DataMember]
        public string? Address { get; set; }
    }
}
