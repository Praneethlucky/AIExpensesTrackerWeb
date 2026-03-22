using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers
{
    using System.Security.Claims;

    public abstract class BaseController : ControllerBase
    {
        protected Int32 UserId =>
            Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }
}
