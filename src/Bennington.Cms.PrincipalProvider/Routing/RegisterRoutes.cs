using System.Web.Mvc;
using System.Web.Routing;
using MvcTurbine.Routing;

namespace Bennington.Cms.PrincipalProvider.Routing
{
	public class RegisterRoutes : IRouteRegistrator
	{
		public void Register(RouteCollection routes)
		{
			routes.MapRoute(null, "Users/{action}", new { controller = "User", action = "Index" });
            routes.MapRoute(null, "Roles/{action}", new { controller = "Role", action = "Index" });
            routes.MapRoute(null, "Unauthorized/{action}", new { controller = "Unauthorized", action = "Index" });
		}
	}
}