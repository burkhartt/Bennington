using System;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using Bennington.Cms.PrincipalProvider.Context;
using Bennington.Cms.PrincipalProvider.Helpers;
using Bennington.Cms.PrincipalProvider.Models;
using Bennington.Cms.PrincipalProvider.Repositories;
using Bennington.Core;

namespace Bennington.Cms.PrincipalProvider.SecurityHandlers
{
    public interface IHandleModuleSecurity
    {
        void HandleSecurityForThisRequest(HttpApplication app, EventArgs eventArgs);
    }

    public class HandleModuleSecurity : IHandleModuleSecurity
    {
        private readonly IUserRepository userRepository;
        private readonly IRoleRepository roleRepository;
        private readonly ICurrentUserContext currentUserContext;
        private readonly IModuleRepository moduleRepository;
        private readonly IGetTheNotAuthorizedPage getTheNotAuthorizedPage;
        private readonly ISuperUserContext superUserContext;

        public HandleModuleSecurity(IUserRepository userRepository,
            IRoleRepository roleRepository,
            ICurrentUserContext currentUserContext,
            IModuleRepository moduleRepository,
            IGetTheNotAuthorizedPage getTheNotAuthorizedPage,
            ISuperUserContext superUserContext)
        {
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.currentUserContext = currentUserContext;
            this.moduleRepository = moduleRepository;
            this.getTheNotAuthorizedPage = getTheNotAuthorizedPage;
            this.superUserContext = superUserContext;
        }

        public void HandleSecurityForThisRequest(HttpApplication app, EventArgs eventArgs)
        {
            var routeData = app.Request.RequestContext.RouteData;
            var controller = (routeData.Values["controller"] ?? string.Empty).ToString();
            var currentPrincipal = currentUserContext.GetCurrentPrincipal();

            if (currentPrincipal != null && superUserContext.IsSuperUser()) return;

            if (TheControllerRequiresAuthentication(controller))
            {
                if (TheUserIsLoggedIn(currentPrincipal))
                {
                    if (currentPrincipal != null)
                    {
                        var user = userRepository.GetAll().FirstOrDefault(x => x.Username == currentPrincipal.Identity.Name);

                        if (TheUserIsSomehowNotInTheRepository(user))
                        {
                            RedirectToUnauthorizedPage(app);
                        }
                        else
                        {
                            var role = roleRepository.GetById(user.Role);

                            if (TheRoleIsNotDefined(role) || TheRoleIsInactive(role))
                            {
                                RedirectToUnauthorizedPage(app);
                            }
                            else
                            {
                                if (TheUserHasAccessToEverything(role)) return;

                                if (TheRoleDoesNotHaveAccessToThisController(controller, role))
                                    RedirectToUnauthorizedPage(app);
                            }
                        }
                    }
                    else
                    {
                        RedirectToUnauthorizedPage(app);
                    }
                }
                else
                {
                    RedirectToUnauthorizedPage(app);
                }
            }
        }

        private static bool TheRoleIsInactive(Role role)
        {
            return role.Inactive;
        }

        private static bool TheUserHasAccessToEverything(Role role)
        {
            return role.AllContent;
        }

        private static bool TheRoleDoesNotHaveAccessToThisController(string controller, Role role)
        {
            return role.AvailableModules.Contains(controller) == false;
        }

        private static bool TheRoleIsNotDefined(Role role)
        {
            return role == null;
        }

        private static bool TheUserIsSomehowNotInTheRepository(User user)
        {
            return user == null;
        }

        private static bool TheUserIsLoggedIn(IPrincipal currentPrincipal)
        {
            return currentPrincipal != null && string.IsNullOrEmpty(currentPrincipal.Identity.Name) == false;
        }

        private bool TheControllerRequiresAuthentication(string controller)
        {
            return moduleRepository.GetAll().Any(x => x.Id == controller);
        }

        private void RedirectToUnauthorizedPage(HttpApplication app)
        {
            app.Response.Redirect(getTheNotAuthorizedPage.GetUnauthorizedPage());
        }
    }
}