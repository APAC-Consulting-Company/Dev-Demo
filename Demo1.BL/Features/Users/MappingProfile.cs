using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using Demo1.BusinessEntity;

namespace Demo1.BL.Features.Users
{
    public class MappingProfile : Profile
    {
        protected override void Configure()
        {
            //CreateMap<ExternalUserProfile, Queries.GetExternalUsersModel>()
            //    .ForMember(dest => dest.ObjectId, opt => opt.MapFrom(src => src.UserId))
            //    .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.FullName))
            //    .ForMember(dest => dest.GivenName, opt => opt.MapFrom(src => src.FirstName))
            //    .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.LastName));

            //CreateMap<ExternalUserProfile, Queries.GetExternalUserByIdModel>()
            //    .ForMember(dest => dest.ObjectId, opt => opt.MapFrom(src => src.UserId))
            //    .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.FullName))
            //    .ForMember(dest => dest.GivenName, opt => opt.MapFrom(src => src.FirstName))
            //    .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.LastName)); ;
        }
    }
}
