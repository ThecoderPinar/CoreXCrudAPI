namespace CoreXCrud.DTOs
{
    /// <summary>
    /// Sipariş Detay DTO Modeli
    /// </summary>
    public class OrderDetailDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
