using Online_food_delivery_system.Models;

namespace Online_food_delivery_system.Interface
{
    public interface IUser
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
    }
}
