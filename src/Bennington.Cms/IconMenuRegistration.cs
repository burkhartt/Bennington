using System.Web;
using Bennington.Cms.MenuSystem;

namespace Bennington.Cms
{
    public class MenuSystemConfigurer : IMenuSystemConfigurer
    {
        public void Configure(IMenuRegistry sectionMenuRegistry)
        {
            sectionMenuRegistry.Add(new UrlIconMenuItem("Sign Out",
                                                        HttpContext.Current.Request.Url.AbsoluteUri +
                                                        QuestionMarkOrAmpersand() + "Logout=Logout",
                                                        "~/Content/Canvas/lock.png"));
        }

        private static string QuestionMarkOrAmpersand()
        {
            return HttpContext.Current.Request.Url.AbsoluteUri.Contains("?") ? "&" : "?";
        }
    }    
}
