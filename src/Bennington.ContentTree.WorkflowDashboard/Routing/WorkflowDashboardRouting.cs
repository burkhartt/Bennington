using System.Web.Mvc;
using System.Web.Routing;
using Bennington.ContentTree.WorkflowDashboard.Controllers;
using MvcTurbine.Routing;

namespace Bennington.ContentTree.WorkflowDashboard.Routing
{
    public class WorkflowDashboardRouting : IRouteRegistrator
    {
        public void Register(RouteCollection routes)
        {
            routes.MapRoute("WorkflowDashboard", "WorkflowDashboard/{action}", new { controller = typeof(WorkflowDashboardController).Name.Replace("Controller", string.Empty), action = "Index" });
        }
    }
}
