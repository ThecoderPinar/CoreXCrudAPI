using CoreXCrud.DTOs;
using FluentValidation;

namespace CoreXCrud.Validators
{
    public class OrderValidator : AbstractValidator<OrderDto>
    {
        public OrderValidator()
        {
            RuleFor(o => o.UserId).GreaterThan(0).WithMessage("Kullanıcı ID geçerli olmalıdır");
            RuleFor(o => o.OrderDate).NotEmpty().WithMessage("Sipariş tarihi boş olamaz");
            RuleFor(o => o.TotalAmount).GreaterThan(0).WithMessage("Toplam tutar sıfırdan büyük olmalıdır");
        }
    }
}