﻿using System.Linq;
using AutoMoq;
using Bennington.ContentTree.Models;
using Bennington.ContentTree.TreeManager.ViewModelBuilders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bennington.ContentTree.TreeManager.Tests.ViewModelBuilders
{
	[TestClass]
	public class TreeBranchViewModelBuilderTests_BuildViewModel
	{
		private AutoMoqer mocker;

		[TestInitialize]
		public void INit()
		{
			mocker = new AutoMoqer();
		}

		[TestMethod]
		public void Sets_child_tree_node_name_to_UNKNOWN_when_the_name_is_null_or_blank()
		{
			mocker.GetMock<IContentTree>().Setup(a => a.GetChildren("1"))
				.Returns(new ContentTreeNode[]
				         	{
				         		new ContentTreeNode()
				         			{
				         				
				         			}, 
							});

			var treeBranchViewModelBuilder = mocker.Resolve<TreeBranchViewModelBuilder>();
			var result = treeBranchViewModelBuilder.BuildViewModel("1");

			Assert.AreEqual("Unknown", result.TreeNodeSummaries.First().ContentTreeNode.Name);
		}

		[TestMethod]
		public void Returns_child_tree_node_summaries_of_specified_parent_node()
		{
			mocker.GetMock<IContentTree>().Setup(a => a.GetChildren("1"))
				.Returns(new ContentTreeNode[]
				         	{
				         		new ContentTreeNode()
				         			{
				         				
				         			}, 
							});

			var treeBranchViewModelBuilder = mocker.Resolve<TreeBranchViewModelBuilder>();
			var result = treeBranchViewModelBuilder.BuildViewModel("1");

			Assert.AreEqual(1, result.TreeNodeSummaries.Count());
		}

		[TestMethod]
		public void Returns_empty_view_model_when_id_does_not_exist()
		{
			var treeBranchViewModelBuilder = mocker.Resolve<TreeBranchViewModelBuilder>();
			var result = treeBranchViewModelBuilder.BuildViewModel(null);

			Assert.AreEqual(0, result.TreeNodeSummaries.Count());
		}

        [TestMethod]
        public void Sets_IsActive_property_to_true_when_item_is_active()
        {
            mocker.GetMock<IContentTree>().Setup(a => a.GetChildren("1"))
                .Returns(new ContentTreeNode[]
				         	{
				         		new ContentTreeNode()
				         			{
				         				Active = true,
				         			}, 
							});

            var treeBranchViewModelBuilder = mocker.Resolve<TreeBranchViewModelBuilder>();
            var result = treeBranchViewModelBuilder.BuildViewModel("1");

            Assert.IsTrue(result.TreeNodeSummaries.First().ContentTreeNode.Active);
        }

        [TestMethod]
        public void Sets_IsActive_property_to_false_when_item_is_not_active()
        {
            mocker.GetMock<IContentTree>().Setup(a => a.GetChildren("1"))
                .Returns(new ContentTreeNode[]
				         	{
				         		new ContentTreeNode()
				         			{
				         				Active = false,
				         			}, 
							});

            var treeBranchViewModelBuilder = mocker.Resolve<TreeBranchViewModelBuilder>();
            var result = treeBranchViewModelBuilder.BuildViewModel("1");

            Assert.IsFalse(result.TreeNodeSummaries.First().ContentTreeNode.Active);
        }
	}
}
