using System;
using System.Collections.Generic;
using contenfulsyncapi.ViewModel;
using Contentful.Core.Models;
using Xamarin.Forms;

namespace contenfulsyncapi
{
    public partial class SelectFiltersPage : ContentPage
    {
        private SelectFiltersPageViewModel ViewModel => (SelectFiltersPageViewModel)BindingContext;

        private ContentfulClient mClient = null;

        public SelectFiltersPage(ContentfulClient client, IEnumerable<ContentType> contentTypes, ContentfulInitialContentSettings contentfulInitialContentSettings = null)
        {
            mClient = client;
            BindingContext = new SelectFiltersPageViewModel(contentTypes, contentfulInitialContentSettings);
            InitializeComponent();
        }

        async void btn_RequestContent_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                List<ContentType> contentTypes = ViewModel.GetSelectedContentTypes();
                if (0 == contentTypes.Count)
                {
                    throw new Exception("Select at least one content type for the initial request.");
                }
                SyncResult syncResult = await mClient.RequestInitialSyncNET(SyncType.All, null, true);
                App.SetInitialContentSettings(contentTypes, ViewModel.ExpirationHours);
                App.Reset();
                App.SyncEntries.Update(syncResult);
                await Navigation.PushAsync(new ResultsListPage(mClient, App.SyncEntries));
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
