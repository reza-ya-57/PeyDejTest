using System.Security.Claims;

namespace PeyDej.Service;


public static class IdentityExtenstion
{
    public static string? GetUserIdPrincipal(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
    }
}
