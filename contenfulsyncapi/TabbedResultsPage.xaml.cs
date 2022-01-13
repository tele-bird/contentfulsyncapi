using System.Collections.Generic;
using contenfulsyncapi.Dto.DB;
using contenfulsyncapi.Service;
using Contentful.Core.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace contenfulsyncapi
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedResultsPage : TabbedPage
    {
        public TabbedResultsPage(CachingContentService cachingContentService)
        {
            InitializeComponent();
            foreach(ContentTypeDto contentTypeDto in cachingContentService.InitialContentSettings.AllContentTypeDtos)
            {
                if(cachingContentService.InitialContentSettings.SelectedContentTypeIds.Contains(contentTypeDto.Id))
                {
                    this.Children.Add(new ResultsTabPage(contentTypeDto, cachingContentService));
                }
            }
        }
    }
}
