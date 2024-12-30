using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models;

public class CartItem
{
    public int Id { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }
}

