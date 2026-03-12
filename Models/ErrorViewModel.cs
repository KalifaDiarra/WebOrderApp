namespace WebOrderApp.Models
{
    // Ce modèle est utilisé pour afficher les erreurs dans la vue Error.cshtml
    public class ErrorViewModel
    {
        // Contient l'ID de la requête, utile pour le debug
        public string? RequestId { get; set; }

        // Retourne vrai si l'ID de la requête existe (donc s'il faut l'afficher dans la page)
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}

