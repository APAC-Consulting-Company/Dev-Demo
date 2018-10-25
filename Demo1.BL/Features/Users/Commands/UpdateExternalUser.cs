using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

using MediatR;
using AutoMapper;

using Demo1.DAL;
using Demo1.BusinessEntity;

namespace Demo1.BL.Features.Users.Commands
{
    public class UpdateExternalUser : IRequest<bool>
    {
        public string Id { get; set; }
        public ExternalUserProfile ExternalUserProfile { get; set; }
    }

    public class UpdateExternalUserHandler : IRequestHandler<UpdateExternalUser, bool>
    {
        IExternalUserRepository _externalUserRepository;

        public UpdateExternalUserHandler(IExternalUserRepository externalUserRepository)
        {
            _externalUserRepository = externalUserRepository;
        }

        public async Task<bool> Handle(UpdateExternalUser message, CancellationToken cancellationToken)
        {
            await _externalUserRepository.UpdateUserAsync(message.Id, message.ExternalUserProfile);
            return await Task.FromResult(true);
        }
    }
}
