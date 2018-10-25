using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using MediatR;
using AutoMapper;

using Demo1.DAL;
using System.Threading;
using Demo1.BusinessEntity;

namespace Demo1.BL.Features.Users.Queries
{
    public class GetExternalUserById : IRequest<ExternalUserProfile>
    {
        public string Id { get; set; }
    }

    //public class GetExternalUserByIdModel
    //{
    //    public string ObjectId { get; set; }
    //    public bool? AccountEnabled { get; set; }
    //    public string DisplayName { get; set; }
    //    public string GivenName { get; set; }
    //    public string Mail { get; set; }
    //    public string MailNickname { get; set; }
    //    public string Surname { get; set; }
    //    public string UserPrincipalName { get; set; }
    //}

    public class GetExternalUserByIdQueryHandler : IRequestHandler<GetExternalUserById, ExternalUserProfile>
    {
        IExternalUserRepository _externalUserRepository;

        public GetExternalUserByIdQueryHandler(IExternalUserRepository externalUserRepository)
        {
            _externalUserRepository = externalUserRepository;
        }

        public async Task<ExternalUserProfile> Handle(GetExternalUserById message, CancellationToken cancellationToken)
        {
            var user = await _externalUserRepository.GetByIdAsync(message.Id);
            //return await Task.FromResult(Mapper.Map<GetExternalUserByIdModel>(user));
            return await Task.FromResult(user);
        }
    }
}
