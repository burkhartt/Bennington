using System;
using System.Web;
using Bennington.Cms.PrincipalProvider.SecurityHandlers;

namespace Bennington.Cms.PrincipalProvider.HttpModules
{
    public class RoleAuthenticationHttpModule : IHttpModule
    {
        private readonly IHandleModuleSecurity handleModuleSecurity;
        private readonly IHandleContentSecurity handleContentSecurity;

        public RoleAuthenticationHttpModule(IHandleModuleSecurity handleModuleSecurity,
            IHandleContentSecurity handleContentSecurity)
        {
            this.handleModuleSecurity = handleModuleSecurity;
            this.handleContentSecurity = handleContentSecurity;
        }

        public void Init(HttpApplication context)
        {
            context.PostAcquireRequestState += new EventHandler(CheckAccessPermissionsForUserOnThisPage);
        }

        private void CheckAccessPermissionsForUserOnThisPage(object sender, EventArgs e)
        {
            var app = (HttpApplication) sender;
            handleModuleSecurity.HandleSecurityForThisRequest(app, e);
            handleContentSecurity.HandleSecurityForThisPage(app, e);
        }

        public void Dispose()
        {
            
        }
    }
}