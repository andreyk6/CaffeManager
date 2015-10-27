using Microsoft.Owin.Security.OAuth;
using System.Threading.Tasks;
using System;
using System.Security.Claims;
using CaffeManagerServer.Context;
using System.Linq;
using CaffeManagerServer.Enitites;

namespace CaffeManagerServer
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            User user;

            using (CaffeDbContext _db = new CaffeDbContext())
            {
                user = _db.GetUserByName(context.UserName);

                if (user == null || context.Password != user.Password)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Login));
            identity.AddClaim(new Claim(ClaimTypes.Role, user.Role.ToString()));

            context.Validated(identity);
        }
    }
}