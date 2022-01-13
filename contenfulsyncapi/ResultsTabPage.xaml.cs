using contenfulsyncapi.Dto.DB;
using contenfulsyncapi.Service;
using contenfulsyncapi.ViewModel;
using Contentful.Core.Models;
using Xamarin.Forms;

namespace contenfulsyncapi
{
    public partial class ResultsTabPage : ContentPage
    {
        private ResultsTabViewModel ViewModel => (ResultsTabViewModel)BindingContext;

        public ResultsTabPage(ContentTypeDto contentTypeDto, CachingContentService cachingContentService)
        {
            InitializeComponent();
            BindingContext = new ResultsTabViewModel(contentTypeDto, cachingContentService);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.ExecuteRefreshCommand(this);
        }
    }
}
