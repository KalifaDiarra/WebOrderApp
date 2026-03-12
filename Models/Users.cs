namespace WebOrderApp.Models
{
    public class User
    {
        // Nombre d'essais de connexion échoués
        public int FailedLoginAttempts { get; set; } = 0;

        // Si le compte est bloqué ou non
        public bool IsLocked { get; set; } = false;

        // Identifiant unique de l'utilisateur
        public int Id { get; set; }

        // Prénom requis
        public required string FirstName { get; set; }

        // Nom requis
        public required string LastName { get; set; }

        // Email requis
        public required string Email { get; set; }

        // Hash du mot de passe (pas le mot de passe en clair)
        public required string PasswordHash { get; set; }

        // Date de naissance
        public DateTime BirthDate { get; set; }

        // Téléphone requis
        public required string Phone { get; set; }

        // Adresse requise
        public required string Address { get; set; }

        // Indique si l'utilisateur est admin ou non
        public bool IsAdmin { get; set; } = false;
    }
}
