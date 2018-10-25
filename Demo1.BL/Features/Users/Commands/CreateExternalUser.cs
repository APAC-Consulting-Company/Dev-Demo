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
    public class CreateExternalUser : IRequest<bool>
    {
        public ExternalUserProfile ExternalUserProfile { get; set; }
    }

    public class CreateExternalUserHandler : IRequestHandler<CreateExternalUser, bool>
    {
        IExternalUserRepository _externalUserRepository;

        public CreateExternalUserHandler(IExternalUserRepository externalUserRepository)
        {
            _externalUserRepository = externalUserRepository;
        }

        public async Task<bool> Handle(CreateExternalUser message, CancellationToken cancellationToken)
        {
            await _externalUserRepository.CreateUserAsync(message.ExternalUserProfile);
            return await Task.FromResult(true);
        }
    }
}
