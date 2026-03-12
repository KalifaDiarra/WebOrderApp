using Microsoft.AspNetCore.Mvc;
using WebOrderApp.Data;
using WebOrderApp.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace WebOrderApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Constructeur pour injecter le contexte de la base de données
        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Vérifie si l'utilisateur est connecté (présence d'un ID dans la session)
        private bool IsUserAuthenticated() =>
            HttpContext.Session.GetInt32("UserId") != null;

        // Vérifie si c'est un admin
        private bool IsAdmin() =>
            HttpContext.Session.GetInt32("IsAdmin") == 1;

        // Vérifie si l'utilisateur est connecté ET admin
        private bool IsAuthorized() =>
            IsUserAuthenticated() && IsAdmin();

        // Affiche la liste de tous les utilisateurs (seulement visible par admin)
        public IActionResult Index()
        {
            if (!IsAuthorized())
                return RedirectToAction("Login", "Account");

            ViewBag.Message = TempData["Message"];
            var users = _context.Users.ToList();
            return View(users);
        }

        // Formulaire pour ajouter un nouvel utilisateur
        public IActionResult Create()
        {
            if (!IsAuthorized())
                return RedirectToAction("Login", "Account");

            return View();
        }

        // Traitement du formulaire d’ajout d’un nouvel utilisateur
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("FirstName,LastName,Email,BirthDate,Phone,Address")] User user)
        {
            if (!IsAuthorized())
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                // Valeurs par défaut
                user.PasswordHash = "";
                user.IsAdmin = false;
                user.FailedLoginAttempts = 0;
                user.IsLocked = false;

                _context.Users.Add(user);
                _context.SaveChanges();

                TempData["Message"] = "✅ Utilisateur ajouté avec succès.";
                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }

        // Formulaire de modification d’un utilisateur
        public IActionResult Edit(int id)
        {
            if (!IsAuthorized())
                return RedirectToAction("Login", "Account");

            var user = _context.Users.Find(id);
            if (user == null) return NotFound();
            return View(user);
        }

        // Traitement du formulaire de modification
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,FirstName,LastName,Email,BirthDate,Phone,Address")] User formUser)
        {
            if (!IsAuthorized())
                return RedirectToAction("Login", "Account");

            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();

            if (ModelState.IsValid)
            {
                // Mise à jour des champs modifiables
                user.FirstName = formUser.FirstName;
                user.LastName = formUser.LastName;
                user.Email = formUser.Email;
                user.BirthDate = formUser.BirthDate;
                user.Phone = formUser.Phone;
                user.Address = formUser.Address;

                _context.Users.Update(user);
                _context.SaveChanges();

                TempData["Message"] = "✅ Utilisateur mis à jour avec succès.";
                return RedirectToAction(nameof(Index));
            }

            return View(formUser);
        }

        // Affiche la page de confirmation de suppression
        public IActionResult Delete(int id)
        {
            if (!IsAuthorized())
                return RedirectToAction("Login", "Account");

            var user = _context.Users.Find(id);
            if (user == null) return NotFound();
            return View(user);
        }

        // Supprime définitivement l'utilisateur
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!IsAuthorized())
                return RedirectToAction("Login", "Account");

            var user = _context.Users.Find(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            _context.SaveChanges();

            TempData["Message"] = "Utilisateur supprimé.";
            return RedirectToAction(nameof(Index));
        }

        // Promouvoir un utilisateur normal en administrateur
        [HttpPost]
        public IActionResult PromoteToAdmin(int id)
        {
            if (!IsAuthorized())
                return RedirectToAction("Login", "Account");

            var user = _context.Users.Find(id);
            if (user == null) return NotFound();

            user.IsAdmin = true;
            _context.SaveChanges();

            TempData["Message"] = $"{user.FirstName} {user.LastName} est maintenant administrateur.";
            return RedirectToAction(nameof(Index));
        }

        // Page admin pour voir les comptes bloqués (tentatives échouées >= 3)
        public IActionResult ResetAccounts()
        {
            if (!IsAuthorized())
                return RedirectToAction("Login", "Account");

            var blockedUsers = _context.Users
                .Where(u => u.IsLocked || u.FailedLoginAttempts >= 3)
                .ToList();

            return View(blockedUsers);
        }

        // Débloquer un utilisateur depuis la page admin
        [HttpPost]
        public IActionResult UnblockUser(int id)
        {
            if (!IsAuthorized())
                return RedirectToAction("Login", "Account");

            var user = _context.Users.Find(id);
            if (user == null) return NotFound();

            user.FailedLoginAttempts = 0;
            user.IsLocked = false;

            _context.SaveChanges();

            TempData["Message"] = $"{user.FirstName} {user.LastName} a été débloqué.";
            return RedirectToAction(nameof(ResetAccounts));
        }
    }
}
