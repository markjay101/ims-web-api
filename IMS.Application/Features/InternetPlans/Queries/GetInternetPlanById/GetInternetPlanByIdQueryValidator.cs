using IMS.Application.Common.Validators;

namespace IMS.Application.Features.InternetPlans.Queries.GetInternetPlanById
{
    internal class GetInternetPlanByIdQueryValidator : BaseValidator<GetInternetPlanByIdQuery>
    {
        public GetInternetPlanByIdQueryValidator()
        {
            RuleForId(x => x.Id);
        }
    }
}
