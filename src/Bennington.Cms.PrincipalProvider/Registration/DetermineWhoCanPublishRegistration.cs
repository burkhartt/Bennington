﻿using Bennington.Cms.PrincipalProvider.Helpers;
using Bennington.ContentTree.Providers.ContentNodeProvider.Helpers;
using MvcTurbine.ComponentModel;

namespace Bennington.Cms.PrincipalProvider.Registration
{
    public class DetermineWhoCanPublishRegistration : IServiceRegistration
    {
        public void Register(IServiceLocator locator)
        {
            locator.Register<IDetermineWhoHasTheAbilityToPublish, DetermineWhoHasTheAbilityToPublish>();
        }
    }
}