
using Online_food_delivery_system.Models;

public class OrderMenuItem
{
    public int OrderID { get; set; }
    public Order? Order { get; set; }

    public int ItemID { get; set; }
    public MenuItem? MenuItem { get; set; }

    public int Quantity { get; set; } = 1; // NEW FIELD
}
