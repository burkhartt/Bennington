using System;
using SimpleCqrs.Eventing;

namespace Bennington.ContentTree.Domain.Events
{
    public class PageIsNewSetEvent : DomainEvent
    {
        public bool IsNew { get; set; }
    }
}