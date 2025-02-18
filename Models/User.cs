using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreXCrud.Models
{
    /// <summary>
    /// Kullanıcı tablosu modeli.
    /// </summary>
    public class User
    {
        [Key] // Birincil anahtar
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required, StringLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required, StringLength(20)]
        public string Role { get; set; } = "User"; // "Admin" veya "User"

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Kullanıcının oluşturduğu siparişler
        public ICollection<Order>? Orders { get; set; }
    }
}
