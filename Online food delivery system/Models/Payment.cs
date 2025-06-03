using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_food_delivery_system.Models
{
    public class Payment
    {

        [Key]
        public int PaymentID { get; set; }

        [ForeignKey("Order")]
        public int? OrderID { get; set; }
        public Order? Order { get; set; }

        [Required]
        [MaxLength(50)]
        public string? PaymentMethod { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? Amount { get; set; }

        [MaxLength(50)]
        public string? Status { get; set; }
        public DateTime? PaymentTime { get; set; }

        public Delivery? Delivery { get; set; }




    }
}
