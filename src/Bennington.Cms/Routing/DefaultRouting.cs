﻿using System.Web.Mvc;
using System.Web.Routing;
using MvcTurbine.Routing;

namespace Bennington.Cms.Routing
{
    public class DefaultRouting : IRouteRegistrator
    {
        public void Register(RouteCollection routes)
        {
            routes.MapRoute(
                null,
                "Manage",
                new {controller = "TreeManager", action = "Index"}
                );
        }
    }
}