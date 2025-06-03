using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Online_food_delivery_system.Models
{
    public class MenuItem
    {
        [Key]
        public int ItemID { get; set; }

        [Required(ErrorMessage = "Please Enter Your Name")]
        [MaxLength(255)]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Please Enter Description of the Item")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Please Enter Price ")]
        [Column(TypeName = "decimal(10, 2)")]
        
        public decimal? Price { get; set; }
        public string? ImageUrl { get; set; }
        public float? Rating { get; set; }
        [ForeignKey("Restaurant")]
        public int? RestaurantID { get; set; }
        public Restaurant? Restaurant { get; set; }

        public List<OrderMenuItem>? OrderMenuItems { get; set; } = new List<OrderMenuItem>();

    }
}
