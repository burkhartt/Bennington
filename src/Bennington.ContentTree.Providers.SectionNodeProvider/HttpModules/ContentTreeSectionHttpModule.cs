﻿using System;
using System.Linq;
using System.Web;
using Bennington.ContentTree.Contexts;
using Bennington.ContentTree.Helpers;
using Bennington.ContentTree.Providers.SectionNodeProvider.Repositories;
using MvcTurbine.ComponentModel;

namespace Bennington.ContentTree.Providers.SectionNodeProvider.HttpModules
{
	public class ContentTreeSectionHttpModule : IHttpModule
	{
		private readonly IServiceLocator serviceLocator;

		public ContentTreeSectionHttpModule(IServiceLocator serviceLocator)
		{
			this.serviceLocator = serviceLocator;
		}

		public void Init(HttpApplication context)
		{
			context.BeginRequest += new EventHandler(OnBeginRequest);
		}

		public void OnBeginRequest(Object application, EventArgs eventArgs)
		{
			var httpApplication = application as HttpApplication;
			if (httpApplication == null) return;

			if (httpApplication.Request.RawUrl.Split('/').Count() == 2)
			{
				var treeNodeSummary = serviceLocator.Resolve<IUrlToTreeNodeSummaryMapper>().CreateInstance(httpApplication.Request.RawUrl);
				if (treeNodeSummary == null) return;
				
				var section = serviceLocator.Resolve<IContentTreeSectionNodeRepository>().GetAllContentTreeSectionNodes()
							.Where(a => a.TreeNodeId == treeNodeSummary.Id).FirstOrDefault();
				if (section == null) return;

				var childPage = serviceLocator.Resolve<IContentTree>().GetTreeNodeSummaryByTreeNodeId(section.DefaultTreeNodeId);
				
				if (childPage != null)
					httpApplication.Response.Redirect(serviceLocator.Resolve<ITreeNodeIdToUrl>().GetUrlByTreeNodeId(childPage.Id));
			}
		}

		public void Dispose()
		{
		}
	}
}
