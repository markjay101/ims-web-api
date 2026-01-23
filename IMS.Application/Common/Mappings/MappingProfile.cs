using AutoMapper;
using IMS.Application.Features.Users.Queries;
using IMS.Domain.Entities;

namespace IMS.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
        }
    }
}
