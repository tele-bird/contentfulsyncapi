using System;
using System.Collections.Generic;
using contenfulsyncapi.Model;
using contenfulsyncapi.Service;
using contenfulsyncapi.ViewModel;
using Contentful.Core.Models;
using Xamarin.Forms;

namespace contenfulsyncapi
{
    public partial class MainPage : ContentPage
    {
        private MainPageViewModel ViewModel => (MainPageViewModel)BindingContext;

        private CachingContentService mCachingContentService;

        public MainPage()
        {
            BindingContext = new MainPageViewModel();
            mCachingContentService = new CachingContentService();
            ViewModel.SpaceId = mCachingContentService.AppSettings?.SpaceId;
            ViewModel.AccessToken = mCachingContentService.AppSettings?.AccessToken;
            ViewModel.Environment = mCachingContentService.AppSettings?.Environment;
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
                ContentfulClient contentfulClient = new ContentfulClient(ViewModel.SpaceId, ViewModel.AccessToken, ViewModel.Environment);
                IEnumerable<ContentType> contentTypes = await contentfulClient.RequestContentTypesNET();
                mCachingContentService.AppSettings = new ContentfulAppSettings
                {
                    AccessToken = ViewModel.AccessToken,
                    Environment = ViewModel.Environment,
                    SpaceId = ViewModel.SpaceId
                };
                await Navigation.PushAsync(new SelectFiltersPage(mCachingContentService, contentTypes));
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
