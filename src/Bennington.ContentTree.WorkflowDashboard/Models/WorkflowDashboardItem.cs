using System;
using Bennington.Core.List;

namespace Bennington.ContentTree.WorkflowDashboard.Models
{
    public class WorkflowDashboardItem
    {
        public string Name { get; set; }

        [Hidden]
        public string Type { get; set; }

        public string Status { get; set; }        

        [Hidden]
        public Guid Id { get; set; }

        [Hidden]
        public string Link { get; set; }
    }
}