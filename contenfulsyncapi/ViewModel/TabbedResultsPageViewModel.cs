using System;
namespace contenfulsyncapi.ViewModel
{
    public class TabbedResultsPageViewModel : BaseViewModel
    {
        private string mPageTitle;
        public string PageTitle
        {
            get { return mPageTitle; }
            protected set
            {
                base.SetProperty<string>(ref mPageTitle, value, "PageTitle", null);
            }
        }

        public TabbedResultsPageViewModel()
        {
            PageTitle = "Synced Data";
        }
    }
}
