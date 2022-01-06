using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Contentful.Core.Models;

namespace contenfulsyncapi.ViewModel
{
    public class SelectFiltersPageViewModel : BaseViewModel
    {
        private string mPageTitle;
        public string PageTitle
        {
            get { return mPageTitle; }
            set
            {
                base.SetProperty<string>(ref mPageTitle, value, "PageTitle");
            }
        }

        private SyncType mSelectedSyncTypeIndex;
        public SyncType SelectedSyncTypeIndex
        {
            get { return mSelectedSyncTypeIndex; }
            set
            {
                base.SetProperty<SyncType>(ref mSelectedSyncTypeIndex, value, "SelectedSyncTypeIndex");
            }
        }

        public ObservableCollection<ContentTypeViewModel> ContentTypeViewModels { get; set; }

        private int mExpirationHours;
        public int ExpirationHours
        {
            get { return mExpirationHours; }
            set
            {
                base.SetProperty<int>(ref mExpirationHours, value, "ExpirationHours");
            }
        }

        public SelectFiltersPageViewModel(IEnumerable<ContentType> allContentTypes, ContentfulInitialContentSettings contentfulInitialContentSettings)
        {
            SelectedSyncTypeIndex = 0;
            ContentTypeViewModels = new ObservableCollection<ContentTypeViewModel>();
            bool allSelected = true;
            foreach (ContentType contentType in allContentTypes)
            {
                ContentTypeViewModel contentTypeViewModel = new ContentTypeViewModel(contentType);
                if(null != contentfulInitialContentSettings)
                {
                    contentTypeViewModel.IsSelected = contentfulInitialContentSettings.ContentTypeIds.Contains(contentTypeViewModel.ContentType.SystemProperties.Id);
                }
                contentTypeViewModel.PropertyChanged += ContentTypeViewModel_PropertyChanged;
                allSelected = allSelected && contentTypeViewModel.IsSelected;
                ContentTypeViewModels.Add(contentTypeViewModel);
            }
            ContentTypeViewModel contentTypeViewModelAll = ContentTypeViewModel.ConstructInstanceAll();
            contentTypeViewModelAll.IsSelected = allSelected;
            contentTypeViewModelAll.PropertyChanged += ContentTypeViewModel_PropertyChanged;
            ContentTypeViewModels.Insert(0, contentTypeViewModelAll);
            this.RecalculatePageTitle();
            if (null != contentfulInitialContentSettings)
            {
                ExpirationHours = contentfulInitialContentSettings.ExpirationHours;
            }
        }

        private void ContentTypeViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName.Equals("IsSelected"))
            {
                ContentTypeViewModel contentTypeViewModel = (ContentTypeViewModel)sender;
                if (contentTypeViewModel.IsAll())
                {
                    foreach (var contentType in ContentTypeViewModels)
                    {
                        contentType.IsSelected = contentTypeViewModel.IsSelected;
                    }
                }
                else
                {
                    this.RecalculatePageTitle();
                }
            }
        }

        private void RecalculatePageTitle()
        {
            int num = 0;
            foreach (ContentTypeViewModel contentTypeViewModel in ContentTypeViewModels)
            {
                if (contentTypeViewModel.IsSelected)
                {
                    ++num;
                }
            }
            PageTitle = $"Content Types for sync ({num})";
        }

        public List<ContentType> GetSelectedContentTypes()
        {
            List<ContentType> contentTypes = new List<ContentType>(ContentTypeViewModels.Count - 1);
            foreach(ContentTypeViewModel contentTypeViewModel in ContentTypeViewModels)
            {
                if(!contentTypeViewModel.IsAll() && contentTypeViewModel.IsSelected)
                {
                    contentTypes.Add(contentTypeViewModel.ContentType);
                }
            }
            return contentTypes;
        }
    }
}
