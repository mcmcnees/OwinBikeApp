using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace BikeMgrWeb
{
	public static class Extensions
	{
		public static bool HasPermission(this IIdentity identity, string permission)
		{
			var claimsIdentity = identity as ClaimsIdentity;
			if (claimsIdentity != null)
			{
				var claims = claimsIdentity.Claims;
				return claims.Any(c => string.Equals(c.Value, permission, StringComparison.OrdinalIgnoreCase));
			}
			return false;
		}
	}
}