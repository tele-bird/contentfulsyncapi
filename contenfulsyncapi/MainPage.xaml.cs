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

        public MainPage(string spaceId, string accessToken, string environment)
        {
            BindingContext = new MainPageViewModel();
            ViewModel.SpaceId = spaceId;
            ViewModel.AccessToken = accessToken;
            ViewModel.Environment = environment;
            InitializeComponent();
        }

        async void btn_InitializeService_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(ViewModel.SpaceId)  || string.IsNullOrWhiteSpace(ViewModel.AccessToken) || string.IsNullOrWhiteSpace(ViewModel.Environment))
                {
                    throw new Exception("A required parameter is missing.");
                }
                ContentfulClient client = new ContentfulClient(ViewModel.SpaceId, ViewModel.AccessToken, ViewModel.Environment);
                IEnumerable<ContentType> contentTypes = await client.RequestContentTypesNET();
                await Navigation.PushAsync(new SelectFiltersPage(client, contentTypes));
            }
            catch (Exception exc)
            {
                await DisplayAlert(exc.GetType().Name, exc.Message, "OK");
            }
        }
    }
}
