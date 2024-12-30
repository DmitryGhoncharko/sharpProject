using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    /// <summary>
    /// Контроллер для обработки запросов на главную страницу и страницы конфиденциальности.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        /// <summary>
        /// Конструктор контроллера.
        /// </summary>
        /// <param name="logger">Логгер для контроллера.</param>
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Метод для отображения главной страницы.
        /// </summary>
        /// <returns>Представление главной страницы.</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Метод для отображения страницы конфиденциальности.
        /// </summary>
        /// <returns>Представление страницы конфиденциальности.</returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Метод для обработки ошибок, если возникает исключение.
        /// </summary>
        /// <returns>Представление ошибки с идентификатором запроса.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}