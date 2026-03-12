using System.ComponentModel.DataAnnotations;

namespace WebOrderApp.Models
{
    public class RegisterViewModel
    {
        // Prénom requis
        [Required]
        public string FirstName { get; set; } = string.Empty;

        // Nom requis
        [Required]
        public string LastName { get; set; } = string.Empty;

        // Email requis + doit être un vrai format email
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        // Mot de passe requis avec validation de sécurité :
        // - au moins 8 caractères
        // - au moins une majuscule, une minuscule, un chiffre et un symbole
        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
            ErrorMessage = "Le mot de passe doit contenir au moins 8 caractères, une majuscule, une minuscule, un chiffre et un symbole.")]
        public string Password { get; set; } = string.Empty;

        // Confirmation du mot de passe, doit être identique à Password
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Les mots de passe ne correspondent pas.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
