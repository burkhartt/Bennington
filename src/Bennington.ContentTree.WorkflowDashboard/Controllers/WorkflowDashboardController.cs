using System.Linq;
using Bennington.Cms.Controllers;
using Bennington.ContentTree.WorkflowDashboard.Builders;
using Bennington.ContentTree.WorkflowDashboard.Models;
using Bennington.ContentTree.WorkflowDashboard.Repositories;

namespace Bennington.ContentTree.WorkflowDashboard.Controllers
{
    public class WorkflowDashboardController : ListManageController<WorkflowDashboardItem, WorkflowDashboardForm>
    {
        private readonly IWorkflowItemRepository workflowItemRepository;
        private readonly IWorkflowItemLinkBuilder workflowItemLinkBuilder;

        public WorkflowDashboardController(IWorkflowItemRepository workflowItemRepository,
            IWorkflowItemLinkBuilder workflowItemLinkBuilder)
        {
            this.workflowItemRepository = workflowItemRepository;
            this.workflowItemLinkBuilder = workflowItemLinkBuilder;
        }

        protected override IQueryable<WorkflowDashboardItem> GetListItems(Core.List.ListViewModel listViewModel)
        {
            return workflowItemRepository.GetAll().Select(x => new WorkflowDashboardItem
                                                                   {
                                                                       Id = x.Id,
                                                                       Name = x.Name,
                                                                       Status = x.Status.ToString(),
                                                                       Type = x.Type.ToString(),
                                                                       Link = workflowItemLinkBuilder.BuildLink(x.Type, x.Id)
                                                                   }).AsQueryable();
        }
    }
}
