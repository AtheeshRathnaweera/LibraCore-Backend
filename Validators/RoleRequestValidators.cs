using FluentValidation;
using LibraCore.Backend.DTOs.Role;

namespace LibraCore.Backend.Validators;

public class CreateRoleRequestValidator : AbstractValidator<CreateRoleRequest>
{
  public CreateRoleRequestValidator()
  {
    RuleFor(x => x.Name).NotEmpty();
  }
}

public class UpdateRoleRequestValidator : AbstractValidator<UpdateRoleRequest>
{
  public UpdateRoleRequestValidator()
  {
    RuleFor(x => x.Id).GreaterThan(0).WithMessage("'Id' is required and must be greater than zero.");
    RuleFor(x => x.Name).NotEmpty();
  }
}