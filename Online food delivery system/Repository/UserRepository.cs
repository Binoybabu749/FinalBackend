using Microsoft.EntityFrameworkCore;
using Online_food_delivery_system.Interface;
using Online_food_delivery_system.Models;

namespace Online_food_delivery_system.Repository
{
    public class UserRepository : IUser
    {
        private readonly FoodDbContext _context;
        public UserRepository(FoodDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<User> GetByIdAsync(int userId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.ID == userId);
        }
        public async Task AddAsync(User user)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingUser != null)
            {
                throw new Exception("User with this email already exists");
            }
            if (user.Role?.ToLower() == "customer")
            {
                var existingCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == user.Email);
                if (existingCustomer == null)
                {
                    var customer = new Customer
                    {
                        Name = user.Username,
                        Email = user.Email,
                        Phone = "+91",
                        Address = "Address"

                    };
                    _context.Customers.Add(customer);
                    await _context.SaveChangesAsync();
                }
            }
            else if (user.Role?.ToLower() == "agent")
            {
                var existingCustomer = await _context.Agents.FirstOrDefaultAsync(c => c.Email == user.Email);
                if (existingCustomer == null)
                {
                    var agent = new Agent
                    {
                        Name = user.Username,
                        Email = user.Email,
                        AgentContact = "+91"

                    };
                    _context.Agents.Add(agent);
                    await _context.SaveChangesAsync();
                }
            }
            else if (user.Role?.ToLower() == "restaurant")
            {
                var existingCustomer = await _context.Restaurants.FirstOrDefaultAsync(c => c.Email == user.Email);
                if (existingCustomer == null)
                {
                    var restaurant = new Restaurant
                    {
                        RestaurantName = user.Username,
                        Email = user.Email,
                        RestaurantContact = "+91",
                        Address = "default",
                        Availability = true,

                    };
                    _context.Restaurants.Add(restaurant);
                    await _context.SaveChangesAsync();
                }
            }
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(User user)
        {


            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }



}