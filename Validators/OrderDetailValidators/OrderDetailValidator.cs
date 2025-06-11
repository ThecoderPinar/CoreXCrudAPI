using CoreXCrud.DTOs.OrderDetailDtos;
using FluentValidation;

namespace CoreXCrud.Validators
{
    public class OrderDetailValidator : AbstractValidator<OrderDetailDto>
    {
        public OrderDetailValidator()
        {
            RuleFor(od => od.OrderId).GreaterThan(0).WithMessage("Sipariş ID geçerli olmalıdır");
            RuleFor(od => od.ProductId).GreaterThan(0).WithMessage("Ürün ID geçerli olmalıdır");
            RuleFor(od => od.Quantity).GreaterThan(0).WithMessage("Miktar sıfırdan büyük olmalıdır");
            RuleFor(od => od.UnitPrice).GreaterThan(0).WithMessage("Birim fiyat sıfırdan büyük olmalıdır");
        }
    }
}
