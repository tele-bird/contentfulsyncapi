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

        private bool mAppeared;

        public MainPage()
        {
            mCachingContentService = new CachingContentService();
            BindingContext = new MainPageViewModel(mCachingContentService.AppSettings);
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // do this stuff only the first time the page appears:
            if (!mAppeared)
            {
                // navigate to next page, if needed:
                if (mCachingContentService.AppSettings != null)
                {
                    NavigateToNextPage();
                }
                mAppeared = true;
            }
        }

        async void btn_Next_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(ViewModel.SpaceId)  || string.IsNullOrWhiteSpace(ViewModel.AccessToken) || string.IsNullOrWhiteSpace(ViewModel.Environment))
                {
                    throw new Exception("A required parameter is missing.");
                }
                mCachingContentService.AppSettings = new ContentfulAppSettings
                {
                    AccessToken = ViewModel.AccessToken,
                    Environment = ViewModel.Environment,
                    SpaceId = ViewModel.SpaceId
                };
                NavigateToNextPage();
            }
            catch (Exception exc)
            {
                await DisplayAlert(exc.GetType().Name, exc.Message, "OK");
            }
        }

        private async void NavigateToNextPage()
        {
            await Navigation.PushAsync(new SelectFiltersPage(mCachingContentService));
        }
    }
}
