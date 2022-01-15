using contenfulsyncapi.Dto.DB;
using contenfulsyncapi.Service;
using contenfulsyncapi.ViewModel;
using Xamarin.Forms;

namespace contenfulsyncapi
{
    public partial class ResultsTabPage : ContentPage
    {
        private ResultsTabViewModel ViewModel => (ResultsTabViewModel)BindingContext;

        private bool mAppeared;

        public ResultsTabPage(ContentTypeDto contentTypeDto, CachingContentService cachingContentService)
        {
            InitializeComponent();
            BindingContext = new ResultsTabViewModel(contentTypeDto, cachingContentService);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // do this stuff only the first time the tab appears:
            if(!mAppeared)
            {
                // populate with data from cache:
                ViewModel.PopulateResultsFromCache();

                // ensure this block isn't fired when OnAppearing() fires when tabs are switched:
                mAppeared = true;
            }
        }
    }
}
