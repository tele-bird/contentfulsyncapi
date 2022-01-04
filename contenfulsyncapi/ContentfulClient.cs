using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Contentful.Core.Configuration;
using Contentful.Core.Models;

namespace contenfulsyncapi
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

        public async Task<IEnumerable<Contentful.Core.Models.ContentType>> RequestContentTypesNET()
        {
            using (var httpClient = new HttpClient())
            {
                var client = new Contentful.Core.ContentfulClient(httpClient, options);
                return await client.GetContentTypes();
            }
        }

        public async Task<SyncResult> RequestInitialSyncNET(SyncType syncType, Contentful.Core.Models.ContentType contentType, bool recursive)
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
                    return await client.SyncInitialRecursive(syncType, contentType?.SystemProperties.Id);
                }
                else
                {
                    return await client.SyncInitial(syncType, contentType?.SystemProperties.Id);
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
