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
using contenfulsyncapi.Model;
using MonkeyCache.FileStore;

namespace contenfulsyncapi
{
    public partial class MainPage : ContentPage
    {
        private MainPageViewModel ViewModel => (MainPageViewModel)BindingContext;

        public MainPage()
        {
            BindingContext = new MainPageViewModel();
            var appSettings = App.AppSettings;
            ViewModel.SpaceId = appSettings?.SpaceId;
            ViewModel.AccessToken = appSettings?.AccessToken;
            ViewModel.Environment = appSettings?.Environment;
            InitializeComponent();
        }

        async void btn_InitializeClient_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(ViewModel.SpaceId)  || string.IsNullOrWhiteSpace(ViewModel.AccessToken) || string.IsNullOrWhiteSpace(ViewModel.Environment))
                {
                    throw new Exception("A required parameter is missing.");
                }
                ContentfulClient client = new ContentfulClient(ViewModel.SpaceId, ViewModel.AccessToken, ViewModel.Environment);
                IEnumerable<ContentType> contentTypes = await client.RequestContentTypesNET();
                App.AppSettings = new ContentfulAppSettings
                {
                    AccessToken = ViewModel.AccessToken,
                    Environment = ViewModel.Environment,
                    SpaceId = ViewModel.SpaceId
                };
                ContentPage nextPage = null;
                if (ViewModel.ContentfulApi.Equals(ContentfulApi.SyncAPI))
                {
                    nextPage = new SelectFiltersPage(client, contentTypes);
                }
                else
                {
                    throw new Exception("Sorry, GraphQL isn't plugged in yet.");
                    //nextPage = new SomeOtherPage(client, contentTypes);
                }
                await Navigation.PushAsync(nextPage);
            }
            catch (Exception exc)
            {
                await DisplayAlert(exc.GetType().Name, exc.Message, "OK");
            }
        }
    }
}
