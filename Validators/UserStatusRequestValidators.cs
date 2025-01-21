using FluentValidation;
using LibraCore.Backend.DTOs.UserStatus;

namespace LibraCore.Backend.Validators;

public class CreateUserStatusRequestValidator : AbstractValidator<CreateUserStatusRequest>
{
  public CreateUserStatusRequestValidator()
  {
    RuleFor(x => x.Name).NotEmpty();
  }
}

public class UpdateUserStatusRequestValidator : AbstractValidator<UpdateUserStatusRequest>
{
  public UpdateUserStatusRequestValidator()
  {
    RuleFor(x => x.Id).GreaterThan(0).WithMessage("'Id' is required and must be greater than zero.");
    RuleFor(x => x.Name).NotEmpty();
  }
}