namespace WebOrderApp.Models
{
    // Cette classe représente un détail d'une commande (utile si un jour tu veux faire des commandes avec plusieurs produits)
    public class OrderDetail
    {
        public int Id { get; set; } // Identifiant unique du détail (clé primaire)

        public int OrderId { get; set; } // Référence à la commande principale (clé étrangère vers Order)

        public int ProductId { get; set; } // Produit concerné par ce détail (clé étrangère vers Product)

        public int Quantity { get; set; } // Quantité de ce produit dans la commande

        public decimal Price { get; set; } // Prix du produit au moment de la commande (utile si les prix changent)
    }
}

