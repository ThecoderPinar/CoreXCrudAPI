using CoreXCrud.DTOs.ProductDtos;
using FluentValidation;

namespace CoreXCrud.Validators.ProductValidators
{
    public class ProductValidator : AbstractValidator<ProductDto>
    {
        public ProductValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Ürün adı boş olamaz");
            RuleFor(p => p.Price).GreaterThan(0).WithMessage("Fiyat sıfırdan büyük olmalıdır");
            RuleFor(p => p.Stock).GreaterThanOrEqualTo(0).WithMessage("Stok negatif olamaz");
        }
    }
}
