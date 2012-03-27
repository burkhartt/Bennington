using System;
using Bennington.ContentTree.WorkflowDashboard.Models;

namespace Bennington.ContentTree.WorkflowDashboard.Builders
{
    public interface IWorkflowItemLinkBuilder
    {
        string BuildLink(WorkflowType type, Guid id);
    }

    public class WorkflowItemLinkBuilder : IWorkflowItemLinkBuilder
    {
        public string BuildLink(WorkflowType type, Guid id)
        {
            return "/Manage/ContentTreeNode/Modify?TreeNodeId=" + id;
        }
    }
}