using System;

namespace Bennington.Cms.PrincipalProvider.Helpers
{
    public interface IGetTheNotAuthorizedPage
    {
        string GetUnauthorizedPage();
    }

    public class GetTheNotAuthorizedPage : IGetTheNotAuthorizedPage
    {
        public string GetUnauthorizedPage()
        {
            return "/Manage/Unauthorized";
        }
    }
}