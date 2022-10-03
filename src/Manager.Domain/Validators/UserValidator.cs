using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manager.Domain.Entities;
using FluentValidation;

namespace Manager.Domain.Validators
{
    public class UserValidator : AbstractValidator <User>
    {
        public UserValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .NotNull().WithMessage("Name is required")
                .MinimumLength(3).WithMessage("Name must be greater than 3 characters")
                .MaximumLength(80).WithMessage("Name must be less than 80 characters");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .NotNull().WithMessage("Email is required")
                .MinimumLength(10).WithMessage("Name must be greater than 10 characters")
                .MaximumLength(80).WithMessage("Name must be less than 80 characters")
                .Matches(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$").
                WithMessage("Email is invalid");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .NotNull().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be greater than 6 characters");
        }
    }
}