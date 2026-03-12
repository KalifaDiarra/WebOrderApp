using Microsoft.EntityFrameworkCore;
using WebOrderApp.Models;

namespace WebOrderApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        // Constructeur utilisé par l'injection de dépendance (ex: dans Program.cs)
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Constructeur par défaut (utilisé surtout pour les tests ou configuration manuelle)
        public ApplicationDbContext() { }

        // DbSet = chaque table de la base de données
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        // Méthode appelée si la config n'est pas déjà faite (utile pour tests ou projets simples)
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Connexion MySQL locale à la base weborderdb
                optionsBuilder.UseMySql(
                    "server=localhost;port=3306;database=weborderdb;user=root;password=AnnaDiarra2004;",
                    new MySqlServerVersion(new Version(8, 0, 21))
                );
            }
        }
    }
}
