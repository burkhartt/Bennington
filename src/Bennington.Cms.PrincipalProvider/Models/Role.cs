using System.Collections.Generic;
using Bennington.Core.List;

namespace Bennington.Cms.PrincipalProvider.Models
{
    public class Role
    {
        [Hidden]
        public string Id { get; set; }

        public string Name { get; set; }

        [Hidden]
        public bool Inactive { get; set; }

        [Hidden]
        public bool AllContent { get; set; }

        [Hidden]
        public List<string> AvailableContentSections { get; set; }

        [Hidden]
        public List<string> AvailableModules { get; set; }
    }
}