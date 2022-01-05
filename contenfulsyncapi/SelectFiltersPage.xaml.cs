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

        public SelectFiltersPage(ContentfulClient client, IEnumerable<ContentType> contentTypes)
        {
            mClient = client;
            BindingContext = new SelectFiltersPageViewModel(contentTypes);
            InitializeComponent();
        }

        async void btn_RequestContent_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                App.Reset();
                SyncResult syncResult = await mClient.RequestInitialSyncNET(SyncType.All, null, true);
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
