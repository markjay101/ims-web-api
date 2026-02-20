using FluentValidation;
using System.Linq.Expressions;

namespace IMS.Application.Common.Validators
{
    public class BaseValidator<T> : AbstractValidator<T>
    {
        protected IRuleBuilderOptions<T, Guid> RuleForId(Expression<Func<T, Guid>> selector, string displayName = "Id")
        {
            return RuleFor(selector)
                .NotEmpty().WithMessage($"{displayName} is required.")
                .NotEqual(Guid.Empty).WithMessage($"{displayName} format is invalid.");
        }

        protected IRuleBuilderOptions<T, string> RuleForRequiredString(Expression<Func<T, string>> selector, string displayName)
        {
            return RuleFor(selector)
                .NotEmpty().WithMessage($"{displayName} is required.");
        }

        protected IRuleBuilderOptions<T, string> RuleForRequiredEmail(Expression<Func<T, string>> selector, string displayName = "Email Address")
        {
            var invalidEmailMessage = $"{displayName} format is invalid.";

            if(!displayName.Equals("Email Address", StringComparison.OrdinalIgnoreCase))
                invalidEmailMessage = $"{displayName} is not valid Email Address format.";

            return RuleFor(selector)
                .NotEmpty().WithMessage($"{displayName} is required.")
                .EmailAddress().WithMessage(invalidEmailMessage);
        }

        protected IRuleBuilderOptions<T, TEnum> RuleForEnum<TEnum>(Expression<Func<T, TEnum>> selector, string displayName) where TEnum : struct, Enum
        {
            return RuleFor(selector)
                .IsInEnum().WithMessage($"{displayName} is not a valid option.");
        }
    }
}
