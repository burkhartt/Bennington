using System;
using System.ComponentModel.DataAnnotations;
using Bennington.Core.List;

namespace Bennington.ContentTree.WorkflowDashboard.Models
{
    public class WorkflowDashboardItem
    {
        public string Name { get; set; }
 
        public string Type { get; set; }

        public string Status { get; set; }        

        [Hidden]
        public Guid Id { get; set; }

        [Hidden]
        public string Link { get; set; }

        [Display(Name="Last Modified By")]
        public string LastModifiedBy { get; set; }

        [Display(Name = "Last Modified Date")]
        public DateTime LastModifyDate { get; set; }
    }
}