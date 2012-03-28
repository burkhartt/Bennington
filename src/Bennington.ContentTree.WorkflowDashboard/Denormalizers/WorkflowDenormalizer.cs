using System;
using System.Linq;
using Bennington.ContentTree.Domain.Events;
using Bennington.ContentTree.Domain.Events.Page;
using Bennington.ContentTree.Domain.Events.TreeNode;
using Bennington.ContentTree.Providers.ContentNodeProvider.Repositories;
using Bennington.ContentTree.Repositories;
using Bennington.ContentTree.WorkflowDashboard.Models;
using Bennington.ContentTree.WorkflowDashboard.Repositories;
using SimpleCqrs.Eventing;

namespace Bennington.ContentTree.WorkflowDashboard.Denormalizers
{
    public class WorkflowDenormalizer : IHandleDomainEvents<PageWorkflowStatusSetEvent>,
        IHandleDomainEvents<PageCreatedEvent>,
        IHandleDomainEvents<PageNameSetEvent>,
        IHandleDomainEvents<PageLastModifyBySetEvent>,
        IHandleDomainEvents<PageLastModifyDateSetEvent>
    {
        private readonly IWorkflowItemRepository workflowItemRepository;
        private readonly ITreeNodeRepository treeNodeRepository;
        private readonly IContentNodeProviderDraftRepository contentNodeProviderDraftRepository;

        public WorkflowDenormalizer(IWorkflowItemRepository workflowItemRepository,
            ITreeNodeRepository treeNodeRepository,
            IContentNodeProviderDraftRepository contentNodeProviderDraftRepository)
        {
            this.workflowItemRepository = workflowItemRepository;
            this.treeNodeRepository = treeNodeRepository;
            this.contentNodeProviderDraftRepository = contentNodeProviderDraftRepository;
        }

        public void Handle(PageWorkflowStatusSetEvent domainEvent)
        {
            var item = workflowItemRepository.GetById(domainEvent.AggregateRootId) ?? CreatePageAndReturnIt(domainEvent.AggregateRootId);
            item.Status = domainEvent.Status;
            workflowItemRepository.Update(item);
        }

        private WorkflowItem CreatePageAndReturnIt(Guid aggregateRootId)
        {
            workflowItemRepository.Create(aggregateRootId);
            return workflowItemRepository.GetById(aggregateRootId);
        }

        public void Handle(PageCreatedEvent domainEvent)
        {
            workflowItemRepository.Create(domainEvent.AggregateRootId);
        }

        public void Handle(PageNameSetEvent domainEvent)
        {
            var item = workflowItemRepository.GetById(domainEvent.AggregateRootId) ?? CreatePageAndReturnIt(domainEvent.AggregateRootId);
            item.Name = domainEvent.Name;
            workflowItemRepository.Update(item);
        }

        public void Handle(PageLastModifyBySetEvent domainEvent)
        {
            var item = workflowItemRepository.GetById(domainEvent.AggregateRootId) ?? CreatePageAndReturnIt(domainEvent.AggregateRootId);
            item.LastModifiedBy = domainEvent.LastModifyBy;
            workflowItemRepository.Update(item);
        }

        public void Handle(PageLastModifyDateSetEvent domainEvent)
        {
            var item = workflowItemRepository.GetById(domainEvent.AggregateRootId) ?? CreatePageAndReturnIt(domainEvent.AggregateRootId);
            item.LastModifyDate = domainEvent.DateTime;

            var treeNodeId = contentNodeProviderDraftRepository.GetAllContentNodeProviderDrafts().FirstOrDefault(x => x.PageId == domainEvent.AggregateRootId.ToString()).TreeNodeId;

            var treeNode = treeNodeRepository.GetAll().FirstOrDefault(x => x.TreeNodeId == treeNodeId);
            item.TreeNodeId = new Guid(treeNodeId);
            item.Type = treeNode.ControllerName != "ContentTree" ? treeNode.ControllerName : "Page";
            workflowItemRepository.Update(item);
        }
    }
}