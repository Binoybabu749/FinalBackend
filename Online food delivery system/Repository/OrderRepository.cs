using Microsoft.EntityFrameworkCore;
using Online_food_delivery_system.Interface;
using Online_food_delivery_system.Models;

namespace Online_food_delivery_system.Repository
{
    public class OrderRepository : IOrder
    {
        private readonly FoodDbContext _context;
        public OrderRepository(FoodDbContext context)
        {
            _context = context;
        }
        public async Task<Agent?> GetAvailableAgentAsync()
        {
            return await _context.Agents.FirstOrDefaultAsync(a => a.IsAvailable);
        }
        public async Task UpdateDeliveryAsync(Delivery delivery)
        {
            _context.Deliveries.Update(delivery);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateAgentAsync(Agent agent)
        {
            _context.Agents.Update(agent);
            await _context.SaveChangesAsync();
        }

        public async Task AddAsync(Order order)
        {
            // Extract the list of ItemIDs from OrderMenuItems
            IList<int> orderMenuItemIds = order.OrderMenuItems.Select(o => o.ItemID).ToList();

            // Fetch the corresponding MenuItems from the database
            IEnumerable<MenuItem> menuItems = await _context.MenuItems
                .Where(m => orderMenuItemIds.Contains(m.ItemID))
                .ToListAsync();

            // ✅ Calculate the total amount using quantity
            decimal total = 0;
            foreach (var omi in order.OrderMenuItems)
            {
                var menuItem = menuItems.FirstOrDefault(m => m.ItemID == omi.ItemID);
                if (menuItem != null)
                {
                    total += (menuItem.Price ?? 0) * omi.Quantity;
                }
            }
            order.TotalAmount = total;

            // Add the order
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            // Create the delivery entry
            Delivery delivery = new Delivery
            {
                OrderID = order.OrderID,
                Status = "Pending",
                EstimatedTimeOfArrival = DateTime.Now.AddHours(1) // Example ETA
            };
            await _context.Deliveries.AddAsync(delivery);
            await _context.SaveChangesAsync();

            // Associate the delivery with the order
            order.Delivery = delivery;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteAsync(int OrderID)
        {
            var ord = await _context.Orders.FindAsync(OrderID);
            if (ord != null)
            {
                _context.Orders.Remove(ord);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Restaurant)
                .Include(o => o.OrderMenuItems)
                .ThenInclude(omi => omi.MenuItem)
                .Include(o => o.Payment)
                .Include(o => o.Delivery)
                .ToListAsync();
        }

        public async Task<Order> GetByIdAsync(int? orderId)
        {
            return await _context.Orders
                .Include(o => o.Payment)
                .Include(o => o.Delivery)
                .FirstOrDefaultAsync(o => o.OrderID == orderId);
        }


        public async Task UpdateAsync(Order order)
        {
            // Update the order
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            // Update the payment entry
            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.OrderID == order.OrderID);
            if (payment != null)
            {
                payment.Amount = order.TotalAmount;
                _context.Payments.Update(payment);
                await _context.SaveChangesAsync();
            }
        }
    }
}