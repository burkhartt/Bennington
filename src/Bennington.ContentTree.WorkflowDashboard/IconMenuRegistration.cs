using Bennington.Cms.MenuSystem;

namespace Bennington.ContentTree.WorkflowDashboard
{
    public class MenuSystemConfigurer : IMenuSystemConfigurer
    {
        public void Configure(IMenuRegistry sectionMenuRegistry)
        {
            sectionMenuRegistry.Add(new ActionIconMenuItem("Workflow Dashboard", "~/Content/Canvas/icondemo.png", "Index", "WorkflowDashboard"));
        }
    }
}
