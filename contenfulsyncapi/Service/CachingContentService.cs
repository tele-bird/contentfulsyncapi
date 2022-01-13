using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using contenfulsyncapi.Model;
using Contentful.Core.Models;
using MonkeyCache.FileStore;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace contenfulsyncapi.Service
{
    public class CachingContentService
    {
        private ContentfulClient mContentfulClient;

        public CachingContentService()
        {
            Barrel.ApplicationId = AppInfo.PackageName;
            if(null != AppSettings)
            {
                mContentfulClient = new ContentfulClient(AppSettings.SpaceId, AppSettings.AccessToken, AppSettings.Environment);
            }
        }

        private ContentfulAppSettings mAppSettings;
        public ContentfulAppSettings AppSettings
        {
            get
            {
                if (null == mAppSettings)
                {
                    mAppSettings = Barrel.Current.Get<ContentfulAppSettings>("settings");
                }
                return mAppSettings;
            }
            set
            {
                if (null == value)
                {
                    Barrel.Current.Empty("settings");
                    mAppSettings = null;
                    mContentfulClient = null;
                }
                else
                {
                    if (!value.Equals(mAppSettings))
                    {
                        mAppSettings = value;
                        mContentfulClient = new ContentfulClient(mAppSettings.SpaceId, mAppSettings.AccessToken, mAppSettings.Environment);
                        Barrel.Current.Add<ContentfulAppSettings>("settings", mAppSettings, TimeSpan.MaxValue);
                    }
                }
            }
        }

        private ContentfulInitialContentSettings mInitialContentSettings;
        public ContentfulInitialContentSettings InitialContentSettings
        {
            get
            {
                if (null == mInitialContentSettings)
                {
                    mInitialContentSettings = Barrel.Current.Get<ContentfulInitialContentSettings>("initialContentSettings");
                }
                return mInitialContentSettings;
            }
            set
            {
                if (null != mInitialContentSettings)
                {
                    // clean out previous initial content, if it exists:
                    // clear old content type caches:
                    foreach (string contentTypeId in mInitialContentSettings.SelectedContentTypeIds)
                    {
                        Barrel.Current.Empty(contentTypeId);
                    }
                    // clear initialContentSettings:
                    Barrel.Current.Empty("initialContentSettings");
                }

                // store new settings:
                mInitialContentSettings = value;
                if(null != mInitialContentSettings)
                {
                    Barrel.Current.Add<ContentfulInitialContentSettings>("initialContentSettings", mInitialContentSettings, TimeSpan.MaxValue);
                }
            }
        }

        public async Task<IEnumerable<ContentType>> GetContentTypesAsync()
        {
            if(null == mContentfulClient)
            {
                throw new Exception("The AppSettings must be set before invoking this method.");
            }
            return await mContentfulClient.RequestContentTypesNET();
        }

        public async Task<EntryCollectionShell> GetEntriesByContentTypeAsync(string contentTypeId)
        {
            // return var:
            EntryCollectionShell result = null;

            //result = await GetEntriesByContentTypeFromCMSAsync(contentTypeId, contentfulInitialContentSettings);

            if (!Barrel.Current.Exists(contentTypeId))
            {
                // data is absent, so fetch the "initial" data from the CMS:
                result = await GetEntriesByContentTypeFromCMSAsync(contentTypeId);

                // serialize the result to JSON:
                string json = JsonConvert.SerializeObject(result);
                Debug.WriteLine(json);

                // add it to the cache on disk:
                //Barrel.Current.Add<EntryCollectionShell>(contentTypeId, result, TimeSpan.FromMinutes(contentfulInitialContentSettings.ExpirationMinutes));
                Barrel.Current.Add(contentTypeId, json, TimeSpan.FromMinutes(InitialContentSettings.ExpirationMinutes));
            }
            else
            {
                // return empty delta if its not expired:
                if (!Barrel.Current.IsExpired(contentTypeId))
                {
                    // not expired yet, so return an empty collection:
                    result = new EntryCollectionShell();
                }
                else
                {
                    // get the expired cache on disk:
                    string json = Barrel.Current.Get<string>(contentTypeId);

                    // deserialize the cached data:
                    //result = Barrel.Current.Get<EntryCollectionShell>(contentTypeId);
                    result = JsonConvert.DeserializeObject<EntryCollectionShell>(json);

                    // data is absent or expired, so fetch the "delta" from the CMS:
                    EntryCollectionShell delta = await GetDeltaFromCMSAsync(result.DeltaUrl);

                    result.UpdateWithDelta(delta);

                    // replace the cached content on disk:
                    Barrel.Current.Empty(contentTypeId);
                    Barrel.Current.Add<EntryCollectionShell>(contentTypeId, result, TimeSpan.FromMinutes(InitialContentSettings.ExpirationMinutes));
                }
            }

            // return result:
            return result;
        }

        private async Task<EntryCollectionShell> GetDeltaFromCMSAsync(string deltaUrl)
        {
            SyncResult syncResult = await mContentfulClient.RequestDeltaOrNextPageNET(deltaUrl);
            return ConstructEntryCollectionShell(syncResult);
        }

        private async Task<EntryCollectionShell> GetEntriesByContentTypeFromCMSAsync(string contentTypeId)
        {
            SyncResult syncResult = await mContentfulClient.RequestInitialSyncNET(SyncType.Entry, contentTypeId, true);
            return ConstructEntryCollectionShell(syncResult);
        }

        private EntryCollectionShell ConstructEntryCollectionShell(SyncResult syncResult)
        {
            List<EntryShell> entryShells = new List<EntryShell>();
            foreach (Entry<dynamic> entry in syncResult.Entries)
            {
                entryShells.Add(new EntryShell(entry));
            }
            DateTime now = DateTime.UtcNow;
            return new EntryCollectionShell
            {
                EntryShells = entryShells,
                LastUpdatedUtc = now,
                ExpiresUtc = now.AddMinutes(InitialContentSettings.ExpirationMinutes),
                DeltaUrl = syncResult.NextSyncUrl
            };
        }
    }
}
