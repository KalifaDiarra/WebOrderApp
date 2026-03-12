namespace WebOrderApp.Models
{
    // ViewModel utilisé pour afficher une commande dans une vue (ex: page de confirmation)
    public class OrderViewModel
    {
        public int Id { get; set; } // ID de la commande

        public string? ProductName { get; set; } // Nom du produit (nullable au cas où le produit est supprimé)

        public int Quantity { get; set; } // Combien d'articles commandés

        public decimal TotalPrice { get; set; } // Prix total (quantité x prix du produit)

        public DateTime OrderDate { get; set; } // Date à laquelle la commande a été passée
    }
}
