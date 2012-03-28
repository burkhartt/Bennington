using System;
using System.Collections.Generic;
using System.Linq;
using Bennington.Cms.PrincipalProvider.Models;

namespace Bennington.Cms.PrincipalProvider.Repositories
{
    public interface IModuleRepository
    {
        IQueryable<Module> GetAll();
    }

    //public class GenericModuleRepository : IModuleRepository
    //{
    //    public IQueryable<Module> GetAll()
    //    {
    //        return new List<Module>().AsQueryable();
    //    }
    //}
}