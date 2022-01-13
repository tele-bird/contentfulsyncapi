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

        private CachingContentService mCachingContentService;

        private bool mAppeared;

        public SelectFiltersPage(CachingContentService cachingContentService)
        {
            mCachingContentService = cachingContentService;
            BindingContext = new SelectFiltersPageViewModel(cachingContentService);
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // do this stuff only the first time the page appears:
            if (!mAppeared)
            {
                // populate the content types in the list:
                ViewModel.ExecuteRefreshCommand(this);

                // navigate to next page, if needed:
                if (mCachingContentService.InitialContentSettings != null)
                {
                    NavigateToNextPage();
                }
                mAppeared = true;
            }
        }

        private async void NavigateToNextPage()
        {
            await Navigation.PushAsync(new TabbedResultsPage(mCachingContentService));
        }

        async void btn_Next_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                ViewModel.SelectInitialContentSettings();
                NavigateToNextPage();
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
