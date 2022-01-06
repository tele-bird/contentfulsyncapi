using System;
using System.Collections.Generic;
using contenfulsyncapi.ViewModel;
using Contentful.Core.Models;
using Xamarin.Forms;

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

        async void btn_Next_Clicked(System.Object sender, System.EventArgs e)
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
                    SpaceId = ViewModel.SpaceId,
                };
                await Navigation.PushAsync(new SelectFiltersPage(client, contentTypes, App.InitialContentSettings));
            }
            catch (Exception exc)
            {
                await DisplayAlert(exc.GetType().Name, exc.Message, "OK");
            }
        }

        void btn_SaveSettings_Clicked(System.Object sender, System.EventArgs e)
        {
        }
    }
}
