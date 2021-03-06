﻿using System.Web.Configuration;
using Sdl.Web.Common;
using Sdl.Web.Common.Interfaces;
using Sdl.Web.Common.Logging;
using Sdl.Web.Common.Models;
using Sdl.Web.Common.Models.Navigation;
using Sdl.Web.DataModel;
using Sdl.Web.ModelService;
using Sdl.Web.ModelService.Request;

namespace Sdl.Web.Tridion.ModelService
{   
    /// <summary>
    /// Model Service Provider uses the DXA model service to request page, entity and navigation data objects
    /// that are used to build data model objects using the data model builder pipeline.
    /// </summary>
    public class DefaultModelServiceProvider : IModelServiceProvider
    {
        private const string ModelServiceName = "DXA Model Service";
        private const int DefaultRetryCount = 4;
        private const int DefaultTimeout = 60000;
        private readonly ModelServiceClient _modelServiceClient;
        private readonly Binder _binder;
       
        public DefaultModelServiceProvider()
        {
            string uri = WebConfigurationManager.AppSettings["model-service-uri"];          
            int n;
            int retryCount = int.TryParse(
                WebConfigurationManager.AppSettings["model-service-retries"], out n)
                ? n
                : DefaultRetryCount;

            int timeout = int.TryParse(
                WebConfigurationManager.AppSettings["model-service-timeout"], out n)
                ? n
                : DefaultTimeout;
            _modelServiceClient = new ModelServiceClient(uri, retryCount, timeout);
            _binder = new Binder();
        }    

        /// <summary>
        /// Adds a new data model extension to handle deserialization.
        /// </summary>
        /// <param name="extension">Extension.</param>      
        public void AddDataModelExtension(IDataModelExtension extension)
        {
            _binder.AddDataModelExtension(extension);
        }

        /// <summary>
        /// Get page model data object.
        /// </summary>
        public PageModelData GetPageModelData(string urlPath, ILocalization localization, bool addIncludes)
        {
            try
            {                                           
                PageModelRequest request = new PageModelRequest
                {
                    Path = urlPath,
                    CmUriScheme = localization.CmUriScheme,
                    PublicationId = int.Parse(localization.Id),
                    PageInclusion = addIncludes ? PageInclusion.INCLUDE : PageInclusion.EXCLUDE,
                    Binder = _binder
                };

                ModelServiceResponse<PageModelData> response = _modelServiceClient.PerformRequest<PageModelData>(request);
                if (response.Response != null) response.Response.SerializationHashCode = response.Hashcode;
                return response.Response;
            }
            catch (ModelServiceException e)
            {
                Log.Error("{0} returned an unexpected response for URL '{1}':\n{2} ", ModelServiceName, _modelServiceClient.ModelServiceBaseUri,
                   e.Message);
                throw new DxaException($"{ModelServiceName} returned an unexpected response.", e);
            }
            catch (ItemNotFoundException)
            {
                return null;
            }
        }
    
        public PageModelData GetPageModelData(int pageId, ILocalization localization, bool addIncludes)
        {
            try
            {
                PageModelRequest request = new PageModelRequest
                {
                    CmUriScheme = localization.CmUriScheme,
                    PublicationId = int.Parse(localization.Id),
                    PageId = pageId,
                    PageInclusion = addIncludes ? PageInclusion.INCLUDE : PageInclusion.EXCLUDE,                    
                    Binder = _binder
                };

                ModelServiceResponse<PageModelData> response = _modelServiceClient.PerformRequest<PageModelData>(request);
                if (response.Response != null) response.Response.SerializationHashCode = response.Hashcode;
                return response.Response;
            }
            catch (ModelServiceException e)
            {
                Log.Error("{0} returned an unexpected response for URL '{1}':\n{2} ", ModelServiceName, _modelServiceClient.ModelServiceBaseUri,
                   e.Message);
                throw new DxaException($"{ModelServiceName} returned an unexpected response.", e);
            }
            catch (ItemNotFoundException)
            {
                return null;
            }
        }

        /// <summary>
        /// Get entity model data object.
        /// </summary>
        public EntityModelData GetEntityModelData(string entityId, ILocalization localization)
        {
            try
            {
                EntityModelRequest request = new EntityModelRequest
                {
                    EntityId = entityId,
                    CmUriScheme = localization.CmUriScheme,
                    PublicationId = int.Parse(localization.Id),
                    Binder = _binder                 
                };

                ModelServiceResponse<EntityModelData> response = _modelServiceClient.PerformRequest<EntityModelData>(request);
                if (response.Response != null) response.Response.SerializationHashCode = response.Hashcode;
                return response.Response;
            }
            catch (ModelServiceException e)
            {
                Log.Error("{0} returned an unexpected response for URL '{1}':\n{2} ", ModelServiceName, _modelServiceClient.ModelServiceBaseUri,
                   e.Message);
                throw new DxaException($"{ModelServiceName} returned an unexpected response.", e);
            }
            catch (ItemNotFoundException)
            {
                return null;
            }
        }

        /// <summary>
        /// Get site map item.
        /// </summary>
        public TaxonomyNode GetSitemapItem(ILocalization localization)
        {
            try
            {
                SitemapItemModelRequest request = new SitemapItemModelRequest
                {
                    PublicationId = int.Parse(localization.Id),
                    Binder = _binder
                };
                return _modelServiceClient.PerformRequest<TaxonomyNode>(request).Response;
            }
            catch (ModelServiceException e)
            {
                Log.Error("{0} returned an unexpected response for URL '{1}':\n{2} ", ModelServiceName, _modelServiceClient.ModelServiceBaseUri,
                   e.Message);
                throw new DxaException($"{ModelServiceName} returned an unexpected response.", e);
            }
            catch (ItemNotFoundException)
            {
                return null;
            }
        }

        /// <summary>
        /// Get child site map items.
        /// </summary>
        public SitemapItem[] GetChildSitemapItems(string parentSitemapItemId, ILocalization localization,
            bool includeAncestors, int descendantLevels)
        {
            try
            {
                SitemapChildItemModelRequest request = new SitemapChildItemModelRequest
                {
                    PublicationId = int.Parse(localization.Id),
                    ParentSitemapItemId = parentSitemapItemId,
                    IncludeAncestors = includeAncestors,
                    DescendantLevels = descendantLevels,
                    Binder = _binder
                };
                return _modelServiceClient.PerformRequest<SitemapItem[]>(request).Response;
            }
            catch (ModelServiceException e)
            {
                Log.Error("{0} returned an unexpected response for URL '{1}':\n{2} ", ModelServiceName, _modelServiceClient.ModelServiceBaseUri,
                   e.Message);
                throw new DxaException($"{ModelServiceName} returned an unexpected response.", e);
            }
            catch (ItemNotFoundException)
            {
                return null;
            }
        }
    }
}
