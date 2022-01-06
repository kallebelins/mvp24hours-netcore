using FluentValidation;
using Mvp24Hours.Patterns.Test.Support.Entities;

namespace Mvp24Hours.Patterns.Test.Support.Validations
{
    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Customer {PropertyName} is required.");
        }
    }
}
