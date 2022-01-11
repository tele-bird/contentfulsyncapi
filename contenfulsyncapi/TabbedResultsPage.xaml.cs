using System.Collections.Generic;
using contenfulsyncapi.Service;
using Contentful.Core.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace contenfulsyncapi
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedResultsPage : TabbedPage
    {
        public TabbedResultsPage(CachingContentService cachingContentService, List<ContentType> contentTypes)
        {
            InitializeComponent();
            foreach(ContentType contentType in contentTypes)
            {
                this.Children.Add(new ResultsTabPage(contentType, cachingContentService));
            }
        }
    }
}
