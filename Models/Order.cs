using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebOrderApp.Models
{
    // Classe qui représente une commande passée dans l'application
    public class Order
    {
        public int Id { get; set; } // Clé primaire (générée automatiquement)

        [Required]
        public int UserId { get; set; }  // L'utilisateur qui a passé la commande (clé étrangère)

        [Required]
        public int ProductId { get; set; }  // Le produit commandé (clé étrangère)

        [Required]
        public int Quantity { get; set; } // Combien d'unités du produit sont commandées

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now; // Date et heure de la commande (par défaut : maintenant)

        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(10,2)")] // Format précis pour les prix dans la base de données
        public decimal TotalPrice { get; set; } // Prix total = quantité * prix du produit

        // Propriétés de navigation pour accéder facilement à l'objet User et Product lié
        public User? User { get; set; } // Permet d'accéder aux infos de l'utilisateur lié à cette commande
        public Product? Product { get; set; } // Permet d'accéder aux infos du produit commandé
    }
}
