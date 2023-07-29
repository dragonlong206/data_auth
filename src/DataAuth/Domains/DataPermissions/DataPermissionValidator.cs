// Create FluentValidation for DataPermission, GrantedDataValue is required when AccessLevel is Deep or Specific.
using DataAuth.Domains.DataPermissions;
using DataAuth.Enums;
using FluentValidation;

public class DataPermissionValidator : AbstractValidator<DataPermissionModel>
{
    public DataPermissionValidator()
    {
        RuleFor(x => x.SubjectId).NotEmpty();
        RuleFor(x => x.AccessAttributeTableId).NotEmpty();
        RuleFor(x => x.GrantedDataValue)
            .NotEmpty()
            .When(x => x.AccessLevel == AccessLevel.Deep || x.AccessLevel == AccessLevel.Specific);
    }
}
