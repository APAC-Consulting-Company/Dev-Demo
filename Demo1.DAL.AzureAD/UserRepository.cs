using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Azure.ActiveDirectory.GraphClient;

using Demo1.BusinessEntity;
using Demo1.DAL.AzureAD.Cache;

namespace Demo1.DAL.AzureAD
{
    public class UserRepository : IExternalUserRepository
    {
        const string AZURE_GRAPH_API_RESOURCE_URI = "https://graph.windows.net";

        string _clientId;
        string _appSecret;
        string _aadInstance;
        string _tenantID;

        public UserRepository(string clientId, string appSecret, string aadInstance)
        {
            _clientId = clientId;
            _appSecret = appSecret;
            _aadInstance = aadInstance;
            _tenantID = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
        }

        public async Task<IReadOnlyCollection<ExternalUserProfile>> GetAll()
        {
            Uri servicePointUri = new Uri(AZURE_GRAPH_API_RESOURCE_URI);
            Uri serviceRoot = new Uri(servicePointUri, this._tenantID);
            ActiveDirectoryClient activeDirectoryClient = new ActiveDirectoryClient(
                serviceRoot,
                async () => await GetTokenForApplication());
            
            var pagedResult = await activeDirectoryClient.Users.ExecuteAsync();
            
            // Process returned result.
            List<ExternalUserProfile> result = new List<ExternalUserProfile>();
            {
                result.AddRange(pagedResult.CurrentPage
                    .Select(u => new ExternalUserProfile()
                    {
                        AccountEnabled = u.AccountEnabled,
                        FullName = u.DisplayName,
                        FirstName = u.GivenName,
                        Mail = u.Mail,
                        MailNickname = u.MailNickname,
                        LastName = u.Surname,
                        UserPrincipalName = u.UserPrincipalName,
                        UserId = u.ObjectId
                    })
                    .ToList());

                if (pagedResult.MorePagesAvailable)
                    pagedResult = await pagedResult.GetNextPageAsync();
            } while (pagedResult.MorePagesAvailable) ;

            return await Task.FromResult(result);
        }

        public async Task<ExternalUserProfile> GetByIdAsync(string id)
        {
            Uri servicePointUri = new Uri(AZURE_GRAPH_API_RESOURCE_URI);
            Uri serviceRoot = new Uri(servicePointUri, this._tenantID);
            ActiveDirectoryClient activeDirectoryClient = new ActiveDirectoryClient(
                serviceRoot,
                async () => await GetTokenForApplication());

            var pagedResult = await activeDirectoryClient.Users
                .Where(user => user.ObjectId == id)
                .ExecuteAsync();

            // Process returned result.
            var result = new ExternalUserProfile();
            var userProfile = pagedResult.CurrentPage.FirstOrDefault();
            if (userProfile != null)
            {
                result.AccountEnabled = userProfile.AccountEnabled;
                result.FullName = userProfile.DisplayName;
                result.FirstName = userProfile.GivenName;
                result.Mail = userProfile.Mail;
                result.MailNickname = userProfile.MailNickname;
                result.LastName = userProfile.Surname;
                result.UserPrincipalName = userProfile.UserPrincipalName;
                result.UserId = userProfile.ObjectId;
            }

            return await Task.FromResult(result);
        }

        public async Task CreateUserAsync(ExternalUserProfile userProfileNew)
        {
            Uri servicePointUri = new Uri(AZURE_GRAPH_API_RESOURCE_URI);
            Uri serviceRoot = new Uri(servicePointUri, this._tenantID);
            ActiveDirectoryClient activeDirectoryClient = new ActiveDirectoryClient(
                serviceRoot,
                async () => await GetTokenForApplication());

            IUser userProfile = new Microsoft.Azure.ActiveDirectory.GraphClient.User
            {
                AccountEnabled = true,
                DisplayName = userProfileNew.FullName,
                MailNickname = userProfileNew.MailNickname,
                UserPrincipalName = userProfileNew.UserPrincipalName,
                PasswordProfile = new Microsoft.Azure.ActiveDirectory.GraphClient.PasswordProfile
                {
                    ForceChangePasswordNextLogin = true,
                    Password = "Pass12345@"
                }
            };
            await activeDirectoryClient.Users.AddUserAsync(userProfile);
        }

        public async Task UpdateUserAsync(string id, ExternalUserProfile userProfileUpdate)
        {
            Uri servicePointUri = new Uri(AZURE_GRAPH_API_RESOURCE_URI);
            Uri serviceRoot = new Uri(servicePointUri, this._tenantID);
            ActiveDirectoryClient activeDirectoryClient = new ActiveDirectoryClient(
                serviceRoot,
                async () => await GetTokenForApplication());

            var pagedResult = await activeDirectoryClient.Users
                .Where(user => user.ObjectId == id)
                .ExecuteAsync();

            // Process returned result.
            var result = new ExternalUserProfile();
            var userProfile = pagedResult.CurrentPage.FirstOrDefault();
            if (userProfile != null)
            {
                userProfile.DisplayName = userProfileUpdate.FullName;
                userProfile.GivenName = userProfileUpdate.FirstName;
                userProfile.Surname = userProfileUpdate.LastName;
                await userProfile.UpdateAsync();
            }
        }

        public async Task DeleteAsync(string id)
        {
            Uri servicePointUri = new Uri(AZURE_GRAPH_API_RESOURCE_URI);
            Uri serviceRoot = new Uri(servicePointUri, this._tenantID);
            ActiveDirectoryClient activeDirectoryClient = new ActiveDirectoryClient(
                serviceRoot,
                async () => await GetTokenForApplication());

            var pagedResult = await activeDirectoryClient.Users
                .Where(user => user.ObjectId == id)
                .ExecuteAsync();

            // Process returned result.
            var result = new ExternalUserProfile();
            var userProfile = pagedResult.CurrentPage.FirstOrDefault();
            if (userProfile != null)
            {
                await userProfile.DeleteAsync();
            }
        }

        public async Task<string> GetTokenForApplication()
        {
            string signedInUserID = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;
            string tenantID = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
            string userObjectID = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            // get a token for the Graph without triggering any user interaction (from the cache, via multi-resource refresh token, etc)
            ClientCredential clientcred = new ClientCredential(this._clientId, this._appSecret);
            // initialize AuthenticationContext with the token cache of the currently signed in user, as kept in the app's database
            AuthenticationContext authenticationContext = new AuthenticationContext(this._aadInstance + tenantID, new ADALTokenCache(signedInUserID));
            AuthenticationResult authenticationResult = await authenticationContext.AcquireTokenSilentAsync(AZURE_GRAPH_API_RESOURCE_URI, clientcred, new UserIdentifier(userObjectID, UserIdentifierType.UniqueId));
            return authenticationResult.AccessToken;
        }
    }
}
