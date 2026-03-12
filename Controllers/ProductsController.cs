using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebOrderApp.Data;
using WebOrderApp.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace WebOrderApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Constructeur qui récupère le contexte de la base de données
        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Vérifie si un utilisateur est connecté (sert à sécuriser les actions admin)
        private bool IsUserAuthenticated()
        {
            return HttpContext.Session.GetInt32("UserId") != null;
        }

        // Page qui affiche tous les produits (ouverte à tous les utilisateurs)
        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        // Formulaire pour créer un produit (accessible seulement si connecté)
        public IActionResult Create()
        {
            if (!IsUserAuthenticated())
                return RedirectToAction("Login", "Account");

            return View();
        }

        // Enregistre un nouveau produit dans la base (via le formulaire)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (!IsUserAuthenticated())
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // Affiche le formulaire de modification d’un produit existant
        public IActionResult Edit(int id)
        {
            if (!IsUserAuthenticated())
                return RedirectToAction("Login", "Account");

            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // Enregistre les modifications du produit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            if (!IsUserAuthenticated())
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                // Marque le produit comme modifié et le sauvegarde
                _context.Entry(product).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // Page de confirmation de suppression d’un produit
        public IActionResult Delete(int id)
        {
            if (!IsUserAuthenticated())
                return RedirectToAction("Login", "Account");

            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // Supprime le produit de la base après confirmation
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!IsUserAuthenticated())
                return RedirectToAction("Login", "Account");

            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
