using System;
using System.Collections.Generic;
using contenfulsyncapi.Dto.DB;
using contenfulsyncapi.ViewModel;
using Contentful.Core.Models;
using Xamarin.Forms;

namespace contenfulsyncapi
{
    public partial class ResultsListPage : ContentPage
    {
        private ResultsListViewModel ViewModel => (ResultsListViewModel)BindingContext;

        private ContentfulClient mClient = null;

        public ResultsListPage(ContentfulClient client, SynchronizedEntries synchronizedData)
        {
            mClient = client;
            InitializeComponent();
            BindingContext = new ResultsListViewModel(synchronizedData);
        }

        async void cv_Response_SelectionChanged(System.Object sender, Xamarin.Forms.SelectionChangedEventArgs e)
        {
            if(e.CurrentSelection.Count > 0)
            {
                await DisplayAlert("Selected", e.CurrentSelection[0].ToString(), "OK");
                cv_Response.SelectedItem = null;
            }
        }

        void btn_LoadNextPage_Clicked(System.Object sender, System.EventArgs e)
        {
            UpdateResults(App.SyncEntries.NextPageUrl);
        }

        void btn_UpdateResults_Clicked(System.Object sender, System.EventArgs e)
        {
            UpdateResults(App.SyncEntries.NextSyncUrl);
        }

        private async void UpdateResults(string updateUrl)
        {
            try
            {
                SyncResult syncResult = await mClient.RequestDeltaOrNextPageNET(updateUrl);
                App.SyncEntries.Update(syncResult);
                ViewModel.Refresh(App.SyncEntries);
            }
            catch (Exception exc)
            {
                await DisplayAlert(exc.GetType().Name, exc.Message, "OK");
            }
        }

        void btn_Toggle_Clicked(System.Object sender, System.EventArgs e)
        {
            ViewModel.ToggleButtonsEnabled();
        }
    }
}
