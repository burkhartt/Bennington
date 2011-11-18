﻿using System.Linq;
using System.Web.Routing;
using Bennington.ContentTree.Contexts;
using Bennington.ContentTree.Providers.ContentNodeProvider.Context;
using Bennington.ContentTree.Providers.ContentNodeProvider.Models;
using Bennington.Core.Helpers;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.ViewModelBuilders
{
	public interface IContentTreeNodeDisplayViewModelBuilder
	{
		ContentTreeNodeDisplayViewModel BuildViewModel(string rawUrl, RouteData routeData, string currentAction);
	}

	public class ContentTreeNodeDisplayViewModelBuilder : IContentTreeNodeDisplayViewModelBuilder
	{
		private readonly IContentTree contentTree;
		private readonly IContentTreeNodeContext contentTreeNodeContext;

	    public ContentTreeNodeDisplayViewModelBuilder(IContentTree contentTree, 
                                                        IContentTreeNodeContext contentTreeNodeContext)
		{
	        this.contentTreeNodeContext = contentTreeNodeContext;
			this.contentTree = contentTree;
		}

		public ContentTreeNodeDisplayViewModel BuildViewModel(string rawUrl, RouteData routeData, string currentAction)
		{
			var nodeSegments = ScrubUrlAndReturnEnumerableOfNodeSegments(rawUrl);

			var workingTreeNodeId = GetTreeNodeIdFromTreeNodeSummaryContextUsingNodeSegments(nodeSegments);

			var viewModel = new ContentTreeNodeDisplayViewModel()
			       	{
			       		Body = string.Empty,
						Header = string.Empty,
			       	};
			if (string.IsNullOrEmpty(workingTreeNodeId)) return viewModel;

            var action = currentAction; //GetAction(routeData);
		    //var parentRouteData = getParentRouteDataDictionaryFromChildActionRouteData.GetRouteValues(routeData);

			var contentTreeNodes = contentTreeNodeContext.GetContentTreeNodesByTreeId(workingTreeNodeId).Where(a => a.Action == action);
			if (contentTreeNodes.Count() == 0) return viewModel;

			return (from item in contentTreeNodes
			       select new ContentTreeNodeDisplayViewModel
			              	{
								Body = item.Body,
								Header = item.HeaderText,
								HeaderImage = item.HeaderImage,
			              	}).FirstOrDefault();
		}

		private string GetTreeNodeIdFromTreeNodeSummaryContextUsingNodeSegments(string[] nodeSegments)
		{
			var workingTreeNodeId = ContentTreeNodeContext.RootNodeId;
			foreach (var nodeSegment in nodeSegments)
			{
				var childNodes = contentTree.GetChildren(workingTreeNodeId);
				var treeNodeSummary = childNodes.Where(a => a.UrlSegment.ToUpper() == nodeSegment.ToUpper()).FirstOrDefault();
				if (treeNodeSummary == null) break;
				workingTreeNodeId = treeNodeSummary.Id;
			}
			return workingTreeNodeId;
		}

		private string[] ScrubUrlAndReturnEnumerableOfNodeSegments(string rawUrl)
		{
			if (rawUrl == null) rawUrl = string.Empty;
			if ((rawUrl.StartsWith("/")) && (rawUrl.Length > 1)) rawUrl = rawUrl.Substring(1, rawUrl.Length - 1);
			if (rawUrl.Contains("?"))
				rawUrl = rawUrl.Split('?')[0];
			return rawUrl.Split('/');
		}

		private string GetAction(RouteData routeData)
		{
			var action = "Index";
			if (routeData == null) return action;
			if (routeData.Values == null) return action;

			var actionValue = routeData.Values["Action"];
			if (actionValue!= null)
				action = routeData.Values["Action"].ToString();
			return action;
		}
	}
}
