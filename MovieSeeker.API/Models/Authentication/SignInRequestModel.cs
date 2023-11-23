using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MovieSeeker.API.Models.Authentication
{
    public class SignInRequestModel
    {
        [Required(ErrorMessage = "O atributo Email é obrigatório")]
        [EmailAddress(ErrorMessage = "O atributo Email deve ser um e-mail num formato válido")]
        [MaxLength(50, ErrorMessage = "O atributo Email não pode ter mais de 50 caracteres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O atributo Password é obrigatório")]
        [MaxLength(50, ErrorMessage = "O atributo Password não pode ter mais de 62 caracteres")]
        public string Password { get; set; }
    }
}