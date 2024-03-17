using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SmartRefund.WebAPI.Filters
{
    public class AuthorizationFilterFinance : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedObjectResult(new { Message = "Unauthorized access. Authentication is required. Please authenticate at the /api/login route and copy the generated token to paste in the authorization." });
                return;
            }

            var userTypeClaim = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userType")?.Value;

            if (userTypeClaim.Equals("finance"))
                return;

            context.Result = new ForbidResult();
            return;
        }
    }
}
