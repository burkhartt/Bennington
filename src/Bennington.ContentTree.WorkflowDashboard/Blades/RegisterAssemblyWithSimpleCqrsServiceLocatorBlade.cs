using Bennington.ContentTree.WorkflowDashboard.Denormalizers;
using Bennington.ContentTree.WorkflowDashboard.Repositories;
using Bennington.Core;
using Bennington.Core.Helpers;
using MvcTurbine;
using MvcTurbine.Blades;
using SimpleCqrs;

namespace Bennington.ContentTree.WorkflowDashboard.Blades
{
	public class RegisterAssemblyWithSimpleCqrsServiceLocatorBlade : IBlade
	{
		private readonly IServiceLocator simpleCqrsServiceLocator;

		public RegisterAssemblyWithSimpleCqrsServiceLocatorBlade(SimpleCqrs.IServiceLocator simpleCqrsServiceLocator)
		{
			this.simpleCqrsServiceLocator = simpleCqrsServiceLocator;
		}

		public void Dispose()
		{
		}

		public void Initialize(IRotorContext context)
		{
		}

		public void Spin(IRotorContext context)
		{
            ServiceLocator.Current.Register<IWorkflowItemRepository, WorkflowItemRepository>();
            simpleCqrsServiceLocator.Register(context.ServiceLocator.Resolve<WorkflowDenormalizer>);
		}
	}
}