using Bennington.ContentTree.Domain.AggregateRoots;
using Bennington.ContentTree.Domain.Commands;
using SimpleCqrs.Commanding;
using SimpleCqrs.Domain;

namespace Bennington.ContentTree.Domain.CommandHandlers
{
    public class RevertPageCommandHandler : AggregateRootCommandHandler<RevertPageCommand, Page>
    {
        private readonly IDomainRepository domainRepository;

        public RevertPageCommandHandler(IDomainRepository domainRepository)
        {
            this.domainRepository = domainRepository;
        }

        public override void Handle(RevertPageCommand command, Page aggregateRoot)
        {
            var page = new Page();
            page.PageId = command.AggregateRootId;
            page.Revert();
            domainRepository.Save(page);
        }
    }
}