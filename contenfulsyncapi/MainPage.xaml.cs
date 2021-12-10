using System;
using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Net.Http;
//using System.Text;
//using System.Threading.Tasks;
using contenfulsyncapi.ViewModel;
using Xamarin.Forms;
using Contentful.Core.Models;

namespace contenfulsyncapi
{
    public partial class MainPage : ContentPage
    {
        private MainPageViewModel ViewModel => (MainPageViewModel)BindingContext;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel();
        }

        async void btn_Request_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(ViewModel.Space)  || string.IsNullOrWhiteSpace(ViewModel.AccessToken) || string.IsNullOrWhiteSpace(ViewModel.Environment))
                {
                    throw new Exception("A required parameter is missing.");
                }
                ContentfulClient client = new ContentfulClient(ViewModel.Space, ViewModel.AccessToken, ViewModel.Environment);
                if(!ViewModel.IsSpaceSelected)
                {
                    IEnumerable<ContentType> contentTypes = await client.RequestContentTypesNET();
                    App.SyncContentTypes.Update(contentTypes, true);
                    ViewModel.Refresh(App.SyncContentTypes);
                }
                else
                {
                    App.Reset();
                    SyncType syncType = SyncType.All;
                    SyncType.TryParse((string)picker_type.SelectedItem, true, out syncType);
                    ContentTypeViewModel contentTypeViewModel = (ContentTypeViewModel)picker_contentType.SelectedItem;
                    ContentType contentType = null;
                    if(!contentTypeViewModel.Equals(ContentTypeViewModel.ALL_CONTENT_TYPES))
                    {
                        contentType = App.SyncContentTypes.ContentTypesByName[contentTypeViewModel.Name];
                    }
                    SyncResult syncResult = await client.RequestInitialSyncNET(syncType, contentType, ViewModel.Recursive);
                    App.SyncEntries.Update(syncResult);
                    await Navigation.PushAsync(new ResultsListPage(client, App.SyncEntries));
                }
            }
            catch (Exception exc)
            {
                await DisplayAlert(exc.GetType().Name, exc.Message, "OK");
            }
        }
    }
}
