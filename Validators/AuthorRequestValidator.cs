using FluentValidation;
using LibraCore.Backend.DTOs.Role;
using LibraCore.Backend.Utilities;

namespace LibraCore.Backend.Validators;

public class CreateAuthorRequestValidator : AbstractValidator<CreateAuthorRequest>
{
  public CreateAuthorRequestValidator()
  {
    RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
    RuleFor(x => x.DateOfBirth)
      .Must(CommonUtils.BeAValidDate)
        .When(x => x.DateOfBirth != default).WithMessage("Date of birth must be a valid date.")
      .LessThan(DateTime.Now).When(x => x.DateOfBirth != default).WithMessage("Date of birth must be in the past.");
    RuleFor(x => x.DateOfDeath)
      .Must(CommonUtils.BeAValidDate)
        .When(x => x.DateOfDeath != default).WithMessage("Date of death must be a valid date.")
      .LessThan(DateTime.Now).When(x => x.DateOfDeath != default).WithMessage("Date of death must be in the past.");
  }
}

public class UpdateAuthorRequestValidator : AbstractValidator<UpdateAuthorRequest>
{
  public UpdateAuthorRequestValidator()
  {
    RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
    RuleFor(x => x.DateOfBirth)
      .Must(CommonUtils.BeAValidDate)
        .When(x => x.DateOfBirth != default(DateTime) && !string.IsNullOrEmpty(x.DateOfBirth.ToString()))
        .WithMessage("Date of birth must be a valid date.")
      .LessThan(DateTime.Now).When(x => x.DateOfBirth != default).WithMessage("Date of birth must be in the past.");
    RuleFor(x => x.DateOfDeath)
      .Must(CommonUtils.BeAValidDate)
        .When(x => x.DateOfDeath != default).WithMessage("Date of death must be a valid date.")
      .LessThan(DateTime.Now).When(x => x.DateOfDeath != default).WithMessage("Date of death must be in the past.");
  }
}