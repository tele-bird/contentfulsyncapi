using System.Collections.Generic;
using contenfulsyncapi.Dto.DB;
using contenfulsyncapi.Service;
using contenfulsyncapi.ViewModel;
using Contentful.Core.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace contenfulsyncapi
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedResultsPage : TabbedPage
    {
        private TabbedResultsPageViewModel ViewModel => (TabbedResultsPageViewModel)BindingContext;

        public TabbedResultsPage(CachingContentService cachingContentService)
        {
            InitializeComponent();
            BindingContext = new TabbedResultsPageViewModel();
            foreach (ContentTypeDto contentTypeDto in cachingContentService.InitialContentSettings.AllContentTypeDtos)
            {
                if(cachingContentService.InitialContentSettings.SelectedContentTypeIds.Contains(contentTypeDto.Id))
                {
                    this.Children.Add(new ResultsTabPage(contentTypeDto, cachingContentService));
                }
            }
        }
    }
}
