using AutoMapper;
using IMS.Application.Features.Applications.Queries;
using IMS.Application.Features.Customers.Queries;
using IMS.Application.Features.InternetPlans.Queries;
using IMS.Application.Features.Modems.Queries;
using IMS.Application.Features.PaymentMethods.Queries;
using IMS.Application.Features.Users.Queries;
using IMS.Domain.Entities;

namespace IMS.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();

            CreateMap<Domain.Entities.Application, ApplicationDto>();

            CreateMap<Domain.Entities.Application, Customer>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.ApplicationId, opt => opt.MapFrom(src => src.Id));

            CreateMap<Customer, CustomerDto>();

            CreateMap<InternetPlan, InternetPlanDto>();

            CreateMap<Modem, ModemDto>();

            CreateMap<PaymentMethod, PaymentMethodDto>();
        }
    }
}
 