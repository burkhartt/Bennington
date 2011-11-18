﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using AutoMoq;
using Bennington.ContentTree.Contexts;
using Bennington.ContentTree.Models;
using Bennington.ContentTree.TreeManager.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Action = Bennington.ContentTree.Models.Action;

namespace Bennington.ContentTree.TreeManager.Tests.Routing
{
	[TestClass]
	public class RegisterRoutesForAllProvidersTests_Register
	{
		private AutoMoqer mocker;

		[TestInitialize]
		public void Init()
		{
			mocker = new AutoMoqer();
		}

		[TestMethod]
		public void Registers_a_route_to_the_providers_ControllerToUseForCreate_with_correct_url()
		{
			mocker.GetMock<ITreeNodeProviderContext>().Setup(a => a.GetAllTreeNodeProviders())
				.Returns(new IAmATreeNodeExtensionProvider[]
				         	{
				         		new FakeTreeNodeExtensionProvider()
							});
			var routes = new RouteCollection();
			var httpContext = new Moq.Mock<HttpContextBase>();


			mocker.Resolve<RegisterSmsRoutesForAllProviders>().Register(routes);
			
			var query = from item in routes
						where (((System.Web.Routing.Route)item).Url == "ControllerToUseForCreation/{action}")
						select item;

			Assert.AreEqual(1, query.Count());
		}

		[TestMethod]
		public void Registers_a_route_to_the_providers_ControllerToUseForModification_with_correct_url()
		{
			mocker.GetMock<ITreeNodeProviderContext>().Setup(a => a.GetAllTreeNodeProviders())
				.Returns(new IAmATreeNodeExtensionProvider[]
				         	{
				         		new FakeTreeNodeExtensionProvider()
							});
			var routes = new RouteCollection();
			var httpContext = new Moq.Mock<HttpContextBase>();


			mocker.Resolve<RegisterSmsRoutesForAllProviders>().Register(routes);

			var query = from item in routes
						where (((System.Web.Routing.Route)item).Url == "ControllerToUseForModification/{action}")
						select item;

			Assert.AreEqual(1, query.Count());
		}

		[TestMethod]
		public void Registers_a_route_to_the_providers_ControllerToUseForModification_with_correct_default_values()
		{
			mocker.GetMock<ITreeNodeProviderContext>().Setup(a => a.GetAllTreeNodeProviders())
				.Returns(new IAmATreeNodeExtensionProvider[]
				         	{
				         		new FakeTreeNodeExtensionProvider()
							});
			var routes = new RouteCollection();
			var httpContext = new Moq.Mock<HttpContextBase>();


			mocker.Resolve<RegisterSmsRoutesForAllProviders>().Register(routes);

			var query = from item in routes
						where (((Route)item).Defaults["controller"].ToString() == "ControllerToUseForModification")
						where (((Route)item).Defaults["action"].ToString() == "ActionToUseForModification")
						select item;

			Assert.AreEqual(1, query.Count());
		}

		[TestMethod]
		public void Registers_a_route_to_the_providers_ControllerToUseForCreation_with_correct_default_values()
		{
			mocker.GetMock<ITreeNodeProviderContext>().Setup(a => a.GetAllTreeNodeProviders())
				.Returns(new IAmATreeNodeExtensionProvider[]
				         	{
				         		new FakeTreeNodeExtensionProvider()
							});
			var routes = new RouteCollection();
			var httpContext = new Moq.Mock<HttpContextBase>();


			mocker.Resolve<RegisterSmsRoutesForAllProviders>().Register(routes);

			var query = from item in routes
						where (((Route)item).Defaults["controller"].ToString() == "ControllerToUseForCreation")
						where (((Route)item).Defaults["action"].ToString() == "ActionToUseForCreation")
						select item;

			Assert.AreEqual(1, query.Count());
		}

		public class FakeTreeNodeExtensionProvider : IAmATreeNodeExtensionProvider
		{
			public IQueryable<IContentTreeNode> GetAll()
			{
				throw new NotImplementedException();
			}

			public string Name
			{
				get { throw new NotImplementedException(); }
			}

			public string ControllerToUseForModification
			{
				get { return "ControllerToUseForModification"; }
				set { throw new NotImplementedException(); }
			}

			public string ActionToUseForModification
			{
				get { return "ActionToUseForModification"; }
				set { throw new NotImplementedException(); }
			}

			public string ControllerToUseForCreation
			{
				get { return "ControllerToUseForCreation"; }
				set { throw new NotImplementedException(); }
			}

			public string ActionToUseForCreation
			{
				get { return "ActionToUseForCreation"; }
				set { throw new NotImplementedException(); }
			}

			public IRouteConstraint IgnoreConstraint
			{
				get { throw new NotImplementedException(); }
			}

			public IEnumerable<Action> Actions
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

			public bool MayHaveChildNodes
			{
				get { return false; }
				set { throw new NotImplementedException(); }
			}

		    public string Controller
		    {
		        get { throw new NotImplementedException(); }
		        set { throw new NotImplementedException(); }
		    }

		    public void RegisterRouteForTreeNodeId(string treeNodeId)
			{
				throw new NotImplementedException();
			}
		}
	}
}
