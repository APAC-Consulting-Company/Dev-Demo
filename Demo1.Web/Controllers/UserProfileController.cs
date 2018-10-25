using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using MediatR;

namespace Demo1.Controllers
{
    [Authorize]
    public class UserProfileController : Controller
    {
        IMediator _mediator;

        public UserProfileController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        public async Task<ActionResult> Index()
        {
            var objectID = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var user = await _mediator.Send(new BL.Features.Users.Queries.GetExternalUserById() { Id = objectID });
            return View(user);
        }

        public void RefreshSession()
        {
            HttpContext.GetOwinContext().Authentication.Challenge(
                new AuthenticationProperties { RedirectUri = "/UserProfile" },
                OpenIdConnectAuthenticationDefaults.AuthenticationType);
        }
    }
}
