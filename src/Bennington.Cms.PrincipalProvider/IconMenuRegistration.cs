using Bennington.Cms.MenuSystem;

namespace Bennington.Cms.PrincipalProvider
{
    public class MenuSystemConfigurer : IMenuSystemConfigurer
    {
        public void Configure(IMenuRegistry sectionMenuRegistry)
        {
            sectionMenuRegistry.Add(new ActionIconMenuItem("Security Management", "~/Content/Canvas/UserManagementIcon.gif", "Index", "User"));
        }
    }
}
