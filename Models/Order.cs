using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreXCrud.Models
{
    /// <summary>
    /// Sipariş tablosu modeli.
    /// </summary>
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        // Bir sipariş birçok ürünü içerebilir (Many-to-Many ilişki)
        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
