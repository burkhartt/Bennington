using System;

namespace Bennington.ContentTree.WorkflowDashboard.Models
{
    public class WorkflowItem
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Status { get; set; }

        public string Type { get; set; }

        public Guid TreeNodeId { get; set; }
    }
}