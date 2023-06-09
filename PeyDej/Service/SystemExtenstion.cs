using Microsoft.AspNetCore.Mvc.Rendering;

using System.Linq.Expressions;
using System.Security.Claims;

namespace PeyDej.Service;


public static class SystemExtenstion
{
    public static string? GetUserIdPrincipal(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
    }
    public static SelectList Select<M>(IQueryable<M>? queryable,string dataValueField, string dataTextField, string dataGroupField = null, object selectedValue = null, IEnumerable<Expression<Func<M, bool>>> wheres = null, Expression<Func<M, bool>> where = null) where M : class
    {
        if (wheres is not null)
        {
            foreach (var we in wheres)
            {
                queryable = queryable.Where(we).AsQueryable();
            }
        }
        if (where is not null)
        {
            queryable = queryable.Where(where).AsQueryable();
        }
        return new SelectList(queryable.AsEnumerable(), dataValueField, dataTextField, selectedValue, dataGroupField);
    }
}
