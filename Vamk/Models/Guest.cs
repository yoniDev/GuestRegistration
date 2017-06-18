using Newtonsoft.Json;

namespace Vamk.Models
{

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Guest
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "fname")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "lname")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "age")]
        public string Age { get; set; }

        [JsonProperty(PropertyName = "sex")]
        public string Sex { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "image")]
        public byte[] Image { get; set; }

    }
}