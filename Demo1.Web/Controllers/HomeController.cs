using System;
using System.Threading.Tasks;
using System.Web.Mvc;

using MediatR;

namespace Demo1.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        IMediator _mediator;

        public HomeController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        public async Task<ActionResult> Index()
        {
            var users = await _mediator.Send(new BL.Features.Users.Queries.GetExternalUsers() { });
            return View(users);
        }
    }
}