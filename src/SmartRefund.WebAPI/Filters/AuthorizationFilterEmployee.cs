﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SmartRefund.WebAPI.Filters
{
    public class AuthorizationFilterEmployee : IAuthorizationFilter
    {
        private readonly ILogger<AuthorizationFilterEmployee> _logger;

        public AuthorizationFilterEmployee(ILogger<AuthorizationFilterEmployee> logger)
        {
            _logger = logger;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _logger.LogInformation(context.ToString());

            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedObjectResult(new { Message = "Unauthorized access. Authentication is required. Please authenticate at the /api/login route and copy the generated token to paste in the authorization." });
                return;
            }

            var userTypeClaim = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userType")?.Value;

            if (userTypeClaim.Equals("employee"))
                return;

            context.Result = new ForbidResult();
            return;
        }
    }
}
