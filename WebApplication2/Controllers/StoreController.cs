using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    /// <summary>
    /// Контроллер для управления товарами в магазине и корзиной.
    /// </summary>
    public class StoreController : Controller
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Конструктор контроллера.
        /// </summary>
        /// <param name="context">Контекст базы данных для работы с продуктами.</param>
        public StoreController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получает список товаров, сохраненных в корзине, из сессии.
        /// </summary>
        /// <returns>Список элементов корзины.</returns>
        private List<CartItem> GetCartItems()
        {
            var cart = HttpContext.Session.GetString("Cart");
            if (string.IsNullOrEmpty(cart))
            {
                return new List<CartItem>();
            }
            return JsonConvert.DeserializeObject<List<CartItem>>(cart);
        }

        /// <summary>
        /// Логика добавления товара в корзину.
        /// </summary>
        /// <param name="product">Товар, который добавляется в корзину.</param>
        /// <param name="quantity">Количество товара, которое добавляется в корзину.</param>
        private void AddToCartLogic(Product product, int quantity)
        {
            var cartItems = GetCartItems();
            var existingItem = cartItems.FirstOrDefault(c => c.Product.Id == product.Id);

            if (existingItem != null)
            {
                // Если товар уже в корзине, увеличиваем количество
                existingItem.Quantity += quantity;
            }
            else
            {
                // Если товара нет в корзине, добавляем новый элемент
                cartItems.Add(new CartItem { Product = product, Quantity = quantity });
            }

            // Сохраняем обновленную корзину в сессии
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cartItems));
        }

        /// <summary>
        /// Метод для отображения списка товаров в магазине.
        /// </summary>
        /// <returns>Представление с товарами магазина.</returns>
        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        /// <summary>
        /// Метод для отображения подробной информации о товаре.
        /// </summary>
        /// <param name="id">Идентификатор товара.</param>
        /// <returns>Представление с деталями товара.</returns>
        public IActionResult ProductDetails(int id)
        {
            var product = _context.Products.Find(id);
            return View(product);
        }

        /// <summary>
        /// Реализует поиск товара по наименованию.
        /// </summary>
        /// <param name="search">Текст для поиска товара.</param>
        /// <returns>Представление с результатами поиска товаров.</returns>
        public IActionResult Search(string search)
        {
            var products = string.IsNullOrEmpty(search)
                ? _context.Products.ToList()
                : _context.Products.Where(p => p.Name.Contains(search)).ToList();

            return View("Index", products);
        }

        /// <summary>
        /// Метод для добавления товара в корзину.
        /// </summary>
        /// <param name="productId">Идентификатор товара.</param>
        /// <param name="quantity">Количество товара, которое добавляется в корзину.</param>
        /// <returns>Редирект на страницу магазина после добавления товара в корзину.</returns>
        public IActionResult AddToCart(int productId, int quantity)
        {
            var product = _context.Products.Find(productId);
            if (product != null && product.Stock >= quantity)
            {
                AddToCartLogic(product, quantity); // Добавляем товар в корзину
            }
            else
            {
                TempData["Error"] = "Not enough stock available!";
            }
            return RedirectToAction("Index", "Store");
        }
    }
}
