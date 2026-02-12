using FluentValidation;

namespace IMS.Application.Features.Applications.Queries.GetApplicationById
{
    internal class GetApplicationByIdQueryValidator : AbstractValidator<GetApplicationByIdQuery>
    {
        public GetApplicationByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .Must(ShouldBeValidId).WithMessage("Id is not a valid application Id.");
        }

        private bool ShouldBeValidId(string id)
        {
            return Guid.TryParse(id, out _);
        }
    }
}
