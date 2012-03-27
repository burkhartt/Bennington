using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Bennington.ContentTree.WorkflowDashboard.Models;
using Simple.Data;

namespace Bennington.ContentTree.WorkflowDashboard.Repositories
{
    public interface IWorkflowItemRepository
    {
        IQueryable<WorkflowItem> GetAll();
        void Create(Guid id);
        WorkflowItem GetById(Guid id);
        void Update(WorkflowItem item);
    }

    public class WorkflowItemRepository : IWorkflowItemRepository
    {
        public IQueryable<WorkflowItem> GetAll()
        {
            IList<WorkflowItem> workflowItems = GetDatabase().WorkflowItems.All().ToList<WorkflowItem>();
            return workflowItems.AsQueryable();
        }

        public void Create(Guid id)
        {
            GetDatabase().WorkflowItems.Insert(Id: id, LastModifyDate: DateTime.Now);
        }

        public WorkflowItem GetById(Guid id)
        {
            return (WorkflowItem)GetDatabase().WorkflowItems.FindById(id);
        }

        public void Update(WorkflowItem item)
        {
            if (item.LastModifyDate == DateTime.MinValue)
                item.LastModifyDate = DateTime.Now;

            GetDatabase().WorkflowItems.Update(item);
        }

        private dynamic GetDatabase()
        {
            return Database.OpenConnection(ConfigurationManager.ConnectionStrings["Bennington.ContentTree.Domain.ConnectionString"].ConnectionString);
        }
    }
}