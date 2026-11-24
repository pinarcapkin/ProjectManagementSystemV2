using FluentValidation;
using ProjectManagementSystemV2.Entities.DTOs;

namespace ProjectManagementSystemV2.Validators;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.Email)// e postanın içi için bir kural tanımlayacağım
            .NotEmpty().WithMessage("Email boş olamaz.")// boş olamaz
            .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.");// gerçekçi bir e posta olmalı

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifre boş olamaz.")// WithMessage : eğer kural sağlanmaz ise kullanıcıya bu mesajı gönder
            .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalı.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("İsim boş olamaz.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Soyisim boş olamaz.");
    }
}