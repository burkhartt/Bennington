﻿using System;
using Bennington.ContentTree.Models;

namespace Bennington.ContentTree.Providers.SectionNodeProvider.Models
{
	public class ContentTreeSectionNode : IContentTreeNode
	{
		public string SectionId { get; set; }
		public string TreeNodeId { get; set; }
		public string Name { get; set; }
		public string UrlSegment { get; set; }
		public string DefaultTreeNodeId { get; set; }
		public int? Sequence { get; set; }
		public bool Inactive { get; set; }

	    public string IconUrl
	    {
            get { return "Content/SectionNodeProvider/section.png"; }
	        set { throw new NotImplementedException(); }
	    }

	    public DateTime LastModifyDate { get; set; }
	    public string LastModifyBy { get; set; }

	    public bool Hidden { get; set; }
	}
}
