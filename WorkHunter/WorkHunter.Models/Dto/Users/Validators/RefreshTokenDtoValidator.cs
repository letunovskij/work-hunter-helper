using FluentValidation;

namespace WorkHunter.Models.Dto.Users.Validators;

public sealed class RefreshTokenDtoValidator : AbstractValidator<RefreshTokenDto>
{
    public RefreshTokenDtoValidator()
    {
        RuleFor(x => x.Token).NotEmpty();
    }
}
