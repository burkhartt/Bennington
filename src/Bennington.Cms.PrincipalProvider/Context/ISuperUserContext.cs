using System.Web;

namespace Bennington.Cms.PrincipalProvider.Context
{
    public interface ISuperUserContext
    {
        bool IsSuperUser();
    }

    public class SuperUserContext : ISuperUserContext
    {
        public bool IsSuperUser()
        {
            return string.Equals(HttpContext.Current.User.Identity.Name, "admin");
        }
    }
}