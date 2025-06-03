using System.ComponentModel.DataAnnotations;

namespace Online_food_delivery_system.Models
{
    public class MenuItemDTO
    {
        [Required(ErrorMessage = "Please Enter Your Name")]
        [MaxLength(255)]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Please Enter Description of the Item")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Please Enter Price")]
        [Range(20, 30000, ErrorMessage = "Price must be between 20 and 30000")]
        public decimal? Price { get; set; }
        public string? ImageUrl { get; set; }
        public float? Rating { get; set; }

        [Required(ErrorMessage = "Please Enter Restaurant ID")]
        public int? RestaurantID { get; set; }
    }
}