using FluentValidation;
using IMS.Application.Common.Validators;

namespace IMS.Application.Features.Applications.Queries.GetApplicationById
{
    internal class GetApplicationByIdQueryValidator : BaseValidator<GetApplicationByIdQuery>
    {
        public GetApplicationByIdQueryValidator()
        {
            RuleForId(x => x.Id);
        }
    }
}
