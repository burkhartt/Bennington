﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Bennington.Content.Sql.Data;
using Bennington.ContentTree.Contexts;
using Bennington.ContentTree.Domain.Events.Page;
using SimpleCqrs.Eventing;

namespace Bennington.Content.Sql.Denormalizers
{
    public class ContentRoutingDenormalizer : IHandleDomainEvents<PagePublishedEvent>,
                                              IHandleDomainEvents<PageDeletedEvent>
    {
        private readonly ITreeNodeSummaryContext treeNodeSummaryContext;
        private readonly ITreeNodeProviderContext treeNodeProviderContext;

        public ContentRoutingDenormalizer(ITreeNodeSummaryContext treeNodeSummaryContext,
                                          ITreeNodeProviderContext treeNodeProviderContext)
        {
            this.treeNodeProviderContext = treeNodeProviderContext;
            this.treeNodeSummaryContext = treeNodeSummaryContext;
        }

        public void Handle(PagePublishedEvent domainEvent)
        {
            using (var dataContext = new ContentDataContext(ConfigurationManager.ConnectionStrings["Bennington.ContentTree.Domain.ConnectionString"].ToString()))
            {
                var treeNode = treeNodeSummaryContext.GetTreeNodeSummaryByTreeNodeId(domainEvent.Id.ToString());
                var provider = treeNodeProviderContext.GetProviderByTypeName(treeNode.Type);



                var actions = provider.ContentTreeNodeContentItems;
            }
        }

        public void Handle(PageDeletedEvent domainEvent)
        {
        }
    }
}