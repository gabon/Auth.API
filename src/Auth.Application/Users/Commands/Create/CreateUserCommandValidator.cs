﻿using FluentValidation;
using FluentValidation.Results;

namespace Auth.Application.Users.Commands.Create
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(v => v.UserName)
                .NotNull()
                .NotEmpty()
                .MaximumLength(200);
        }
        public override ValidationResult Validate(ValidationContext<CreateUserCommand> context)
        {
            var validate = base.Validate(context);
            if (validate.IsValid)
            {
                context.InstanceToValidate.UserName = context.InstanceToValidate.UserName.ToLowerInvariant();
            }
            return validate;
        }
    }
}