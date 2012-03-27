using System;

namespace Bennington.ContentTree.WorkflowDashboard.Models
{
    public class WorkflowItem
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public WorkflowStatus Status { get; set; }

        public WorkflowType Type { get; set; }
    }

    public enum WorkflowType
    {
        Content
    }

    public enum WorkflowStatus
    {
        Pending,
        Active
    }
}