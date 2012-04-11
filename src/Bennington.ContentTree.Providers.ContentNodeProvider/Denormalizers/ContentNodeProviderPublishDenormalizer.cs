using System;
using System.Linq;
using Bennington.ContentTree.Domain.Events.Page;
using Bennington.ContentTree.Providers.ContentNodeProvider.Data;
using Bennington.ContentTree.Providers.ContentNodeProvider.Mappers;
using Bennington.ContentTree.Providers.ContentNodeProvider.Repositories;
using Bennington.Core.Helpers;
using SimpleCqrs.Eventing;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Denormalizers
{
	public class ContentNodeProviderPublishDenormalizer : IHandleDomainEvents<PagePublishedEvent>,
															IHandleDomainEvents<PageDeletedEvent>
	{
		private readonly IContentNodeProviderPublishedVersionRepository contentNodeProviderPublishedVersionRepository;
		private readonly IContentNodeProviderDraftToContentNodeProviderPublishedVersionMapper contentNodeProviderDraftToContentNodeProviderPublishedVersionMapper;
		private readonly IContentNodeProviderDraftRepository contentNodeProviderDraftRepository;
        private readonly ContentNodeProviderLastPublishedVersionRepository contentNodeProviderLastPublishedVersionRepository;

	    public ContentNodeProviderPublishDenormalizer(IContentNodeProviderPublishedVersionRepository contentNodeProviderPublishedVersionRepository,
			IContentNodeProviderDraftToContentNodeProviderPublishedVersionMapper contentNodeProviderDraftToContentNodeProviderPublishedVersionMapper,
			IContentNodeProviderDraftRepository contentNodeProviderDraftRepository,
            ContentNodeProviderLastPublishedVersionRepository contentNodeProviderLastPublishedVersionRepository)
		{
			this.contentNodeProviderDraftRepository = contentNodeProviderDraftRepository;
		    this.contentNodeProviderLastPublishedVersionRepository = contentNodeProviderLastPublishedVersionRepository;
		    this.contentNodeProviderDraftToContentNodeProviderPublishedVersionMapper = contentNodeProviderDraftToContentNodeProviderPublishedVersionMapper;
			this.contentNodeProviderPublishedVersionRepository = contentNodeProviderPublishedVersionRepository;
		}

		public void Handle(PagePublishedEvent domainEvent)
		{
		    var pageToPublish = contentNodeProviderPublishedVersionRepository.GetAllContentNodeProviderPublishedVersions().FirstOrDefault(x => x.PageId == domainEvent.AggregateRootId.ToString());
            if (pageToPublish != null)
            {
                contentNodeProviderLastPublishedVersionRepository.Delete(pageToPublish.PageId);
                contentNodeProviderLastPublishedVersionRepository.SaveAndReturnId(new ContentNodeProviderLastPublishedVersion
                                                                                      {
                                                                                          Id = Guid.NewGuid(),
                                                                                          Action = pageToPublish.Action,
                                                                                          Body = pageToPublish.Body,
                                                                                          HeaderImage = pageToPublish.HeaderImage,
                                                                                          HeaderText = pageToPublish.HeaderText,
                                                                                          Hidden = pageToPublish.Hidden,
                                                                                          Inactive = pageToPublish.Inactive,
                                                                                          IsNew = pageToPublish.IsNew,
                                                                                          LastModifyBy = pageToPublish.LastModifyBy,
                                                                                          LastModifyDate = pageToPublish.LastModifyDate,
                                                                                          MetaDescription = pageToPublish.MetaDescription,
                                                                                          MetaKeywords = pageToPublish.MetaKeywords,
                                                                                          MetaTitle = pageToPublish.MetaTitle,
                                                                                          Name = pageToPublish.Name,
                                                                                          PageId = pageToPublish.PageId,
                                                                                          Sequence = pageToPublish.Sequence,
                                                                                          TreeNodeId = pageToPublish.TreeNodeId,
                                                                                          UrlSegment = pageToPublish.UrlSegment,
                                                                                          WorkflowStatus = pageToPublish.WorkflowStatus
                                                                                      });
            }

		    var contentNodeProviderDrafts = contentNodeProviderDraftRepository.GetAllContentNodeProviderDrafts().ToArray();
			var draftVersion = contentNodeProviderDrafts.Where(a => a.PageId == domainEvent.AggregateRootId.ToString()).FirstOrDefault();
			if (draftVersion == null) return;

			var existingPublishedVersion = contentNodeProviderPublishedVersionRepository.GetAllContentNodeProviderPublishedVersions()
				.Where(a => a.PageId == domainEvent.AggregateRootId.ToString()).FirstOrDefault();

            //var draftFileUploadPath = applicationSettingsValueGetter.GetValue("Bennington.ContentTree.Providers.ContentNodeProvider.DraftFileUploadPath");
            //var publishedVersionFileUploadPath = applicationSettingsValueGetter.GetValue("Bennington.ContentTree.Providers.ContentNodeProvider.PublishedVersionFileUploadPath");
            //var draftVersionHeaderImagePath = string.Format(@"{0}{1}\HeaderImage\{2}", draftFileUploadPath, domainEvent.AggregateRootId, draftVersion.HeaderImage);
            //if (!fileSystem.DirectoryExists(publishedVersionFileUploadPath + domainEvent.AggregateRootId + @"\HeaderImage"))
            //    fileSystem.CreateFolder(publishedVersionFileUploadPath + domainEvent.AggregateRootId + @"\HeaderImage");
            //if (fileSystem.FileExists(draftVersionHeaderImagePath))
            //    fileSystem.Copy(draftVersionHeaderImagePath,
            //                    string.Format(@"{0}{1}\HeaderImage\{2}", publishedVersionFileUploadPath, domainEvent.AggregateRootId, draftVersion.HeaderImage));

			if (existingPublishedVersion == null)
				contentNodeProviderPublishedVersionRepository
					.Create(contentNodeProviderDraftToContentNodeProviderPublishedVersionMapper.CreateInstance(draftVersion));
			else
			{
				var mappedInstance = contentNodeProviderDraftToContentNodeProviderPublishedVersionMapper.CreateInstance(draftVersion);
				contentNodeProviderPublishedVersionRepository.Update(mappedInstance);
			}
		}

		public void Handle(PageDeletedEvent domainEvent)
		{
			var item = contentNodeProviderPublishedVersionRepository
							.GetAllContentNodeProviderPublishedVersions()
								.Where(a => a.TreeNodeId == domainEvent.TreeNodeId.ToString()).FirstOrDefault();

			if (item != null)
				contentNodeProviderPublishedVersionRepository.Delete(item);
		}
	}    
}
