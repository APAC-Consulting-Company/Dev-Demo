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
    public class DeleteExternalUser : IRequest<DeleteExternalUserModel>
    {
        public string Id { get; set; }
    }

    public class DeleteExternalUserModel
    {
    }

    public class DeleteExternalUserHandler : IRequestHandler<DeleteExternalUser, DeleteExternalUserModel>
    {
        IExternalUserRepository _externalUserRepository;

        public DeleteExternalUserHandler(IExternalUserRepository externalUserRepository)
        {
            _externalUserRepository = externalUserRepository;
        }

        public async Task<DeleteExternalUserModel> Handle(DeleteExternalUser message, CancellationToken cancellationToken)
        {
            await _externalUserRepository.DeleteAsync(message.Id);
            return await Task.FromResult(new DeleteExternalUserModel());
        }
    }
}
