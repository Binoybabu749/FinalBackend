﻿using Microsoft.EntityFrameworkCore;
using Online_food_delivery_system.Interface;
using Online_food_delivery_system.Models;

namespace Online_food_delivery_system.Service
{
    public class DeliveryService
    {
        private readonly IDelivery _deliveryRepository;

        public DeliveryService(IDelivery deliveryRepository)
        {
            _deliveryRepository = deliveryRepository;
        }

        public async Task<IEnumerable<Delivery>> GetAllDeliveriesAsync()
        {
            return await _deliveryRepository.GetAllAsync();
        }

        public async Task<Delivery> GetDeliveryByIdAsync(int deliveryId)

        {
            return await _deliveryRepository.GetByIdAsync(deliveryId);
        }
        public async Task AssignDeliveryAgentAutomaticallyAsync(int orderId, DateTime estimatedTimeOfArrival)
        {
            // Validate the order exists


            var availableAgents = await _deliveryRepository.GetAvailableAgentsAsync();
            if (!availableAgents.Any())
            {
                throw new Exception("No available delivery agents at the moment.");
            }

            var selectedAgent = availableAgents.First();
            selectedAgent.IsAvailable = false; // Mark the agent as unavailable

            var delivery = new Delivery
            {
                OrderID = orderId,
                AgentID = selectedAgent.AgentID,
                Status = "Assigned",
                EstimatedTimeOfArrival = estimatedTimeOfArrival
            };

            // Save the delivery and update agent availability
            await _deliveryRepository.AddAsync(delivery);
            await _deliveryRepository.UpdateAgentAvailabilityAsync(selectedAgent);
        }

        public async Task UpdateAgentAsync(Agent agent)
        {
            await _deliveryRepository.UpdateAgentAvailabilityAsync(agent);
        }


        public async Task UpdateDeliveryAsync(int deliveryId, string status)
        {
            var delivery = await _deliveryRepository.GetByIdAsync(deliveryId);
            if (delivery == null)
            {
                throw new Exception("Delivery not found");
            }

            delivery.Status = status;

            if (status.ToLower() == "completed")
            {
                var agent = delivery.Agent;
                if (agent != null)
                {
                    agent.IsAvailable = true;
                    await _deliveryRepository.UpdateAgentAvailabilityAsync(agent); // Ensure this is called
                }
            }

            await _deliveryRepository.UpdateAsync(delivery);
        }




        public async Task AssignDeliveryAgentAsync(int orderId, int agentId, DateTime estimatedTimeOfArrival)
        {
            var delivery = new Delivery
            {
                OrderID = orderId,
                AgentID = agentId,
                Status = "In Progress",
                EstimatedTimeOfArrival = estimatedTimeOfArrival
            };

            await _deliveryRepository.AddAsync(delivery);
        }



        public async Task DeleteDeliveryAsync(int deliveryId)
        {
            await _deliveryRepository.DeleteAsync(deliveryId);
        }
    }
}
