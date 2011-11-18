﻿using System;
using System.Collections.Generic;
using System.Linq;
using Bennington.ContentTree.Contexts;
using Bennington.ContentTree.Data;
using Bennington.ContentTree.Domain.Commands;
using Bennington.ContentTree.Models;
using Bennington.ContentTree.Repositories;
using Bennington.Core.Helpers;
using SimpleCqrs.Commanding;

namespace Bennington.ContentTree
{
	public interface IContentTree
	{
		TreeNodeSummary GetTreeNodeSummaryByTreeNodeId(string nodeId);
		IEnumerable<TreeNodeSummary> GetChildren(string parentNodeId);
	    IEnumerable<TreeNodeSummary> GetRootNodes();
		string Create(string parentNodeId, string providerTypeAssemblyQualifiedName, string controllerName);
	}

	public class ContentTree : IContentTree
	{
        public const string RootNodeId = "00000000-0000-0000-0000-000000000000";
		private readonly ITreeNodeRepository treeNodeRepository;
		private readonly ITreeNodeProviderContext treeNodeProviderContext;
		private readonly ICommandBus commandBus;
		private readonly IGuidGetter guidGetter;

		public ContentTree(ITreeNodeRepository treeNodeRepository, 
										ITreeNodeProviderContext treeNodeProviderContext,
										ICommandBus commandBus,
										IGuidGetter guidGetter)
		{
			this.guidGetter = guidGetter;
			this.commandBus = commandBus;
			this.treeNodeProviderContext = treeNodeProviderContext;
			this.treeNodeRepository = treeNodeRepository;
		}

	    public IEnumerable<TreeNodeSummary> GetRootNodes()
	    {
	        return GetChildren(RootNodeId);
	    }

        public string Create(string parentNodeId, string providerTypeAssemblyQualifiedName, string controllerName)
		{
			ThrowExceptionIfTheProviderTypeDoesNotImplementIAmATreeNodeExtensionProvider(Type.GetType(providerTypeAssemblyQualifiedName));

			var guid = guidGetter.GetGuid();
			commandBus.Send(new CreateTreeNodeCommand()
			                	{
			                		ParentId = parentNodeId,
									Type = Type.GetType(providerTypeAssemblyQualifiedName),
									TreeNodeId = guid,
									AggregateRootId = guid,
                                    ControllerName = controllerName
			                	});

			return guid.ToString();
		}

		private static void ThrowExceptionIfTheProviderTypeDoesNotImplementIAmATreeNodeExtensionProvider(Type providerType)
		{
			if (!typeof(IAmATreeNodeExtensionProvider).IsAssignableFrom(providerType))
				throw new Exception(string.Format("Provider type must implement {0}", typeof(IAmATreeNodeExtensionProvider).FullName));
		}

		public IEnumerable<TreeNodeSummary> GetChildren(string parentNodeId)
		{
			var allNodes = treeNodeRepository.GetAll();
			var childNodes = from node in allNodes
							 where (node.ParentTreeNodeId == parentNodeId)
							 select GetTreeNodeSummaryForTreeNode(node);
			return childNodes.Where(a => a != null);
		}

		public TreeNodeSummary GetTreeNodeSummaryByTreeNodeId(string nodeId)
		{
			var treeNode = treeNodeRepository.GetAll().Where(a => a.Id == nodeId).FirstOrDefault();
			if (treeNode == null) return null;
			if (treeNode.Id == RootNodeId) return null;
			
			return GetTreeNodeSummaryForTreeNode(treeNode);
		}

		private TreeNodeSummary GetTreeNodeSummaryForTreeNode(TreeNode treeNode)
		{
			var provider = treeNodeProviderContext.GetProviderByTypeName(treeNode.Type);
			if (provider == null) throw new Exception(string.Format("Content tree node provider for type: {0} not found.", treeNode.Type));

			var treeNodeExtension = provider.GetAll().Where(a => a.TreeNodeId == treeNode.Id).FirstOrDefault();
			if (treeNodeExtension == null) return null;

			var treeNodeSummary = new TreeNodeSummary()
			       	{
						Name = treeNodeExtension.Name,
						Id = treeNode.Id,
						UrlSegment = treeNodeExtension.UrlSegment,
						HasChildren = treeNodeRepository.GetAll().Where(a => a.ParentTreeNodeId == treeNode.Id).Any(),
						ControllerToUseForModification = provider.ControllerToUseForModification,
						ActionToUseForModification = provider.ActionToUseForModification,
						ControllerToUseForCreation = provider.ControllerToUseForCreation,
						ActionToUseForCreation = provider.ActionToUseForCreation,
						RouteValuesForModification = new { TreeNodeId = treeNode.Id },
						RouteValuesForCreation = new { ParentTreeNodeId = treeNode.Id },
						ParentTreeNodeId = treeNode.ParentTreeNodeId,
						Sequence = treeNodeExtension.Sequence,
						Type = treeNode.Type,
						MayHaveChildNodes = provider.MayHaveChildNodes,
						Hidden = treeNodeExtension.Hidden,
                        IconUrl = treeNodeExtension.IconUrl,
                        LastModifyBy = treeNodeExtension.LastModifyBy,
                        LastModifyDate = treeNodeExtension.LastModifyDate,
                        Active = !treeNodeExtension.Inactive
			       	};
			return treeNodeSummary;
		}
	}
}