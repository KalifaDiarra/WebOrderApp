namespace WebOrderApp.Models
{
    // Ce modèle est utilisé pour la page de connexion
    public class LoginViewModel
    {
        // L'utilisateur va entrer son email pour se connecter
        public string Email { get; set; } = "";

        // L'utilisateur entre aussi son mot de passe
        public string Password { get; set; } = "";
    }
}

