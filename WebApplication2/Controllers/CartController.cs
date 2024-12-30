using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    /// <summary>
    /// Контроллер для работы с корзиной покупок.
    /// </summary>
    [Authorize]
    [Route("Cart")]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Конструктор контроллера корзины.
        /// </summary>
        /// <param name="context">Контекст базы данных.</param>
        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Метод для получения элементов корзины из сессии.
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
        /// Метод для добавления товара в корзину.
        /// </summary>
        /// <param name="productId">Идентификатор товара.</param>
        /// <param name="quantity">Количество товара.</param>
        /// <returns>Результат добавления товара в корзину.</returns>
        [HttpPost]
        [Route("AddToCart/{productId}")]
        public IActionResult AddToCart(int productId, int quantity = 1)
        {
            // Проверяем, существует ли продукт с указанным ID
            var product = _context.Products.Find(productId);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            // Получаем текущие элементы корзины
            var cartItems = GetCartItems();
            var existingItem = cartItems.FirstOrDefault(c => c.Product.Id == product.Id);

            // Обновляем количество товара, если он уже есть в корзине
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                // Добавляем новый товар в корзину
                cartItems.Add(new CartItem { Product = product, Quantity = quantity });
            }

            // Сохраняем обновлённую корзину в сессии
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cartItems));

            // Редирект на главную страницу
            return RedirectToAction("Index", "Store"); // Здесь "Index" — это метод в контроллере "Store"
        }

        /// <summary>
        /// Метод для очистки корзины.
        /// </summary>
        /// <returns>Ответ с успешным удалением корзины.</returns>
        [HttpPost]
        [Route("ClearCart")]
        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove("Cart");
            return Ok("Cart cleared.");
        }

        /// <summary>
        /// Метод для завершения заказа и уменьшения количества на складе.
        /// </summary>
        /// <returns>Представление с подтверждением заказа.</returns>
        [HttpGet]
        [Route("CompleteOrder")]
        public IActionResult CompleteOrder()
        {
            var cartItems = GetCartItems();
            if (cartItems == null || !cartItems.Any())
            {
                return BadRequest("Cart is empty.");
            }

            decimal total = 0;

            foreach (var item in cartItems)
            {
                var product = _context.Products.Find(item.Product.Id);
                if (product == null)
                {
                    return NotFound($"Product with ID {item.Product.Id} not found.");
                }

                if (product.Stock < item.Quantity)
                {
                    return BadRequest($"Not enough stock for product {product.Name}.");
                }

                product.Stock -= item.Quantity; // Уменьшаем количество на складе
                total += item.Quantity * product.Price;
            }

            _context.SaveChanges(); // Сохраняем изменения в базе данных
            ClearCart(); // Очищаем корзину

            ViewBag.OrderDetails = $"Your order has been placed! Total amount: {total:C}";
            return View("OrderConfirmation");
        }

        /// <summary>
        /// Метод для отображения корзины покупок.
        /// </summary>
        /// <returns>Представление с элементами корзины.</returns>
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            var cartItems = GetCartItems();
            return View(cartItems);
        }

        /// <summary>
        /// Метод для оформления заказа (переход на страницу оформления).
        /// </summary>
        /// <returns>Представление с деталями оформления заказа.</returns>
        [HttpGet]
        [Route("Checkout")]
        public IActionResult Checkout()
        {
            var cartItems = GetCartItems();

            if (!cartItems.Any())
            {
                TempData["Error"] = "Your cart is empty. Please add items before checking out.";
                return RedirectToAction("Index", "Cart");
            }

            // Рассчитываем общую стоимость товаров
            decimal total = cartItems.Sum(item => item.Quantity * item.Product.Price);

            // Передаем данные в представление
            ViewBag.Total = total;
            return View(cartItems);
        }
    }
}
