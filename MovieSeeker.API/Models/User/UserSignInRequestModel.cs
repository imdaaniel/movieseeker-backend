using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MovieSeeker.API.Models.User
{
    public class UserSignInRequestModel
    {
        [Required]
        [NotNull]
        [EmailAddress]
        [MaxLength(62)]
        public string Email { get; set; }

        [Required]
        [NotNull]
        [MaxLength(50)]
        public string Password { get; set; }
    }
}