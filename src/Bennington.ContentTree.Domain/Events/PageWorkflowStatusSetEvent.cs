using System;
using SimpleCqrs.Eventing;

namespace Bennington.ContentTree.Domain.Events
{
    public class PageWorkflowStatusSetEvent : DomainEvent
    {
        public string Status { get; set; }
    }
}