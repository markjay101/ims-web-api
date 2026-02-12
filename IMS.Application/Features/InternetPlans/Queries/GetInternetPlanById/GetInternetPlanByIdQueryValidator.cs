using FluentValidation;

namespace IMS.Application.Features.InternetPlans.Queries.GetInternetPlanById
{
    internal class GetInternetPlanByIdQueryValidator : AbstractValidator<GetInternetPlanByIdQuery>
    {
        public GetInternetPlanByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .Must(ShouldBeValidId).WithMessage("Id is not a valid internet plan Id.");
        }

        private bool ShouldBeValidId(string id)
        {
            return Guid.TryParse(id, out _);
        }
    }
}
