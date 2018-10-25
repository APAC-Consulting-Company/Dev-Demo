using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

using MediatR;
using AutoMapper;

using Demo1.DAL;
using Demo1.BusinessEntity;

namespace Demo1.BL.Features.Users.Queries
{
    public class GetExternalUsers : IRequest<IReadOnlyCollection<ExternalUserProfile>>
    {
    }

    //public class GetExternalUsersModel
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

    public class GetExternalUsersQueryHandler : IRequestHandler<GetExternalUsers, IReadOnlyCollection<ExternalUserProfile>>
    {
        IExternalUserRepository _externalUserRepository;

        public GetExternalUsersQueryHandler(IExternalUserRepository externalUserRepository)
        {
            _externalUserRepository = externalUserRepository;
        }

        public async Task<IReadOnlyCollection<ExternalUserProfile>> Handle(GetExternalUsers message, CancellationToken cancellationToken)
        {
            var users = await _externalUserRepository.GetAll();
            //return await Task.FromResult(Mapper.Map<IReadOnlyCollection<GetExternalUsersModel>>(users));
            return await Task.FromResult(users);
        }
    }
}
