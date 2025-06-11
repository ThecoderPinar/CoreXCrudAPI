namespace CoreXCrud.DTOs.ProductDtos
{
    /// <summary>
    /// Ürün DTO Modeli
    /// </summary>
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
