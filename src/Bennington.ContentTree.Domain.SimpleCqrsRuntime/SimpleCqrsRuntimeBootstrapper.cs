﻿using System;
using System.Configuration;
using MvcTurbine.ComponentModel;
using SimpleCqrs;
using SimpleCqrs.Commanding;
using SimpleCqrs.EventStore.SqlServer;
using SimpleCqrs.Eventing;
using SimpleCqrs.EventStore.SqlServer.Serializers;
using IServiceLocator = MvcTurbine.ComponentModel.IServiceLocator;

namespace Bennington.ContentTree.Domain.SimpleCqrsRuntime
{
	public class SimpleCqrsRuntimeBootstrapper : IServiceRegistration
	{
	    private readonly IServiceLocator serviceLocator;

        public SimpleCqrsRuntimeBootstrapper(IServiceLocator serviceLocator)
	    {
	        this.serviceLocator = serviceLocator;
	    }

        public void Register(IServiceLocator serviceLocator)
	    {
            try
            {
                serviceLocator.Resolve<ISimpleCqrsRuntime>();
                return;
            }
            catch (Exception exception)
            {

            }

            RegisterAndStartRuntime();
	    }

        void RegisterAndStartRuntime()
        {
            var settings = ConfigurationManager.ConnectionStrings["Bennington.ContentTree.Domain.ConnectionString"];

            if (settings == null)
                throw new Exception("Cannot find connection string for 'Bennington.ContentTree.Domain.ConnectionString' in the configuration file");

            var runtime = new BenningtonContentTreeSimpleCqrsRuntime();

            runtime.Start();

            runtime.ServiceLocator.Register<IEventStore>(
                new SqlServerEventStore(
                    new SqlServerConfiguration(settings.ToString()),
                    new JsonDomainEventSerializer()));

            serviceLocator.Register(runtime.ServiceLocator.Resolve<ICommandBus>());

            serviceLocator.Register<SimpleCqrs.IServiceLocator>(runtime.ServiceLocator);
        }
	}
}
