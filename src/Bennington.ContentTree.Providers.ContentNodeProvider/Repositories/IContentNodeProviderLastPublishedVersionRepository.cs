using System;
using Bennington.ContentTree.Providers.ContentNodeProvider.Data;
using Bennington.Core.Helpers;
using Bennington.Repository;
using Bennington.Repository.Helpers;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Repositories
{
    public interface IContentNodeProviderLastPublishedVersionRepository
    {
        void Create(ContentNodeProviderPublishedVersion lastPublishedVersion);
        void Delete(Guid pageId);
    }

    public class ContentNodeProviderLastPublishedVersionRepository : ObjectStore<ContentNodeProviderLastPublishedVersion>
    {
        public ContentNodeProviderLastPublishedVersionRepository(IXmlFileSerializationHelper xmlFileSerializationHelper, IGetDataPathForType getDataPathForType, IGetValueOfIdPropertyForInstance getValueOfIdPropertyForInstance, IGuidGetter guidGetter, IFileSystem fileSystem) : base(xmlFileSerializationHelper, getDataPathForType, getValueOfIdPropertyForInstance, guidGetter, fileSystem)
        {
        }        
    }

    
}