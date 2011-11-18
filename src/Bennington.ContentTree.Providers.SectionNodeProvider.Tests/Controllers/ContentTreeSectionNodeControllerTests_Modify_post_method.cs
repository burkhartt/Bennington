﻿using System;
using System.Security.Principal;
using AutoMoq;
using Bennington.ContentTree.Domain.Commands;
using Bennington.ContentTree.Providers.SectionNodeProvider.Controllers;
using Bennington.ContentTree.Providers.SectionNodeProvider.Models;
using Bennington.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SimpleCqrs.Commanding;

namespace Bennington.ContentTree.Providers.SectionNodeProvider.Tests.Controllers
{
	[TestClass]
	public class ContentTreeSectionNodeControllerTests_Modify_post_method
	{
		private AutoMoqer mocker;

		[TestInitialize]
		public void Init()
		{
			mocker = new AutoMoqer();
            mocker.GetMock<ICurrentUserContext>()
               .Setup(a => a.GetCurrentPrincipal())
               .Returns(new GenericPrincipal(new GenericIdentity("test"), new string[] { }));
		}

		[TestMethod]
		public void Sends_ModifySectionCommand_when_model_state_is_valid()
		{
			mocker.Resolve<ContentTreeSectionNodeController>().Modify(new ContentTreeSectionInputModel()
			                                                          	{
			                                                          		SectionId = Guid.NewGuid().ToString()
			                                                          	});

			mocker.GetMock<ICommandBus>().Verify(a => a.Send(It.IsAny<ModifySectionCommand>()), Times.Once());
		}

		[TestMethod]
		public void Does_not_send_ModifySectionCommand_when_model_state_is_not_valid()
		{
			var controller = mocker.Resolve<ContentTreeSectionNodeController>();
			controller.ModelState.AddModelError("key", "error");

			controller.Modify(new ContentTreeSectionInputModel()
			                  	{
									SectionId = Guid.NewGuid().ToString()
			                  	});

			mocker.GetMock<ICommandBus>().Verify(a => a.Send(It.IsAny<ModifySectionCommand>()), Times.Never());
		}

		[TestMethod]
		public void Sends_ModifySectionCommand_with_correct_DefaultTreeNodeId_set_when_input_model_is_valid()
		{
			var defaultTreeNodeId = new Guid().ToString();
			mocker.Resolve<ContentTreeSectionNodeController>().Modify(new ContentTreeSectionInputModel()
			{
				Action = "action",
				DefaultTreeNodeId = defaultTreeNodeId,
				SectionId = Guid.NewGuid().ToString()
			});

			mocker.GetMock<ICommandBus>().Verify(a => a.Send(It.Is<ModifySectionCommand>(b => b.DefaultTreeNodeId == defaultTreeNodeId)), Times.Once());
		}

		[TestMethod]
		public void Sends_ModifySectionCommand_with_correct_Name_set_when_input_model_is_valid()
		{
			var defaultTreeNodeId = new Guid().ToString();
			mocker.Resolve<ContentTreeSectionNodeController>().Modify(new ContentTreeSectionInputModel()
			{
				Action = "action",
				Name = "name",
				SectionId = Guid.NewGuid().ToString()
			});

			mocker.GetMock<ICommandBus>().Verify(a => a.Send(It.Is<ModifySectionCommand>(b => b.Name == "name")), Times.Once());
		}

		[TestMethod]
		public void Sends_ModifySectionCommand_with_correct_ParentTreeNodeId_set_when_input_model_is_valid()
		{
			var defaultTreeNodeId = new Guid().ToString();
			mocker.Resolve<ContentTreeSectionNodeController>().Modify(new ContentTreeSectionInputModel()
			{
				Action = "action",
				ParentTreeNodeId = "parentId",
				SectionId = Guid.NewGuid().ToString()
			});

			mocker.GetMock<ICommandBus>().Verify(a => a.Send(It.Is<ModifySectionCommand>(b => b.ParentTreeNodeId == "parentId")), Times.Once());
		}

