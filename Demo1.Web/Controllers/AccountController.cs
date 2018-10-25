using System;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security;
using MediatR;

using Demo1.BL.Features.Users.Commands;
using Demo1.BusinessEntity;

namespace Demo1.Controllers
{
    public class AccountController : Controller
    {
        IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        public async Task<ActionResult> Details(string id)
        {
            var user = await _mediator.Send(new BL.Features.Users.Queries.GetExternalUserById() { Id = id });
            return View(user);
        }

        public async Task<ActionResult> New(CreateExternalUser command)
        {
            if (command != null && command.ExternalUserProfile != null)
            {
                await _mediator.Send(command);
                return Redirect("Home/Index");
            }
            return View();
        }

        public async Task<ActionResult> Edit(UpdateExternalUser command)
        {
            if (command != null && command.Id != "" && command.ExternalUserProfile != null)
            {
                await _mediator.Send(command);
                return Redirect("Home/Index");
            }

            var user = await _mediator.Send(new BL.Features.Users.Queries.GetExternalUserById() { Id = command?.Id });
            return View(new UpdateExternalUser { Id = user.UserId, ExternalUserProfile = user });
        }

        public async Task<ActionResult> Delete(string id)
        {
            await _mediator.Send(new BL.Features.Users.Commands.DeleteExternalUser() { Id = id });
            return Redirect("Home/Index");
        }

        public void SignIn()
        {
            // Send an OpenID Connect sign-in request.
            if (!Request.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = "/" },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
        }

        public void SignOut()
        {
            string callbackUrl = Url.Action("SignOutCallback", "Account", routeValues: null, protocol: Request.Url.Scheme);

            HttpContext.GetOwinContext().Authentication.SignOut(
                new AuthenticationProperties { RedirectUri = callbackUrl },
                OpenIdConnectAuthenticationDefaults.AuthenticationType, CookieAuthenticationDefaults.AuthenticationType);
        }

        public ActionResult SignOutCallback()
        {
            if (Request.IsAuthenticated)
            {
                // Redirect to home page if the user is authenticated.
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
    }
}
