﻿using System.Collections.Generic;
using System.Linq;
using Paragon.ContentTree.Contexts;
using Paragon.ContentTree.Models;

namespace Paragon.Cms.ViewModelBuilders
{
	public interface ITreeBranchViewModelBuilder
	{
		TreeBranchViewModel BuildViewModel(string parentNodeId);
	}

	public class TreeBranchViewModelBuilder : ITreeBranchViewModelBuilder
	{
		private readonly ITreeNodeSummaryContext treeNodeSummaryContext;

		public TreeBranchViewModelBuilder(ITreeNodeSummaryContext treeNodeSummaryContext)
		{
			this.treeNodeSummaryContext = treeNodeSummaryContext;
		}

		public TreeBranchViewModel BuildViewModel(string parentNodeId)
		{
			var listToReturn = new List<TreeNodeSummary>();
			var treeNodeSummaries = treeNodeSummaryContext.GetChildren(parentNodeId).OrderBy(a => a.Sequence ?? 999999);
			foreach (var treeNodeSummary in treeNodeSummaries)
			{
				if (string.IsNullOrEmpty(treeNodeSummary.Name))
					treeNodeSummary.Name = "Unknown";
				listToReturn.Add(treeNodeSummary);
			}
			return new TreeBranchViewModel()
			       	{
			       		TreeNodeSummaries = listToReturn,
			       	};
		}
	}
}