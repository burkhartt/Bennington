using Bennington.Cms.MenuSystem;

namespace Bennington.Cms.PrincipalProvider.MenuSystem
{
    public class MenuSystemConfigurer : IMenuSystemConfigurer
    {
        public void Configure(IMenuRegistry menuRegistry)
        {
            menuRegistry.Add(new PermissionsSubMenuItem("Users", "Index", "User"));
            menuRegistry.Add(new PermissionsSubMenuItem("Roles", "Index", "Role"));
        }
    }
}