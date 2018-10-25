using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Demo1.BusinessEntity;

namespace Demo1.DAL
{
    public interface IExternalUserRepository
    {
        Task<IReadOnlyCollection<ExternalUserProfile>> GetAll();
        Task<ExternalUserProfile> GetByIdAsync(string id);
        Task CreateUserAsync(ExternalUserProfile userProfileNew);
        Task UpdateUserAsync(string id, ExternalUserProfile userProfileUpdate);
        Task DeleteAsync(string id);
    }
}
