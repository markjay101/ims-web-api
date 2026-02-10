using AutoMapper;
using IMS.Application.Features.Applications.Queries;
using IMS.Application.Features.Customers.Queries;
using IMS.Application.Features.InternetPlans.Queries;
using IMS.Application.Features.Invoices.Queries;
using IMS.Application.Features.Modems.Queries;
using IMS.Application.Features.PaymentMethods.Queries;
using IMS.Application.Features.Payments.Queries;
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

            CreateMap<Customer, CustomerDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Application.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Application.LastName));

            CreateMap<CustomerPlan, CustomerPlanDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.InternetPlan.Name))
                .ForMember(dest => dest.SpeedMbps, opt => opt.MapFrom(src => src.InternetPlan.SpeedMbps))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.InternetPlan.Price))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.InternetPlan.Description));

            CreateMap<InternetPlan, InternetPlanDto>();

            CreateMap<Modem, ModemDto>();

            CreateMap<PaymentMethod, PaymentMethodDto>();

            CreateMap<Invoice, InvoiceDto>()
                .ForMember(dest => dest.IssueDate, opt => opt.MapFrom(src => src.CreatedAt));

            CreateMap<Payment, PaymentDto>();
        }
    }
}
 