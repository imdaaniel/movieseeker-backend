using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MovieSeeker.API.Models.User
{
    public class UserSignUpRequestModel
    {
        [Required]
        [NotNull]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [NotNull]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [NotNull]
        [EmailAddress]
        [MaxLength(62)]
        public string Email { get; set; }

        [Required]
        [NotNull]
        [MaxLength(50)]
        public string Password { get; set; }

        [Required]
        [NotNull]
        [MaxLength(50)]
        [Compare(nameof(Password), ErrorMessage = "Passwords mismatch")]
        public string PasswordConfirmation { get; set; }
    }
}