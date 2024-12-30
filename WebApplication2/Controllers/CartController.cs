using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers;
[Authorize]
public class CartController : Controller
{
    private static List<CartItem> _cart = new List<CartItem>();

    public IActionResult Index()
    {
        return View(_cart);
    }

    public IActionResult AddToCart(int productId)
    {
        var product = new Product { Id = productId, Name = $"Product {productId}", Price = 10.99m };
        var cartItem = _cart.FirstOrDefault(c => c.Product.Id == productId);
        if (cartItem == null)
        {
            _cart.Add(new CartItem { Product = product, Quantity = 1 });
        }
        else
        {
            cartItem.Quantity++;
        }

        return RedirectToAction("Index");
    }

    public IActionResult RemoveFromCart(int productId)
    {
        var cartItem = _cart.FirstOrDefault(c => c.Product.Id == productId);
        if (cartItem != null)
        {
            _cart.Remove(cartItem);
        }
        return RedirectToAction("Index");
    }
}
