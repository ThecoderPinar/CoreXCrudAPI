using CoreXCrud.DTOs;
using FluentValidation;

namespace CoreXCrud.Validators
{
    public class UserValidator : AbstractValidator<UserDto>
    {
        public UserValidator()
        {
            RuleFor(u => u.FullName).NotEmpty().WithMessage("Ad boş olamaz");
            RuleFor(u => u.Email).NotEmpty().EmailAddress().WithMessage("Geçerli bir e-posta adresi girin");
        }
    }
}
