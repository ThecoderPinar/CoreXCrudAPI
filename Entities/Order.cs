using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreXCrud.Entities
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

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Pending"; // 📌 Yeni Sipariş Durumu

        // Bir sipariş birçok ürünü içerebilir (Many-to-Many ilişki)
        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}