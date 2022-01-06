using System;
using System.Collections.Generic;
using contenfulsyncapi.Dto.DB;
using Contentful.Core.Models;
using MonkeyCache.FileStore;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace contenfulsyncapi
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Barrel.ApplicationId = AppInfo.PackageName;
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        public static SynchronizedEntries SyncEntries { get; private set; } = new SynchronizedEntries();
        //public static SynchronizedContentTypes SyncContentTypes { get; private set; } = new SynchronizedContentTypes();

        //public static Dictionary<string, SyncedAsset> SyncedAssetsById = new Dictionary<string, SyncedAsset>();
        private static ContentfulAppSettings mAppSettings;
        public static ContentfulAppSettings AppSettings
        {
            get
            {
                if(null == mAppSettings)
                {
                    mAppSettings = Barrel.Current.Get<ContentfulAppSettings>("settings");
                }
                return mAppSettings;
            }
            set
            {
                if(null == value)
                {
                    throw new Exception("Cannot set app settings to null.");
                }
                if(!value.Equals(mAppSettings))
                {
                    mAppSettings = value;
                    Barrel.Current.Add<ContentfulAppSettings>("settings", mAppSettings, TimeSpan.MaxValue);
                }
            }
        }

        private static ContentfulInitialContentSettings mInitialContentSettings;
        public static ContentfulInitialContentSettings InitialContentSettings
        {
            get
            {
                if(null == mInitialContentSettings)
                {
                    mInitialContentSettings = Barrel.Current.Get<ContentfulInitialContentSettings>("initialContentSettings");
                }
                return mInitialContentSettings;
            }
        }

        public static void SetInitialContentSettings(IEnumerable<ContentType> contentTypes, int expirationHours)
        {
            mInitialContentSettings = new ContentfulInitialContentSettings();
            foreach(ContentType contentType in contentTypes)
            {
                mInitialContentSettings.ContentTypeIds.Add(contentType.SystemProperties.Id);
            }
            mInitialContentSettings.ExpirationHours = expirationHours;
            Barrel.Current.Add<ContentfulInitialContentSettings>("initialContentSettings", mInitialContentSettings, TimeSpan.MaxValue);
        }

        public static void Reset()
        {
            SyncEntries = new SynchronizedEntries();
        }

    }
}
