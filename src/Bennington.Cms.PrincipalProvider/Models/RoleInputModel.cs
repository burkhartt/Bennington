using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace Bennington.Cms.PrincipalProvider.Models
{
    public class RoleInputModel
    {
        public RoleInputModel()
        {
            AvailableContentSections = new[] { "" };
            AvailableModules = new[] { "" };
        }

        [HiddenInput(DisplayValue = false)]
        public string Id { get; set; }

        [DisplayName("Make Inactive")]
        public bool Inactive { get; set; }

        public string Name { get; set; }

        [DisplayName("All Content")]
        public bool AllContent { get; set; }
        
        [DisplayName("Available Content Sections")]
        public IEnumerable<string> AvailableContentSections { get; set; }

        [DisplayName("Available Modules")]
        public IEnumerable<string> AvailableModules { get; set; }

        public IEnumerable<SelectListItem> ContentSections { get; set; }

        public IEnumerable<SelectListItem> Modules { get; set; }
    }
}