		[TestMethod]
		public void Sends_ModifySectionCommand_with_correct_Sequence_set_when_input_model_is_valid()
		{
			var defaultTreeNodeId = new Guid().ToString();
			mocker.Resolve<ContentTreeSectionNodeController>().Modify(new ContentTreeSectionInputModel()
			{
				Action = "action",
				Sequence = 1000,
				SectionId = Guid.NewGuid().ToString()
			});

			mocker.GetMock<ICommandBus>().Verify(a => a.Send(It.Is<ModifySectionCommand>(b => b.Sequence == 1000)), Times.Once());
		}

		[TestMethod]
		public void Sends_ModifySectionCommand_with_correct_UrlSegment_set_when_input_model_is_valid()
		{
			var defaultTreeNodeId = new Guid().ToString();
			mocker.Resolve<ContentTreeSectionNodeController>().Modify(new ContentTreeSectionInputModel()
			{
				Action = "action",
				UrlSegment = "urlSegment",
				SectionId = Guid.NewGuid().ToString()
			});

			mocker.GetMock<ICommandBus>().Verify(a => a.Send(It.Is<ModifySectionCommand>(b => b.UrlSegment == "urlSegment")), Times.Once());
		}

		[TestMethod]
		public void Sends_ModifySectionCommand_with_correct_TreeNodeId_set_when_input_model_is_valid()
		{
			var treeNodeId = new Guid();
			mocker.Resolve<ContentTreeSectionNodeController>().Modify(new ContentTreeSectionInputModel()
			{
				TreeNodeId = treeNodeId.ToString(),
				Action = "action",
				UrlSegment = "urlSegment",
				SectionId = Guid.NewGuid().ToString()
			});

			mocker.GetMock<ICommandBus>().Verify(a => a.Send(It.Is<ModifySectionCommand>(b => b.TreeNodeId == treeNodeId.ToString())), Times.Once());
		}

		[TestMethod]
		public void Sends_ModifySectionCommand_with_correct_Hidden_property_set_when_input_model_is_valid()
		{
			var defaultTreeNodeId = new Guid().ToString();
			mocker.Resolve<ContentTreeSectionNodeController>().Modify(new ContentTreeSectionInputModel()
			{
				Action = "action",
				Name = "name",
				SectionId = Guid.NewGuid().ToString(),
				Hidden = true,
			});

			mocker.GetMock<ICommandBus>().Verify(a => a.Send(It.Is<ModifySectionCommand>(b => b.Hidden == true)), Times.Once());
		}

		[TestMethod]
		public void Sends_ModifySectionCommand_with_correct_Inactive_property_set_when_input_model_is_valid()
		{
			var defaultTreeNodeId = new Guid().ToString();
			mocker.Resolve<ContentTreeSectionNodeController>().Modify(new ContentTreeSectionInputModel()
			{
				Action = "action",
				Name = "name",
				SectionId = Guid.NewGuid().ToString(),
				Inactive= true,
			});

			mocker.GetMock<ICommandBus>().Verify(a => a.Send(It.Is<ModifySectionCommand>(b => b.Inactive == true)), Times.Once());
		}

        [TestMethod]
        public void Sends_ModifySectionCommand_with_LastModifyBy_set_when_input_model_is_valid()
        {
            var defaultTreeNodeId = new Guid().ToString();
            mocker.Resolve<ContentTreeSectionNodeController>().Modify(new ContentTreeSectionInputModel()
            {
                Action = "action",
                DefaultTreeNodeId = defaultTreeNodeId,
                SectionId = Guid.NewGuid().ToString()
            });

            mocker.GetMock<ICommandBus>().Verify(a => a.Send(It.Is<ModifySectionCommand>(b => b.LastModifyBy == "test")), Times.Once());
        }
	}
}
