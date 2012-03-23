using System.Web.Mvc;
using System.Web.Routing;
using Bennington.Cms.Models.MenuSystem;
using System.Linq;
using System.Collections.Generic;

namespace Bennington.Cms.PrincipalProvider.MenuSystem
{
    public class PermissionsSubMenuItem : GenericSubMenuItem
    {
        private readonly List<string> controllersNames = new List<string>(new[] { "User", "Role" }.Select(cn => cn.ToLower()));

        public PermissionsSubMenuItem(string name, string actionName, string controllerName, params string[] selectedActions) : base(name, actionName, controllerName, selectedActions)
        {
        }

        public PermissionsSubMenuItem(string name, string actionName, string controllerName, object routeValues, params string[] selectedActions) : base(name, actionName, controllerName, routeValues, selectedActions)
        {
        }

        public override SubMenuItemViewModel GetViewModel(ControllerContext controllerContext)
        {
            var viewModel = base.GetViewModel(controllerContext);
            var controllerName = GetRootRouteData(controllerContext).GetRequiredString("controller");
            viewModel.Visible = controllersNames.Contains(controllerName.ToLower());

            return viewModel;
        }

        private static RouteData GetRootRouteData(ControllerContext controllerContext)
        {
            return controllerContext.IsChildAction ? GetRootRouteData(controllerContext.ParentActionViewContext) : controllerContext.RouteData;
        }
    }
}