namespace Online_food_delivery_system.Models
{
    public class OrderDTO
    {
        public int CustomerID { get; set; }
        public int RestaurantID { get; set; }
        public List<int> MenuItemIDs { get; set; } = new List<int>();
        public string? Status { get; set; }

    }

}
