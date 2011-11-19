﻿using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Bennington.ContentTree.Domain.Commands;
using Bennington.ContentTree.Providers.SectionNodeProvider.Context;
using Bennington.ContentTree.Providers.SectionNodeProvider.Mappers;
using Bennington.ContentTree.Providers.SectionNodeProvider.Models;
using Bennington.ContentTree.Providers.SectionNodeProvider.Repositories;
using Bennington.Core;
using Bennington.Core.Helpers;
using SimpleCqrs.Commanding;

namespace Bennington.ContentTree.Providers.SectionNodeProvider.Controllers
{
	public class ContentTreeSectionNodeController : Controller
	{
		private readonly IContentTreeSectionNodeRepository contentTreeSectionNodeRepository;
		private readonly IContentTreeSectionNodeToContentTreeSectionInputModelMapper contentTreeSectionNodeToContentTreeSectionInputModelMapper;
		private readonly ICommandBus commandBus;
		private readonly IContentTree contentTree;
		private readonly IGuidGetter guidGetter;
	    private readonly ICurrentUserContext currentUserContext;

	    public ContentTreeSectionNodeController(IContentTreeSectionNodeRepository contentTreeSectionNodeRepository, 
												IContentTreeSectionNodeToContentTreeSectionInputModelMapper contentTreeSectionNodeToContentTreeSectionInputModelMapper,
												ICommandBus commandBus,
												IContentTree contentTree,
												IGuidGetter guidGetter,
                                                ICurrentUserContext currentUserContext)
		{
	        this.currentUserContext = currentUserContext;
	        this.guidGetter = guidGetter;
			this.contentTree = contentTree;
			this.commandBus = commandBus;
			this.contentTreeSectionNodeToContentTreeSectionInputModelMapper = contentTreeSectionNodeToContentTreeSectionInputModelMapper;
			this.contentTreeSectionNodeRepository = contentTreeSectionNodeRepository;
		}

		[Authorize]
		public virtual ActionResult Delete(string id)
		{
			commandBus.Send(new DeleteTreeNodeCommand()
			{
				AggregateRootId = new Guid(id)
			});
			commandBus.Send(new DeleteSectionCommand(){ AggregateRootId = new Guid(id), LastModifyBy = currentUserContext.GetCurrentPrincipal().Identity.Name });
		    
            return new RedirectToRouteResult(new RouteValueDictionary { { "controller", "TreeManager" }, { "action", "Index" } });
		}

		[Authorize]
		[HttpPost]
        public virtual ActionResult Create(ContentTreeSectionInputModel contentTreeSectionInputModel)
		{
			if (ModelState.IsValid == false)
				return View("Modify", new ContentTreeSectionNodeViewModel()
				                      	{
				                      		ContentTreeSectionInputModel = contentTreeSectionInputModel,
											Action = "Create",
				                      	});

			var treeNodeIdString = contentTree.Create(contentTreeSectionInputModel.ParentTreeNodeId, typeof(SectionNodeProvider).AssemblyQualifiedName, typeof(ContentTreeSectionController).Name.Replace("Controller", string.Empty));
			
			commandBus.Send(new CreateSectionCommand()
			                	{
									SectionId = guidGetter.GetGuid().ToString(),
									TreeNodeId = treeNodeIdString,
			                		DefaultTreeNodeId = contentTreeSectionInputModel.DefaultTreeNodeId,
									Name = contentTreeSectionInputModel.Name,
									ParentTreeNodeId = contentTreeSectionInputModel.ParentTreeNodeId,
									Sequence = contentTreeSectionInputModel.Sequence,
									UrlSegment = contentTreeSectionInputModel.UrlSegment,
									Hidden = contentTreeSectionInputModel.Hidden,
									Inactive = contentTreeSectionInputModel.Inactive,
                                    LastModifyBy = currentUserContext.GetCurrentPrincipal().Identity.Name,
			                	});

			if (contentTreeSectionInputModel.Action.ToLower() == "save and exit")
				return new RedirectToRouteResult(new RouteValueDictionary()
			                                 	{
			                                 		{"controller", "ContentTree"},
													{"action", "Index"},
			                                 	});

			contentTreeSectionInputModel.TreeNodeId = treeNodeIdString;
			return new RedirectResult(GetRedirectUrlToModifyMethod(contentTreeSectionInputModel));
		}

