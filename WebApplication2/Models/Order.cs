namespace WebApplication2.Models;

public class Order
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public List<CartItem> Items { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime OrderDate { get; set; }
}