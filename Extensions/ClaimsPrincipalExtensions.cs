using System.Security.Claims;

namespace TutorBridge.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string? FullName(this ClaimsPrincipal user)
        {
            return user.FindFirst("FullName")?.Value;
        }
        public static string? FirstName(this ClaimsPrincipal user)
        {
            return user.FindFirst("FirstName")?.Value;
        }
        public static string? LastName(this ClaimsPrincipal user)
        {
            return user.FindFirst("LastName")?.Value;
        }
    }
}