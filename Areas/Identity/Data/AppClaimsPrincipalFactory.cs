using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace TutorBridge.Areas.Identity.Data
{
    public class AppClaimsPrincipalFactory : UserClaimsPrincipalFactory<User, IdentityRole>
    {
        public AppClaimsPrincipalFactory(UserManager<User> um, RoleManager<IdentityRole> rm, IOptions<IdentityOptions> o)
            : base(um, rm, o) { }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("FirstName", $"{user.NameFirst}"));
            identity.AddClaim(new Claim("LastName", $"{user.NameLast}"));
            identity.AddClaim(new Claim("FullName", $"{user.NameFirst} {user.NameLast}"));
            return identity;
        }
    }
}
