using FluentValidation;
using LibraCore.Backend.DTOs.UserActiveStatus;
using LibraCore.Backend.DTOs.UserStatus;

namespace LibraCore.Backend.Validators;

public class CreateUserActiveStatusRequestValidator : AbstractValidator<CreateUserActiveStatusRequest>
{
  public CreateUserActiveStatusRequestValidator()
  {
    RuleFor(x => x.UserId).NotEmpty();
    RuleFor(x => x.UserStatusId).NotEmpty();
  }
}

public class UpdateUserActiveStatusRequestValidator : AbstractValidator<UpdateUserActiveStatusRequest>
{
  public UpdateUserActiveStatusRequestValidator()
  {
    RuleFor(x => x.Id).GreaterThan(0).WithMessage("'Id' is required and must be greater than zero.");
    RuleFor(x => x.UserId).NotEmpty();
    RuleFor(x => x.UserStatusId).NotEmpty();
  }
}