using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models;

public class CartItem
{
    public Product Product { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public int Quantity { get; set; }
}

