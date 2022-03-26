using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NetCoreApp.Domain.Entities
{
    [Table("Users")]
    public class User : EntidadBase
    {
        [JsonIgnore]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        [JsonIgnore]
        public string PasswordHash { get; set; }
    }
}
