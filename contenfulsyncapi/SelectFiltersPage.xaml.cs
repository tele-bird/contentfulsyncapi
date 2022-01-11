using System;
using System.Collections.Generic;
using contenfulsyncapi.Model;
using contenfulsyncapi.Service;
using contenfulsyncapi.ViewModel;
using Contentful.Core.Models;
using Xamarin.Forms;

namespace contenfulsyncapi
{
    public partial class SelectFiltersPage : ContentPage
    {
        private SelectFiltersPageViewModel ViewModel => (SelectFiltersPageViewModel)BindingContext;

        private CachingContentService mCachingContentService = null;

        public SelectFiltersPage(CachingContentService cachingContentService, IEnumerable<ContentType> contentTypes)
        {
            mCachingContentService = cachingContentService;
            BindingContext = new SelectFiltersPageViewModel(contentTypes, cachingContentService.InitialContentSettings);
            InitializeComponent();
        }

        async void btn_Next_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                List<ContentType> contentTypes = ViewModel.GetSelectedContentTypes();
                if (0 == contentTypes.Count)
                {
                    throw new Exception("Select at least one content type for the initial request.");
                }
                mCachingContentService.SetInitialContentSettings(contentTypes, ViewModel.ExpirationMinutes);
                await Navigation.PushAsync(new TabbedResultsPage(mCachingContentService, contentTypes));

                //SyncResult syncResult = await mClient.RequestInitialSyncNET(SyncType.All, null, true);
                //App.SetInitialContentSettings(contentTypes, ViewModel.ExpirationHours);
                //App.Reset();
                //App.SyncEntries.Update(syncResult);
                //await Navigation.PushAsync(new ResultsListPage(mClient, App.SyncEntries));
            }
            catch (Exception exc)
            {
                await DisplayAlert(exc.GetType().Name, exc.Message, "OK");
            }
        }

        void lv_ContentTypes_ItemTapped(System.Object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            ContentTypeViewModel selectedItem = (ContentTypeViewModel)e.Item;
            selectedItem.IsSelected = !selectedItem.IsSelected;
        }
    }
}
