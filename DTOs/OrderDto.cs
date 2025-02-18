namespace CoreXCrud.DTOs
{
    /// <summary>
    /// Sipariş DTO Modeli
    /// </summary>
    public class OrderDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
