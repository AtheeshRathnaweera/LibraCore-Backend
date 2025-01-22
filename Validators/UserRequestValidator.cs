using FluentValidation;
using LibraCore.Backend.DTOs.User;
using LibraCore.Backend.Utilities;

namespace LibraCore.Backend.Validators;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
  public CreateUserRequestValidator()
  {
    RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
    RuleFor(x => x.AddressOne).NotEmpty().MaximumLength(255);
    RuleFor(x => x.City).NotEmpty().MaximumLength(100);
    RuleFor(x => x.District).NotEmpty().MaximumLength(100);
    RuleFor(x => x.Email).NotEmpty().EmailAddress();
    RuleFor(x => x.PhoneNumber).NotEmpty().Matches(@"^\+?[1-9]\d{1,14}$");
    RuleFor(x => x.DateOfBirth)
      .NotEmpty()
      .Must(CommonUtils.BeAValidDate).WithMessage("Date of birth must be a valid date.")
      .LessThan(DateTime.Now).WithMessage("Date of birth must be in the past.");
  }
}

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
  public UpdateUserRequestValidator()
  {
    RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
    RuleFor(x => x.AddressOne).NotEmpty().MaximumLength(255);
    RuleFor(x => x.City).NotEmpty().MaximumLength(100);
    RuleFor(x => x.District).NotEmpty().MaximumLength(100);
    RuleFor(x => x.Email).NotEmpty().EmailAddress();
    RuleFor(x => x.PhoneNumber).NotEmpty().Matches(@"^\+?[1-9]\d{1,14}$");
    RuleFor(x => x.DateOfBirth)
      .NotEmpty()
      .Must(CommonUtils.BeAValidDate).WithMessage("Date of birth must be a valid date.")
      .LessThan(DateTime.Now).WithMessage("Date of birth must be in the past.");
  }
}