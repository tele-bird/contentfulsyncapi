using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Contentful.Core.Configuration;
using Contentful.Core.Models;

namespace contenfulsyncapi.Service
{
    public class ContentfulClient
    {
        private ContentfulOptions options = null;

        public ContentfulClient(string spaceId, string accessToken, string environment)
        {
            options = new ContentfulOptions { DeliveryApiKey = accessToken, Environment = environment, SpaceId = spaceId };
        }

        //public async Task<ContentTypesResponse> RequestContentTypes()
        //{
        //    string response = null;
        //    using(var httpClient = new HttpClient())
        //    {
        //        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", options.DeliveryApiKey);
        //        Uri requestUrl = new Uri($"https://cdn.contentful.com/spaces/{options.SpaceId}/environments/{options.Environment}/content_types");
        //        response = await httpClient.GetStringAsync(requestUrl);
        //    }
        //    return ContentTypesResponse.FromJson(response);
        //}

        //public async Task<EntriesResponse> RequestInitialSync(string type, contentfulsyncapi.Dto.ContentModel.ContentType contentType)
        //{
        //    string response = null;
        //    using (var httpClient = new HttpClient())
        //    {
        //        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", options.DeliveryApiKey);
        //        StringBuilder sbRequestUrl = new StringBuilder($"https://cdn.contentful.com/spaces/{options.SpaceId}/environments/{options.Environment}/sync?initial=true");
        //        if (!type.Equals(ALL_TYPES_STRING)) sbRequestUrl.Append($"&type={type}");
        //        if (!contentType.Name.Equals(ALL_CONTENT_TYPES_NAME_STRING)) sbRequestUrl.Append($"&content_type={contentType}");
        //        Uri requestUrl = new Uri(sbRequestUrl.ToString());
        //        response = await httpClient.GetStringAsync(requestUrl);
        //    }
        //    return EntriesResponse.FromJson(response);
        //}

        //public async Task<EntriesResponse> Request(Uri url)
        //{
        //    string response = null;
        //    using (var httpClient = new HttpClient())
        //    {
        //        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", options.DeliveryApiKey);
        //        response = await httpClient.GetStringAsync(url);
        //    }
        //    return EntriesResponse.FromJson(response);
        //}

        public async Task<IEnumerable<ContentType>> RequestContentTypesNET()
        {
            using (var httpClient = new HttpClient())
            {
                var client = new Contentful.Core.ContentfulClient(httpClient, options);
                IEnumerable<ContentType> contentTypes = await client.GetContentTypes();
                List<ContentType> myContentTypes = new List<ContentType>();
                foreach(ContentType contentType in contentTypes)
                {
                    myContentTypes.Add(contentType);
                }
                return myContentTypes;
            }
        }

        public async Task<SyncResult> RequestInitialSyncNET(SyncType syncType, string contentTypeId, bool recursive)
        {
            //if((null != contentType) && (syncType != SyncType.Entry))
            //{
            //    throw new Exception("When passing a content_type the type must be set to Entry.");
            //}
            using(var httpClient = new HttpClient())
            {
                var client = new Contentful.Core.ContentfulClient(httpClient, options);
                if(recursive)
                {
                    return await client.SyncInitialRecursive(syncType, contentTypeId);
                }
                else
                {
                    return await client.SyncInitial(syncType, contentTypeId);
                }
            }
        }

        public async Task<SyncResult> RequestDeltaOrNextPageNET(string url)
        {
            using(var httpClient = new HttpClient())
            {
                var client = new Contentful.Core.ContentfulClient(httpClient, options);
                return await client.SyncNextResult(url);
            }
        }
    }
}
