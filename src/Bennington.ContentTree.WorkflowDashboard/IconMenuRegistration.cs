﻿using Bennington.Cms.MenuSystem;

namespace Bennington.Cms.PrincipalProvider
{
    public class MenuSystemConfigurer : IMenuSystemConfigurer
    {
        public void Configure(IMenuRegistry sectionMenuRegistry)
        {
            sectionMenuRegistry.Add(new ActionIconMenuItem("Workflow Dashboard", "~/Content/Canvas/UserManagementIcon.gif", "Index", "WorkflowDashboard"));
        }
    }
}
