using System.Collections.Generic;
using System.Linq;
using Bennington.Cms.PrincipalProvider.Models;
using Bennington.Core.Helpers;
using Bennington.Repository;
using Bennington.Repository.Helpers;

namespace Bennington.Cms.PrincipalProvider.Repositories
{
    public interface IRoleRepository
    {
        IEnumerable<Role> GetAll();
        string SaveAndReturnId(Role role);
        void Delete(string id);
        Role GetById(string id);
    }

    public class RoleRepository : ObjectStore<Role>, IRoleRepository
    {
        public RoleRepository(IXmlFileSerializationHelper xmlFileSerializationHelper, IGetDataPathForType getDataPathForType, IGetValueOfIdPropertyForInstance getValueOfIdPropertyForInstance, IGuidGetter guidGetter, IFileSystem fileSystem) : base(xmlFileSerializationHelper, getDataPathForType, getValueOfIdPropertyForInstance, guidGetter, fileSystem)
        {
        } 
    }
}