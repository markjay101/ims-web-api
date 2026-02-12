using FluentValidation;

namespace IMS.Application.Features.Customers.Queries.GetCustomerById
{
    internal class GetCustomerByIdQueryValidator : AbstractValidator<GetCustomerByIdQuery>
    {
        public GetCustomerByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .Must(ShouldBeValidId).WithMessage("Id is not a valid customer Id.");
        }

        private bool ShouldBeValidId(string id)
        {
            return Guid.TryParse(id, out _);
        }
    }
}
