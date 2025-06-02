using Online_food_delivery_system.Interface;
using Online_food_delivery_system.Models;
using Online_food_delivery_system.Repository;

public class OrderService
{
    private readonly IOrder _orderRepository;
    private readonly IPayment _paymentRepository;

    public OrderService(IOrder orderRepository,IPayment paymentRepository)
    {
        _orderRepository = orderRepository;
        _paymentRepository = paymentRepository;
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await _orderRepository.GetAllAsync();
    }

    public async Task<Order> GetOrderByIdAsync(int orderID)
    {
        return await _orderRepository.GetByIdAsync(orderID);
    }

    public async Task<Order> AddOrderAsync(Order order)
    {
       await _orderRepository.AddAsync(order);
        return order;
    }
  


    public async Task<Order> UpdateOrderAsync(Order order)
    {
    //    order.CalculateTotalAmount();
        await _orderRepository.UpdateAsync(order);
        return order;
    }

    public async Task DeleteOrderAsync(int orderID)
    {
        await _orderRepository.DeleteAsync(orderID);
    }
   

    public async Task UpdateAgentAsync(Agent agent)
    {
        await _orderRepository.UpdateAgentAsync(agent);
    }
    public async Task CreatePaymentAsync(Payment payment)
    {
        await _paymentRepository.AddAsync(payment);
    }
    public async Task<Agent?> GetAvailableAgentAsync()
    {
        return await _orderRepository.GetAvailableAgentAsync();
    }
    public async Task UpdateDeliveryAsync(Delivery delivery)
    {
        await _orderRepository.UpdateDeliveryAsync(delivery);
    }



}