		[Authorize]
        public virtual ActionResult Create(string parentTreeNodeId)
		{
			return View("Modify", new ContentTreeSectionNodeViewModel()
			                      	{
										Action = "Create",
			                      		ContentTreeSectionInputModel = new ContentTreeSectionInputModel()
			                      		                        	{
			                      		                        		ParentTreeNodeId = parentTreeNodeId,
			                      		                        	}
			                      	});
		}

		[Authorize]
		[HttpPost]
        public virtual ActionResult Modify(ContentTreeSectionInputModel contentTreeSectionInputModel)
		{
			if (ModelState.IsValid == false)
				return View("Modify", new ContentTreeSectionNodeViewModel() { Action = "Modify", ContentTreeSectionInputModel = contentTreeSectionInputModel });

			commandBus.Send(new ModifySectionCommand()
			                	{
									AggregateRootId = new Guid(contentTreeSectionInputModel.SectionId),
									SectionId = contentTreeSectionInputModel.SectionId,
									TreeNodeId = contentTreeSectionInputModel.TreeNodeId,
			                		DefaultTreeNodeId = contentTreeSectionInputModel.DefaultTreeNodeId,
									ParentTreeNodeId = contentTreeSectionInputModel.ParentTreeNodeId,
									UrlSegment = contentTreeSectionInputModel.UrlSegment,
									Sequence = contentTreeSectionInputModel.Sequence,
									Name = contentTreeSectionInputModel.Name,
									Hidden = contentTreeSectionInputModel.Hidden,
									Inactive = contentTreeSectionInputModel.Inactive,
                                    LastModifyBy = currentUserContext.GetCurrentPrincipal().Identity.Name
			                	});

			if (contentTreeSectionInputModel.Action != null)
			{
				if (contentTreeSectionInputModel.Action.ToLower() == "save and exit")
					return new RedirectToRouteResult(new RouteValueDictionary()
			                                 	{
			                                 		{"controller", "ContentTree"},
													{"action", "Index"},
			                                 	});
			}

			return new RedirectToRouteResult(new RouteValueDictionary()
			                                 	{
			                                 		{"controller", "ContentTreeSectionNode"},
													{"action", "Modify"},
													{ "treenodeid", contentTreeSectionInputModel == null ? "0" : contentTreeSectionInputModel.TreeNodeId },
			                                 	});
		}

		[Authorize]
        public virtual ActionResult Modify(string treeNodeId)
		{
			var contentTreeSection = contentTreeSectionNodeRepository.GetAllContentTreeSectionNodes().Where(a => a.Id == treeNodeId).FirstOrDefault();
			var contentTreeSectionInputModel = contentTreeSection == null ? new ContentTreeSectionInputModel()
			                		                        	{
			                		                        		TreeNodeId = treeNodeId,
			                		                        	} 
										: contentTreeSectionNodeToContentTreeSectionInputModelMapper.CreateInstance(contentTreeSection);

			var viewModel = new ContentTreeSectionNodeViewModel()
			                	{
									Action = "Modify",
			                		ContentTreeSectionInputModel = contentTreeSectionInputModel,
			                	};

			return View("Modify", viewModel);
		}

		private string GetRedirectUrlToModifyMethod(ContentTreeSectionInputModel contentTreeSectionInputModel)
		{
			if (Url == null) return "/";
			return Url.Action("Modify", "ContentTreeSectionNode", new { treeNodeId = contentTreeSectionInputModel == null ? "0" : contentTreeSectionInputModel.TreeNodeId });
		}
	}
}
