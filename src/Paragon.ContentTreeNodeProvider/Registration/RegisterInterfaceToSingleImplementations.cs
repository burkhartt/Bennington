using System.Collections.Generic;
using System.Reflection;
using Paragon.Core.Registration;

namespace Paragon.ContentTreeNodeProvider.Registration
{
    public class RegisterInterfaceToSingleImplementations : InterfaceToSingleImplementationRegistrationConvention
    {
		public RegisterInterfaceToSingleImplementations()
		{
			
		}

        protected override IEnumerable<Assembly> GetAssembliesToScan()
        {
            return new[] { Assembly.GetExecutingAssembly() };
        }
    }
}