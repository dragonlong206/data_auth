using DataAuth.Domains.AccessAttributes;
using FluentValidation;

public class AccessAttributeValidator : AbstractValidator<AccessAttributeModel>
{
    public AccessAttributeValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
    }
}
