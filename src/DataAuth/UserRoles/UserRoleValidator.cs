using DataAuth.Entities;
using DataAuth.UserRoles;
using FluentValidation;

public class UserRoleValidator : AbstractValidator<UserRoleModel>
{
    public UserRoleValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.RoleId).NotEmpty();
    }
}
