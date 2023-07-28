using DataAuth.Roles;
using FluentValidation;

public class RoleValidator : AbstractValidator<RoleModel>
{
    public RoleValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Code).NotEmpty().MaximumLength(50);
    }
}
