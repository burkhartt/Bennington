﻿using System.Web.Routing;
using AutoMoq;
using Bennington.ContentTree.Models;
using Bennington.ContentTree.Providers.ContentNodeProvider.Context;
using Bennington.ContentTree.Providers.ContentNodeProvider.Models;
using Bennington.ContentTree.Providers.ContentNodeProvider.ViewModelBuilders;
using Bennington.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ContentTreeNode = Bennington.ContentTree.Models.ContentTreeNode;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Tests.ViewModelBuilders
{
	[TestClass]
	public class ContentTreeNodeDisplayViewModelBuilderTests_BuildViewModel
	{
		private AutoMoqer mocker;

		[TestInitialize]
		public void Init()
		{
			mocker = new AutoMoqer();
		}

		[TestMethod]
		public void Returns_correct_Header_for_tree_node_url_when_url_contains_querystring_data()
		{
			mocker.GetMock<IContentTree>().Setup(a => a.GetChildren(ContentTreeNodeContext.RootNodeId))
				.Returns(new ContentTreeNode[]
				         	{
				         		new ContentTreeNode()
				         			{
				         				Id = "1",
										UrlSegment = "test",
										ParentTreeNodeId = ContentTreeNodeContext.RootNodeId,
				         			}, 
							});
			mocker.GetMock<IContentTreeNodeContext>().Setup(a => a.GetContentTreeNodesByTreeId("1"))
				.Returns(new Models.ContentTreeNode[]
				         	{
				         		new Models.ContentTreeNode()
				         			{
				         				Action = "Index",
										Body = "page content1",
										HeaderText = "test1"
				         			}, 
							});
			var routeData = new RouteData();
			routeData.Values.Add("Action", "Index");

			var result = mocker.Resolve<ContentTreeNodeDisplayViewModelBuilder>().BuildViewModel("test?a=1", routeData, "Index");

			Assert.AreEqual("test1", result.Header);
		}

		[TestMethod]
		public void Returns_correct_HeaderImage_for_tree_node_url()
		{
			mocker.GetMock<IContentTree>().Setup(a => a.GetChildren(ContentTreeNodeContext.RootNodeId))
				.Returns(new ContentTreeNode[]
				         	{
				         		new ContentTreeNode()
				         			{
				         				Id = "1",
										UrlSegment = "test",
										ParentTreeNodeId = ContentTreeNodeContext.RootNodeId,
				         			}, 
							});
			mocker.GetMock<IContentTreeNodeContext>().Setup(a => a.GetContentTreeNodesByTreeId("1"))
				.Returns(new Models.ContentTreeNode[]
				         	{
				         		new Models.ContentTreeNode()
				         			{
				         				Action = "Index",
										Body = "page content1",
										HeaderText = "test1",
										HeaderImage = "test.jpg"
				         			}, 
							});
			var routeData = new RouteData();
			routeData.Values.Add("Action", "Index");

			var result = mocker.Resolve<ContentTreeNodeDisplayViewModelBuilder>().BuildViewModel("test", routeData, "Index");

			Assert.AreEqual("test.jpg", result.HeaderImage);
		}

		[TestMethod]
		public void Returns_correct_Header_value_for_second_level_tree_node_url_when_url_case_does_not_match()
		{
			mocker.GetMock<IContentTree>().Setup(a => a.GetChildren(ContentTreeNodeContext.RootNodeId))
				.Returns(new ContentTreeNode[]
				         	{
				         		new ContentTreeNode()
				         			{
				         				Id = "1",
										UrlSegment = "Section1",
										ParentTreeNodeId = ContentTreeNodeContext.RootNodeId,
				         			}, 
							});
			mocker.GetMock<IContentTreeNodeContext>().Setup(a => a.GetContentTreeNodesByTreeId("1"))
				.Returns(new Models.ContentTreeNode[]
				         	{
				         		new Models.ContentTreeNode()
				         			{
				         				Action = "Index",
										Body = "page content",
										HeaderText = "section1"
				         			}, 
							});
			mocker.GetMock<IContentTree>().Setup(a => a.GetChildren("1"))
				.Returns(new ContentTreeNode[]
				         	{
				         		new ContentTreeNode()
				         			{
				         				Id = "2",
										UrlSegment = "test"
				         			}, 
							});
			mocker.GetMock<IContentTreeNodeContext>().Setup(a => a.GetContentTreeNodesByTreeId("2"))
				.Returns(new Models.ContentTreeNode[]
				         	{
				         		new Models.ContentTreeNode()
				         			{
				         				Action = "Index",
										Body = "page1 content",
										HeaderText = "page1"
				         			}, 
							});
			var routeData = new RouteData();
			routeData.Values.Add("Action", "Index");

			var result = mocker.Resolve<ContentTreeNodeDisplayViewModelBuilder>().BuildViewModel("SECTION1/TEST", routeData, "Index");

			Assert.AreEqual("page1", result.Header);
		}

		[TestMethod]
		public void Returns_correct_Header_value_for_first_level_tree_node_url_when_url_case_does_not_match()
		{
			mocker.GetMock<IContentTree>().Setup(a => a.GetChildren(ContentTreeNodeContext.RootNodeId))
				.Returns(new ContentTreeNode[]
				         	{
				         		new ContentTreeNode()
				         			{
				         				Id = "1",
										UrlSegment = "Section1"
				         			}, 
							});
			mocker.GetMock<IContentTreeNodeContext>().Setup(a => a.GetContentTreeNodesByTreeId("1"))
				.Returns(new Models.ContentTreeNode[]
				         	{
				         		new Models.ContentTreeNode()
				         			{
				         				Action = "Index",
										Body = "page content",
										HeaderText = "section1"
				         			}, 
							});
			var routeData = new RouteData();
			routeData.Values.Add("Action", "Index");

			var result = mocker.Resolve<ContentTreeNodeDisplayViewModelBuilder>().BuildViewModel("SECTION1", routeData, "Index");

			Assert.AreEqual("section1", result.Header);
		}

		[TestMethod]
		public void Returns_correct_Header_value_for_first_level_tree_node_url()
		{
			mocker.GetMock<IContentTree>().Setup(a => a.GetChildren(ContentTreeNodeContext.RootNodeId))
				.Returns(new ContentTreeNode[]
				         	{
				         		new ContentTreeNode()
				         			{
				         				Id = "1",
										UrlSegment = "Section1"
				         			}, 
							});
			mocker.GetMock<IContentTreeNodeContext>().Setup(a => a.GetContentTreeNodesByTreeId("1"))
				.Returns(new Models.ContentTreeNode[]
				         	{
				         		new Models.ContentTreeNode()
				         			{
				         				Action = "Index",
										Body = "page content",
										HeaderText = "section1"
				         			}, 
							});
			var routeData = new RouteData();
			routeData.Values.Add("Action", "Index");

			var result = mocker.Resolve<ContentTreeNodeDisplayViewModelBuilder>().BuildViewModel("Section1/", routeData, "Index");

			Assert.AreEqual("section1", result.Header);
		}

		[TestMethod]
		public void Returns_correct_Body_value_for_first_level_tree_node_url()
		{
			mocker.GetMock<IContentTree>().Setup(a => a.GetChildren(ContentTreeNodeContext.RootNodeId))
				.Returns(new ContentTreeNode[]
				         	{
				         		new ContentTreeNode()
				         			{
				         				Id = "1",
										UrlSegment = "Section1"
				         			}, 
							});
			mocker.GetMock<IContentTreeNodeContext>().Setup(a => a.GetContentTreeNodesByTreeId("1"))
				.Returns(new Models.ContentTreeNode[]
				         	{
				         		new Models.ContentTreeNode()
				         			{
				         				Action = "Index",
										Body = "page content",
				         			}, 
							});
			var routeData = new RouteData();
			routeData.Values.Add("Action", "Index");

			var result = mocker.Resolve<ContentTreeNodeDisplayViewModelBuilder>().BuildViewModel("Section1/", routeData, "Index");

			Assert.AreEqual("page content", result.Body);
		}

		[TestMethod]
		public void Returns_correct_Body_value_for_first_level_tree_node_url_when_there_are_multiple_ContentTreeNodes_for_the_current_url_and_the_action_is_Index2()
		{
		    mocker.GetMock<IContentTree>().Setup(a => a.GetChildren(ContentTreeNodeContext.RootNodeId))
		        .Returns(new ContentTreeNode[]
		                    {
		                        new ContentTreeNode()
		                            {
		                                Id = "1",
		                                UrlSegment = "Section1"
		                            }, 
		                    });
			mocker.GetMock<IGetParentRouteDataDictionaryFromChildActionRouteData>()
				.Setup(a => a.GetRouteValues(It.IsAny<RouteData>()))
				.Returns((RouteData q) => q);
		    mocker.GetMock<IContentTreeNodeContext>().Setup(a => a.GetContentTreeNodesByTreeId("1"))
		        .Returns(new Models.ContentTreeNode[]
		                    {
		                        new Models.ContentTreeNode()
		                            {
		                                Action = "Index",
		                                Body = "page content",
		                            }, 
		                        new Models.ContentTreeNode()
		                            {
		                                Action = "Index2",
		                                Body = "page content2",
		                            },
		                        new Models.ContentTreeNode()
		                            {
		                                Action = "Index3",
		                                Body = "page content3",
		                            },
		                    });
			var routeData = new RouteData();
			routeData.Values.Add("Action", "Index2");

		    var result = mocker.Resolve<ContentTreeNodeDisplayViewModelBuilder>().BuildViewModel("Section1/", routeData, "Index2");

		    Assert.AreEqual("page content2", result.Body);
		}

		[TestMethod]
		public void Returns_emtpy_view_model_when_url_doesnt_match_a_tree_node_url()
		{
			var result = mocker.Resolve<ContentTreeNodeDisplayViewModelBuilder>().BuildViewModel(null, null, "Index");

			Assert.AreEqual(string.Empty, result.Header);
			Assert.AreEqual(string.Empty, result.Body);
		}
	}
}
