using IMS.Application.Common.Validators;

namespace IMS.Application.Features.Customers.Queries.GetCustomerById
{
    internal class GetCustomerByIdQueryValidator : BaseValidator<GetCustomerByIdQuery>
    {
        public GetCustomerByIdQueryValidator()
        {
            RuleForId(x => x.Id);
        }
    }
}
