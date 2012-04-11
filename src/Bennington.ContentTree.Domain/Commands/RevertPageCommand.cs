using System;
using SimpleCqrs.Commanding;

namespace Bennington.ContentTree.Domain.Commands
{
    public class RevertPageCommand : CommandWithAggregateRootId
    {
        public Guid PageId
        {
            get { return AggregateRootId; }
            set { AggregateRootId = value; }
        }
    }
}