using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Bennington.Cms.MenuSystem;
using Bennington.Cms.Models.MenuSystem;

namespace Bennington.Cms.PrincipalProvider.MenuSystem
{
    public class GenericSubMenuItem : ISubMenuItem
    {
        private readonly string name;
        private readonly string actionName;
        private readonly string controllerName;
        private readonly string[] selectedActions;
        private readonly RouteValueDictionary routeValues;

        public GenericSubMenuItem(string name, string actionName, string controllerName, params string[] selectedActions)
            : this(name, actionName, controllerName, null, selectedActions)
        {
        }

        public GenericSubMenuItem(string name, string actionName, string controllerName, object routeValues, params string[] selectedActions)
        {
            this.name = name;
            this.actionName = actionName;
            this.controllerName = controllerName;
            this.selectedActions = selectedActions;
            this.routeValues = new RouteValueDictionary(routeValues);
        }

        public virtual SubMenuItemViewModel GetViewModel(ControllerContext controllerContext)
        {
            var urlHelper = new UrlHelper(controllerContext.RequestContext);
            var routeData = GetRootRouteData(controllerContext);
            return new SubMenuItemViewModel
                       {
                           Name = name,
                           Url = urlHelper.Action(actionName, controllerName, routeValues),
                           Selected = routeData.GetRequiredString("controller") == controllerName && (routeData.GetRequiredString("action") == actionName || selectedActions.Contains(routeData.GetRequiredString("action"))),
                           Visible = routeData.GetRequiredString("controller") == controllerName
                       };
        }

        private static RouteData GetRootRouteData(ControllerContext controllerContext)
        {
            return controllerContext.IsChildAction ? GetRootRouteData(controllerContext.ParentActionViewContext) : controllerContext.RouteData;
        }
    }
}