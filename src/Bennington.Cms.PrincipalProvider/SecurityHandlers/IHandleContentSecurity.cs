using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Bennington.Cms.PrincipalProvider.Helpers;
using Bennington.Cms.PrincipalProvider.Models;
using Bennington.Cms.PrincipalProvider.Repositories;
using Bennington.ContentTree.Data;
using Bennington.ContentTree.Providers.SectionNodeProvider.Models;
using Bennington.ContentTree.Providers.SectionNodeProvider.Repositories;
using Bennington.ContentTree.Repositories;
using Bennington.Core;

namespace Bennington.Cms.PrincipalProvider.SecurityHandlers
{
    public interface IHandleContentSecurity
    {
        void HandleSecurityForThisPage(HttpApplication app, EventArgs eventArgs);
    }

    public class HandleContentSecurity : IHandleContentSecurity
    {
        private readonly ICurrentUserContext currentUserContext;
        private readonly IContentTreeSectionNodeRepository contentTreeSectionNodeRepository;
        private readonly IUserRepository userRepository;
        private readonly IRoleRepository roleRepository;
        private readonly ITreeNodeRepository treeNodeRepository;
        private readonly IGetTheNotAuthorizedPage getTheNotAuthorizedPage;

        public HandleContentSecurity(ICurrentUserContext currentUserContext,
            IContentTreeSectionNodeRepository contentTreeSectionNodeRepository,
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            ITreeNodeRepository treeNodeRepository,
            IGetTheNotAuthorizedPage getTheNotAuthorizedPage)
        {
            this.currentUserContext = currentUserContext;
            this.contentTreeSectionNodeRepository = contentTreeSectionNodeRepository;
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.treeNodeRepository = treeNodeRepository;
            this.getTheNotAuthorizedPage = getTheNotAuthorizedPage;
        }

        public void HandleSecurityForThisPage(HttpApplication app, EventArgs eventArgs)
        {
            var routeData = app.Request.RequestContext.RouteData;
            var controller = (routeData.Values["controller"] ?? string.Empty).ToString();
            var currentPrincipal = currentUserContext.GetCurrentPrincipal();

            if (TheUserIsAnAdmin(currentPrincipal)) return;

            if (controller == "ContentTreeSectionNode" || controller == "ContentTreeNode")
            {
                var contentTreeSectionId = controller == "ContentTreeNode"
                                               ? GetTopLevelContentTreeSectionId(treeNodeRepository.GetAll().ToList(), app.Request.QueryString["TreeNodeId"])
                                               : app.Request.QueryString["TreeNodeId"];

                if (TheUserIsLoggedIn(currentPrincipal))
                {                    
                    var allContentTreeSectionNodes = contentTreeSectionNodeRepository.GetAllContentTreeSectionNodes();
                    var contentTreeSectionNode = allContentTreeSectionNodes.FirstOrDefault(x => x.Id == contentTreeSectionId);

                    var user = userRepository.GetAll().FirstOrDefault(x => x.Username == currentPrincipal.Identity.Name);

                    if (TheUserIsSomehowNotInTheRepository(user))
                    {
                        RedirectToUnauthorizedPage(app);
                    }
                    else if (contentTreeSectionNode != null)
                    {
                        var role = roleRepository.GetById(user.Role);

                        if (TheRoleIsNotDefined(role))
                        {
                            RedirectToUnauthorizedPage(app);
                        }
                        else
                        {
                            if (TheUserHasAccessToEverything(role)) return;

                            if (TheRoleDoesNotHaveAccessToThisContentTreeSectionNode(contentTreeSectionNode, role))
                                RedirectToUnauthorizedPage(app);
                        }
                    }
                }
                else
                {
                    RedirectToUnauthorizedPage(app);
                }
            }
        }

        private static bool TheUserHasAccessToEverything(Role role)
        {
            return role.AllContent;
        }

        private bool TheUserIsAnAdmin(IPrincipal currentPrincipal)
        {
            return currentPrincipal != null && currentPrincipal.Identity.Name.Equals("admin", StringComparison.InvariantCultureIgnoreCase);
        }

        private static string GetTopLevelContentTreeSectionId(IEnumerable<TreeNode> treeNodes, string treeNodeId)
        {
            var currentTreeNode = treeNodes.FirstOrDefault(x => x.TreeNodeId == treeNodeId);

            return currentTreeNode == null || currentTreeNode.ParentTreeNodeId == Guid.Empty.ToString() ? treeNodeId : GetTopLevelContentTreeSectionId(treeNodes, currentTreeNode.ParentTreeNodeId);
        }

        private static bool TheRoleDoesNotHaveAccessToThisContentTreeSectionNode(ContentTreeSectionNode contentTreeSectionNode, Role role)
        {
            return role.AvailableContentSections.Contains(contentTreeSectionNode.Id) == false;
        }

        private static bool TheUserIsLoggedIn(IPrincipal currentPrincipal)
        {
            return currentPrincipal != null && string.IsNullOrEmpty(currentPrincipal.Identity.Name) == false;
        }

        private static bool TheUserIsSomehowNotInTheRepository(User user)
        {
            return user == null;
        }

        private void RedirectToUnauthorizedPage(HttpApplication app)
        {
            app.Response.Redirect(getTheNotAuthorizedPage.GetUnauthorizedPage());
        }

        private static bool TheRoleIsNotDefined(Role role)
        {
            return role == null;
        }
    }
}