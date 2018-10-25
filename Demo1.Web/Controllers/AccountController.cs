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

        [Authorize]
        public async Task<ActionResult> Details(string id)
        {
            var userProfile = await _mediator.Send(new BL.Features.Users.Queries.GetExternalUserById() { Id = id });
            return View(userProfile);
        }

        [Authorize]
        public async Task<ActionResult> New(CreateExternalUser command)
        {
            ViewBag.FailedReason = @"";
            if (command != null && command.ExternalUserProfile != null)
            {
                try
                {
                    await _mediator.Send(command);
                    return RedirectToAction("Index", "Home");
                }
                catch(Exception exception)
                {
                    ViewBag.FailedReason = exception.Message;
                }
            }
            return View();
        }

        [Authorize]
        public async Task<ActionResult> Edit(UpdateExternalUser command)
        {
            ViewBag.FailedReason = @"";
            if (command != null && command.Id != "" && command.ExternalUserProfile != null)
            {
                try
                {
                    await _mediator.Send(command);
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception exception)
                {
                    ViewBag.FailedReason = exception.Message;
                }
            }

            var userProfile = await _mediator.Send(new BL.Features.Users.Queries.GetExternalUserById() { Id = command?.Id });
            return View(new UpdateExternalUser { Id = userProfile.UserId, ExternalUserProfile = userProfile });
        }

        [Authorize]
        public async Task<ActionResult> Delete(string id)
        {
            ViewBag.FailedReason = @"";
            try
            {
                await _mediator.Send(new BL.Features.Users.Commands.DeleteExternalUser() { Id = id });
                return RedirectToAction("Index", "Home");
            }
            catch (Exception exception)
            {
                ViewBag.FailedReason = exception.Message;
            }
            return View();
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
