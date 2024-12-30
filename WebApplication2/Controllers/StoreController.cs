using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers;

public class StoreController : Controller
{
    private readonly List<Product> _products;

    public StoreController()
    {
        // Примерные данные для товаров
        _products = new List<Product>
        {
            new Product { Id = 1, Name = "Product 1", Price = 10.99m, Description = "Description 1", ImageUrl = "product1.jpg" },
            new Product { Id = 2, Name = "Product 2", Price = 15.99m, Description = "Description 2", ImageUrl = "product2.jpg" }
        };
    }

    public IActionResult Index()
    {
        return View(_products);
    }
}
