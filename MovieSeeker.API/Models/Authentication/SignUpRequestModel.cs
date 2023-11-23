using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MovieSeeker.API.Models.Authentication
{
    public class SignUpRequestModel
    {
        [Required(ErrorMessage = "O atributo FirstName é obrigatório")]
        [MaxLength(50, ErrorMessage = "O FirstName x não pode ter mais de 50 caracteres")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "O atributo LastName é obrigatório")]
        [MaxLength(50, ErrorMessage = "O atributo LastName não pode ter mais de 50 caracteres")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "O atributo Email é obrigatório")]
        [EmailAddress(ErrorMessage = "O atributo Email deve ser um e-mail num formato válido")]
        [MaxLength(62, ErrorMessage = "O atributo Email não pode ter mais de 62 caracteres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O atributo Password é obrigatório")]
        [MinLength(8, ErrorMessage = "O atributo Password não pode ter menos de 8 caracteres")]
        [MaxLength(50, ErrorMessage = "O atributo Password não pode ter mais de 50 caracteres")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,50}$", ErrorMessage = "A senha deve ter no mínimo 8 dígitos, uma letra maiúscula, uma letra minúscula, um número e um caractere especial")]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "As senhas não conferem")]
        public string PasswordConfirmation { get; set; }
    }
}