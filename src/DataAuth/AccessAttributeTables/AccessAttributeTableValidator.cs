// Create FluentValidator for AccessAttributeTableModel
using DataAuth.AccessAttributeTables;
using FluentValidation;

public class AccessAttributeTableValidator : AbstractValidator<AccessAttributeTableModel>
{
    public AccessAttributeTableValidator()
    {
        RuleFor(x => x.AccessAttributeId).NotEmpty();
        RuleFor(x => x.TableName).NotEmpty();
        RuleFor(x => x.Alias).NotEmpty();
        RuleFor(x => x.IdColumn).NotEmpty();
        RuleFor(x => x.NameColumn).NotEmpty();
    }
}
