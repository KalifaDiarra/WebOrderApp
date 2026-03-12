namespace WebOrderApp.Models
{
    // Ce modèle représente un produit dans notre boutique
    public class Product
    {
        public int Id { get; set; } // Identifiant unique du produit (clé primaire)

        public required string Name { get; set; } // Nom du produit (obligatoire)

        public string? Description { get; set; } // Description du produit (optionnelle)

        public decimal Price { get; set; } // Prix du produit
    }
}
