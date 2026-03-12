using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using WebOrderApp.Models;
using WebOrderApp.Data;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace WebOrderApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Affiche la page d'inscription
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Gère la soumission du formulaire d'inscription
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            // On vérifie que tous les champs sont valides
            if (ModelState.IsValid)
            {
                // Si c'est le premier utilisateur, on le rend admin automatiquement
                bool isFirstUser = !_context.Users.Any();

                // Création d'un nouvel utilisateur avec les infos entrées
                var user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PasswordHash = HashPassword(model.Password),
                    Phone = "",
                    Address = "",
                    IsAdmin = isFirstUser,
                    FailedLoginAttempts = 0,
                    IsLocked = false
                };

                // On l'ajoute à la base de données
                _context.Users.Add(user);
                _context.SaveChanges();

                // On connecte automatiquement l'utilisateur
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("UserName", user.FirstName);
                HttpContext.Session.SetInt32("IsAdmin", user.IsAdmin ? 1 : 0);

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        // Affiche la page de connexion
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Gère la soumission du formulaire de connexion
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // On essaie de trouver un utilisateur avec l'email donné
            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);

            // Si aucun utilisateur ou mauvais mot de passe
            if (user == null || user.PasswordHash != HashPassword(model.Password))
            {
                // Si l'utilisateur existe, on incrémente le nombre d'échecs
                if (user != null)
                {
                    user.FailedLoginAttempts++;

                    // Si 3 tentatives échouées, on bloque le compte
                    if (user.FailedLoginAttempts >= 3)
                    {
                        user.IsLocked = true;
                        TempData["Error"] = "Votre compte est bloqué après 3 tentatives échouées.";
                    }

                    _context.SaveChanges();
                }

                ModelState.AddModelError(string.Empty, "Email ou mot de passe incorrect.");
                return View(model);
            }

            // Si le compte est bloqué, on affiche un message
            if (user.IsLocked)
            {
                TempData["Error"] = "Votre compte est actuellement bloqué. Contactez l’administrateur.";
                return View(model);
            }

            // Sinon, on remet les tentatives à 0 et on connecte l'utilisateur
            user.FailedLoginAttempts = 0;
            _context.SaveChanges();

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.FirstName);
            HttpContext.Session.SetInt32("IsAdmin", user.IsAdmin ? 1 : 0);

            return RedirectToAction("Index", "Home");
        }

        // Déconnecte l'utilisateur
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // Hachage du mot de passe avec SHA256
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
