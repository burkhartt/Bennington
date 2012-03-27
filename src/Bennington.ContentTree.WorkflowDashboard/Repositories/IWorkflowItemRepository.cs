using System;
using System.Collections.Generic;
using System.Configuration;
using Bennington.ContentTree.WorkflowDashboard.Models;
using Simple.Data;

namespace Bennington.ContentTree.WorkflowDashboard.Repositories
{
    public interface IWorkflowItemRepository
    {
        IEnumerable<WorkflowItem> GetAll();
    }

    public class WorkflowItemRepository : IWorkflowItemRepository
    {
        public IEnumerable<WorkflowItem> GetAll()
        {
            return GetDatabase().WorkflowItems.All().ToList<WorkflowItem>();
        }

        private dynamic GetDatabase()
        {
            return Database.OpenConnection(ConfigurationManager.ConnectionStrings["Bennington.ContentTree.Domain.ConnectionString"].ConnectionString);
        }
    }
}