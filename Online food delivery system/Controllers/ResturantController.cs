﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Online_food_delivery_system.Models;
using Online_food_delivery_system.Service;

namespace Online_food_delivery_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowReactApp")]
    public class RestaurantController : ControllerBase
    {
        private readonly RestaurantService _restaurantService;

        public RestaurantController(RestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet]
        //[Authorize(Roles = "customer,restaurant,admin")]
        public async Task<IActionResult> GetAllRestaurants()
        {
            var restaurants = await _restaurantService.GetAllRestaurantsAsync();
            return Ok(restaurants);
        }

        [HttpGet("{email}")]
        [Authorize(Roles = "customer,restaurant,admin")]
        public async Task<IActionResult> GetRestaurantById(string email)
        {
            var restaurant = await _restaurantService.GetRestaurantByIdAsync(email);
            if (restaurant == null)
                return NotFound("Restaurant not found");
            return Ok(restaurant);
        }

        [HttpPost]
        [Authorize(Roles = "restaurant,admin")]

        public async Task<IActionResult> AddRestaurant([FromBody] RestaurantDTO restaurantDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var restaurant = new Restaurant
            {
                RestaurantName = restaurantDto.RestaurantName,
                RestaurantContact = restaurantDto.RestaurantContact,
                Availability = restaurantDto.Availability,
                Address = restaurantDto.Address,
                Email = restaurantDto.Email
            };

            await _restaurantService.AddRestaurantAsync(restaurant);
            return CreatedAtAction(nameof(GetRestaurantById), new { id = restaurant.RestaurantID }, restaurant);
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "restaurant,admin")]
        public async Task<IActionResult> UpdateRestaurant(int id, [FromBody] Restaurant restaurant)
        {
            if (id != restaurant.RestaurantID)
                return BadRequest("Restaurant ID mismatch");

            await _restaurantService.UpdateRestaurantAsync(restaurant);
            return NoContent();
        }
        [HttpPatch("{email}")]
        [Authorize(Roles = "admin, restaurant")]
        public async Task<IActionResult> UpdatePhoneAddr(string email, [FromBody] UpdatePhoneAddrDTO upd)
        {
            var existing = await _restaurantService.GetRestaurantByIdAsync(email);
            if (existing == null)
                return NotFound("Restaurant not found");
            existing.RestaurantContact = upd.Phone;
            existing.Address = upd.Address;
            await _restaurantService.UpdateRestaurantAsync(existing);
            return NoContent();
        }
        [HttpPatch("{email}/availability")]
        public async Task<IActionResult> UpdateAvailability(string email, [FromBody] bool isAvailable)
        {
            var restaurant = await _restaurantService.GetRestaurantByIdAsync(email);

            if (restaurant == null)
            {
                return NotFound();
            }

            restaurant.Availability = isAvailable;
            await _restaurantService.UpdateRestaurantAsync(restaurant);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "restaurant,admin")]
        public async Task<IActionResult> DeleteRestaurant(int id)
        {
            await _restaurantService.DeleteRestaurantAsync(id);
            return NoContent();
        }
    }
}