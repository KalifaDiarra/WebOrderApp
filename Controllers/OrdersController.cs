using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebOrderApp.Data;
using WebOrderApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace WebOrderApp.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Page d’index des commandes
        // Si admin => il peut voir toutes les commandes
        // Sinon => l’utilisateur voit seulement les siennes
        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            bool isAdmin = HttpContext.Session.GetInt32("IsAdmin") == 1;

            IQueryable<Order> query = _context.Orders
                .Include(o => o.Product)
                .Include(o => o.User);

            if (!isAdmin)
            {
                if (userId == null)
                    return RedirectToAction("Login", "Account");

                // Filtrer pour ne montrer que les commandes de l’utilisateur connecté
                query = query.Where(o => o.UserId == userId.Value);
            }

            var orders = query.ToList();
            return View(orders);
        }

        // Affiche le formulaire pour créer une commande
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
                return RedirectToAction("Login", "Account");

            // Liste des produits pour le dropdown
            ViewBag.Products = new SelectList(_context.Products, "Id", "Name");
            return View();
        }

        // Gère la création de la commande
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Order order)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            // Vérifie que le produit existe
            if (!_context.Products.Any(p => p.Id == order.ProductId))
                ModelState.AddModelError("ProductId", "Le produit sélectionné n'existe pas.");

            if (ModelState.IsValid)
            {
                order.UserId = userId.Value;

                // Calcule le prix total selon le prix du produit et la quantité
                order.TotalPrice = _context.Products
                    .Where(p => p.Id == order.ProductId)
                    .Select(p => p.Price)
                    .FirstOrDefault() * order.Quantity;

                _context.Orders.Add(order);
                _context.SaveChanges();

                // Redirige vers la page de confirmation
                return RedirectToAction("Confirmation", new { id = order.Id });
            }

            ViewBag.Products = new SelectList(_context.Products, "Id", "Name");
            return View(order);
        }

        // Formulaire pour modifier une commande existante
        public IActionResult Edit(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null) return NotFound();

            ViewBag.Products = new SelectList(_context.Products, "Id", "Name", order.ProductId);
            return View(order);
        }

        // Enregistre les modifications
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Order formOrder)
        {
            var existingOrder = _context.Orders.FirstOrDefault(o => o.Id == formOrder.Id);
            if (existingOrder == null) return NotFound();

            // Vérifie que le produit existe
            if (!_context.Products.Any(p => p.Id == formOrder.ProductId))
                ModelState.AddModelError("ProductId", "Le produit sélectionné n'existe pas.");

            if (ModelState.IsValid)
            {
                // Met à jour les champs (sauf UserId)
                existingOrder.ProductId = formOrder.ProductId;
                existingOrder.Quantity = formOrder.Quantity;
                existingOrder.OrderDate = formOrder.OrderDate;

                // Recalcule le prix total
                existingOrder.TotalPrice = _context.Products
                    .Where(p => p.Id == formOrder.ProductId)
                    .Select(p => p.Price)
                    .FirstOrDefault() * formOrder.Quantity;

                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Products = new SelectList(_context.Products, "Id", "Name", formOrder.ProductId);
            return View(formOrder);
        }

        // Page pour confirmer la suppression
        public IActionResult Delete(int id)
        {
            var order = _context.Orders
                .Include(o => o.Product)
                .FirstOrDefault(o => o.Id == id);

            if (order == null) return NotFound();
            return View(order);
        }

        // Suppression de la commande (après confirmation)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var order = _context.Orders.Find(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        // Page de confirmation d’une commande
        public IActionResult Confirmation(int id)
        {
            var order = _context.Orders
                .Include(o => o.Product)
                .Where(o => o.Id == id)
                .Select(o => new OrderViewModel
                {
                    Id = o.Id,
                    ProductName = o.Product != null ? o.Product.Name : "(Produit inconnu)",
                    Quantity = o.Quantity,
                    TotalPrice = o.TotalPrice,
                    OrderDate = o.OrderDate
                })
                .FirstOrDefault();

            if (order == null)
                return NotFound();

            return View(order);
        }
    }
}
