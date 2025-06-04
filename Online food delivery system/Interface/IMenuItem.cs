using Online_food_delivery_system.Models;

namespace Online_food_delivery_system.Interface
{
    public interface IMenuItem
    {
        Task<IEnumerable<MenuItem>> GetAllAsync();
        Task<MenuItem?> GetByIdAsync(int itemId);
        Task AddAsync(MenuItem menuItem);
        Task UpdateAsync(MenuItem menuItem);
        Task DeleteAsync(int itemId);
        Task<IEnumerable<MenuItem>> GetMenuItemsByNameAsync(string menuItemName);

        Task<IEnumerable<Restaurant>> GetRestaurantsByMenuItemNameAsync(string menuItemName);
    }
}



