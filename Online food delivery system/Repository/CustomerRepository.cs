using Microsoft.EntityFrameworkCore;
using Online_food_delivery_system.Interface;
using Online_food_delivery_system.Models;

namespace Online_food_delivery_system.Repository
{
    public class CustomerRepository : ICustomer
    {
        private readonly FoodDbContext _context;

        public CustomerRepository(FoodDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _context.Customers
                .Include(c => c.Orders) // Include related Orders
                    .ThenInclude(o => o.Restaurant) // Include Restaurant in Orders
                .Include(c => c.Orders)
                    .ThenInclude(o => o.Payment) // Include Payment in Orders
                .Include(c => c.Orders)
                    .ThenInclude(o => o.Delivery) // Include Delivery in Orders
                        .ThenInclude(d => d.Agent) // Include Agent in Delivery
                .Include(c => c.Orders)
                    .ThenInclude(o => o.OrderMenuItems) // Include OrderMenuItems in Orders
                        .ThenInclude(omi => omi.MenuItem) // Include MenuItem in OrderMenuItems
                .ToListAsync();
        }


        public async Task<Customer> GetByIdAsync(string email)
        {
            var customer = await _context.Customers
                .Include(c => c.Orders)
                    .ThenInclude(o => o.Restaurant)
                .Include(c => c.Orders)
                    .ThenInclude(o => o.Payment)
                .Include(c => c.Orders)
                    .ThenInclude(o => o.Delivery)
                        .ThenInclude(d => d.Agent)
                .Include(c => c.Orders)
                    .ThenInclude(o => o.OrderMenuItems)
                        .ThenInclude(omi => omi.MenuItem)
                .FirstOrDefaultAsync(c => c.Email == email);

            // Set ImageUrl to null for all MenuItems in the object graph
            if (customer?.Orders != null)
            {
                foreach (var order in customer.Orders)
                {
                    if (order.OrderMenuItems != null)
                    {
                        foreach (var omi in order.OrderMenuItems)
                        {
                            if (omi.MenuItem != null)
                            {
                                omi.MenuItem.ImageUrl = null;
                            }
                        }
                    }
                    if (order.Restaurant?.MenuItems != null)
                    {
                        foreach (var menuItem in order.Restaurant.MenuItems)
                        {
                            menuItem.ImageUrl = null;
                        }
                    }
                }
            }

            return customer;
        }


        public async Task AddAsync(Customer customer)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == customer.Email);
            if (existingUser == null)
            {
                var user = new User
                {
                    Username = customer.Name,
                    Email = customer.Email,
                    Password = "default",
                    Role = "customer"
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }
    }
}
