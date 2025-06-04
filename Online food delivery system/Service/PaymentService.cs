using System.Threading.Tasks;
using Online_food_delivery_system.Interface;
using Online_food_delivery_system.Models;

namespace Online_food_delivery_system.Service
{
    public class PaymentService
    {
        private readonly IPayment _paymentRepository;
        private readonly IOrder _orderRepository;

        public PaymentService(IPayment paymentRepository, IOrder orderRepository)
        {
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
        {
            return await _paymentRepository.GetAllAsync();
        }

        public async Task<Payment> GetPaymentByIdAsync(int orderID)
        {
            return await _paymentRepository.GetByIdAsync(orderID);
        }

        public async Task AddPaymentAsync(Payment payment)
        {
            var order = await _orderRepository.GetByIdAsync(payment.OrderID.Value); // Ensure OrderID is not null before calling GetByIdAsync
            if (order != null)
            {
                // order.CalculateTotalAmount();
                payment.Amount = order.TotalAmount;
            }
            await _paymentRepository.AddAsync(payment);
        }

        public async Task UpdatePaymentAsync(Payment payment)
        {
            var order = await _orderRepository.GetByIdAsync(payment.OrderID.Value); // Ensure OrderID is not null before calling GetByIdAsync
            if (order != null)
            {
                // order.CalculateTotalAmount();
                payment.Amount = order.TotalAmount;
            }
            await _paymentRepository.UpdateAsync(payment);
        }

        public async Task DeletePaymentAsync(int paymentID)
        {
            await _paymentRepository.DeleteAsync(paymentID);
        }

        public async Task<Payment> ProcessPaymentAsync(Payment payment)
        {
            var order = await _orderRepository.GetByIdAsync(payment.OrderID.Value); // Ensure OrderID is not null before calling GetByIdAsync
            if (order != null)
            {
                // Calculate the total amount first
                // order.CalculateTotalAmount();
                payment.Amount = order.TotalAmount;
            }

            // Then set the status to "Confirmed"
            if (payment.Status?.ToLower() == "confirmed")
            {
                payment.Status = "Confirmed";
            }

            return await _paymentRepository.ProcessPaymentAsync(payment);
        }
        public async Task UpdateDeliveryAsync(Delivery delivery)
        {
            await _orderRepository.UpdateDeliveryAsync(delivery);
        }

        public async Task UpdateAgentAsync(Agent agent)
        {
            await _orderRepository.UpdateAgentAsync(agent);
        }
        public async Task<Agent?> GetAvailableAgentAsync()
        {
            return await _orderRepository.GetAvailableAgentAsync();
        }
    }
}
