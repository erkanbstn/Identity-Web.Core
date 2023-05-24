using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace IdentityUI.Core.Requirements
{
    public class ExchangeExpireRequirement : IAuthorizationRequirement
    {
    }
    public class ExchangeExpireRequirementHandler : AuthorizationHandler<ExchangeExpireRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ExchangeExpireRequirement requirement)
        {

            if (!context.User.HasClaim(x => x.Type == "ExchangeExpireDate"))
            {
                context.Fail();
                return Task.CompletedTask;
            }
            Claim exchangeExpireClaim = context.User.FindFirst("ExchangeExpireDate");
            if (DateTime.Now > Convert.ToDateTime(exchangeExpireClaim.Value))
            {
                context.Fail();
                return Task.CompletedTask;
            }
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
