using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebOrderApp.Models;

namespace WebOrderApp.Controllers;

// Contrôleur principal pour gérer les pages publiques comme Accueil et Confidentialité
public class HomeController : Controller
{
    // Logger pour écrire des messages de log (utile pour débogage ou diagnostics)
    private readonly ILogger<HomeController> _logger;

    // Constructeur qui injecte le logger
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    // Action pour afficher la page d'accueil (Index.cshtml)
    public IActionResult Index()
    {
        return View();
    }

    // Action pour afficher la page de politique de confidentialité (Privacy.cshtml)
    public IActionResult Privacy()
    {
        return View();
    }

    // Action pour gérer les erreurs (ex: page 500, exceptions, etc.)
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        // On renvoie la vue Error avec l'ID de la requête pour affichage
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}


