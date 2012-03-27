using System;
using Bennington.ContentTree.Domain.Events;
using Bennington.ContentTree.Domain.Events.Page;
using Bennington.ContentTree.WorkflowDashboard.Repositories;
using SimpleCqrs.Eventing;

namespace Bennington.ContentTree.WorkflowDashboard.Denormalizers
{
    public class WorkflowDenormalizer : IHandleDomainEvents<PageWorkflowStatusSetEvent>,
        IHandleDomainEvents<PageCreatedEvent>,
        IHandleDomainEvents<PageNameSetEvent>,
        IHandleDomainEvents<PageTreeNodeIdSetEvent>
    {
        private readonly IWorkflowItemRepository workflowItemRepository;

        public WorkflowDenormalizer(IWorkflowItemRepository workflowItemRepository)
        {
            this.workflowItemRepository = workflowItemRepository;
        }

        public void Handle(PageWorkflowStatusSetEvent domainEvent)
        {
            var item = workflowItemRepository.GetById(domainEvent.AggregateRootId);
            item.Status = domainEvent.Status;
            workflowItemRepository.Update(item);
        }

        public void Handle(PageCreatedEvent domainEvent)
        {
            workflowItemRepository.Create(domainEvent.AggregateRootId);
        }

        public void Handle(PageNameSetEvent domainEvent)
        {
            var item = workflowItemRepository.GetById(domainEvent.AggregateRootId);
            item.Name = domainEvent.Name;
            workflowItemRepository.Update(item);
        }

        public void Handle(PageTreeNodeIdSetEvent domainEvent)
        {
            var item = workflowItemRepository.GetById(domainEvent.AggregateRootId);
            item.TreeNodeId = domainEvent.TreeNodeId;
            workflowItemRepository.Update(item);
        }
    }
}