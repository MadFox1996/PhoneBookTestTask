using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Data.EntityModel
{
    public class PhoneBookEntry
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        [NotMapped]
        [JsonIgnore]
        public string Password { get; set; }
        
        [JsonIgnore]
        public byte[] PasswordHash { get; set; }
        
        [JsonIgnore]
        public byte[] PasswordSalt { get; set; }

        public DateTime Birthday { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string MediumImageUrl { get; set; }
        
        [JsonIgnore]
        public byte[]? MediumImageData { get; set; }
    }
}
