﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Paragon.ContentTree.Contexts;
using Paragon.ContentTree.Repositories;
using Paragon.Pages.Mappers;

namespace Paragon.Pages.Routing.Helpers
{
	public interface ITreeNodeIdToUrl
	{
		string GetUrlByTreeNodeId(string treeNodeId);
	}

	public class TreeNodeIdToUrl : ITreeNodeIdToUrl
	{
		private readonly ITreeNodeRepository treeNodeRepository;
		private readonly ITreeNodeSummaryContext treeNodeSummaryContext;
		private readonly ITreeNodeSummaryToContentTreeNodeMapper treeNodeSummaryToContentTreeNodeMapper;

		public TreeNodeIdToUrl(ITreeNodeRepository treeNodeRepository, ITreeNodeSummaryContext treeNodeSummaryContext, ITreeNodeSummaryToContentTreeNodeMapper treeNodeSummaryToContentTreeNodeMapper)
		{
			this.treeNodeSummaryToContentTreeNodeMapper = treeNodeSummaryToContentTreeNodeMapper;
			this.treeNodeSummaryContext = treeNodeSummaryContext;
			this.treeNodeRepository = treeNodeRepository;
		}

		public string GetUrlByTreeNodeId(string treeNodeId)
		{
			var url = string.Empty;
			var treeNode = treeNodeRepository.GetAll().Where(a => a.Id == treeNodeId).FirstOrDefault();

			var urlSegments = new List<string>();

			while ((treeNode != null) && (treeNode.ParentTreeNodeId != "-1"))
			{
				var treeNodeSummary = treeNodeSummaryContext.GetChildren(treeNode.ParentTreeNodeId).Where(a => a.Id == treeNode.Id).FirstOrDefault();

				var contentTreeNode = treeNodeSummaryToContentTreeNodeMapper.CreateInstance(treeNodeSummary);

				urlSegments.Add(contentTreeNode.UrlSegment);

				treeNode = treeNodeRepository.GetAll().Where(a => a.Id == treeNode.ParentTreeNodeId).FirstOrDefault();
			}

			urlSegments.Reverse();
			foreach(var segment in urlSegments)
			{
				url += "/" + segment;
			}
			return url;
		}
	}
}