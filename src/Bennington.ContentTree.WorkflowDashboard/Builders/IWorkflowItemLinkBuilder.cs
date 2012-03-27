using System;
using Bennington.ContentTree.WorkflowDashboard.Models;

namespace Bennington.ContentTree.WorkflowDashboard.Builders
{
    public interface IWorkflowItemLinkBuilder
    {
        string BuildLink(string type, Guid id);
    }

    public class WorkflowItemLinkBuilder : IWorkflowItemLinkBuilder
    {
        public string BuildLink(string type, Guid id)
        {
            return "/Manage/ContentTreeNode/Modify?TreeNodeId=" + id;
        }
    }
}