using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Controllers;

public class DocumentationController : Controller
{
    /// <summary>
    /// Страница с документацией для пользователей.
    /// </summary>
    /// <returns>Представление страницы документации.</returns>
    public IActionResult Index()
    {
        return View();
    }
}
