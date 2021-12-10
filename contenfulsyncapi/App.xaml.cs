using System;
using System.Collections.Generic;
using contenfulsyncapi.Dto.DB;
using Contentful.Core.Models;
using Xamarin.Forms;

namespace contenfulsyncapi
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

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
        public static SynchronizedContentTypes SyncContentTypes { get; private set; } = new SynchronizedContentTypes();

        //public static Dictionary<string, SyncedAsset> SyncedAssetsById = new Dictionary<string, SyncedAsset>();

        public static void Reset()
        {
            SyncEntries = new SynchronizedEntries();
        }

    }
}
