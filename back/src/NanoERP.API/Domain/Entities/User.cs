using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NanoERP.API.Domain.Entities
{
    public class User : MasterData, IUser
    {
        [MaxLength(50)]
        [Required(ErrorMessage = "Name is required")]
        public override string Name { get; set; } = string.Empty;

        [MaxLength(50)]
        [Required(ErrorMessage = "Surname is required")]
        public string Surname { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Username is required")]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [JsonIgnore]
        public string Password { get; set; } = string.Empty;

        public User() { }
    }
}