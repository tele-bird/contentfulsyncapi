using contenfulsyncapi.Service;
using contenfulsyncapi.ViewModel;
using Contentful.Core.Models;
using Xamarin.Forms;

namespace contenfulsyncapi
{
    public partial class ResultsTabPage : ContentPage
    {
        private ResultsTabViewModel ViewModel => (ResultsTabViewModel)BindingContext;

        public ResultsTabPage(ContentType contentType, CachingContentService cachingContentService)
        {
            InitializeComponent();
            BindingContext = new ResultsTabViewModel(contentType, cachingContentService);
        }
    }
}
