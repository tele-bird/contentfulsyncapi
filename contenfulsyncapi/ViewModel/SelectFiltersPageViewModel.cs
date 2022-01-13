using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using contenfulsyncapi.Dto.DB;
using contenfulsyncapi.Model;
using contenfulsyncapi.Service;
using Contentful.Core.Models;
using Xamarin.Forms;

namespace contenfulsyncapi.ViewModel
{
    public class SelectFiltersPageViewModel : BaseViewModel
    {
        private readonly CachingContentService mCachingContentService;
        private ContentfulInitialContentSettings mInitialContentSettings;

        private string mPageTitle;
        public string PageTitle
        {
            get { return mPageTitle; }
            set
            {
                base.SetProperty<string>(ref mPageTitle, value, "PageTitle");
            }
        }

        public ObservableCollection<ContentTypeViewModel> ContentTypeViewModels { get; set; }

        private int mExpirationMinutes;
        public int ExpirationMinutes
        {
            get { return mExpirationMinutes; }
            set
            {
                base.SetProperty<int>(ref mExpirationMinutes, value, "ExpirationMinutes");
            }
        }

        public ICommand RefreshCommand { get; }

        private bool mIsRefreshing;
        public bool IsRefreshing
        {
            get { return mIsRefreshing; }
            set
            {
                mIsRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        internal void SelectInitialContentSettings()
        {
            List<string> contentTypeIds = this.GetSelectedContentTypeIds();
            if (0 == contentTypeIds.Count)
            {
                throw new Exception("Select at least one content type for the initial request.");
            }
            mInitialContentSettings.SelectedContentTypeIds.AddRange(contentTypeIds);
            mCachingContentService.InitialContentSettings = mInitialContentSettings;
        }

        public SelectFiltersPageViewModel(CachingContentService cachingContentService)
        {
            mCachingContentService = cachingContentService;
            mInitialContentSettings = mCachingContentService.InitialContentSettings;
            ContentTypeViewModels = new ObservableCollection<ContentTypeViewModel>();
            RefreshCommand = new Command(ExecuteRefreshCommand);
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
            PageTitle = $"Content Types to sync ({num})";
        }

        private List<string> GetSelectedContentTypeIds()
        {
            List<string> contentTypeIds = new List<string>();
            foreach(ContentTypeViewModel contentTypeViewModel in ContentTypeViewModels)
            {
                if(!contentTypeViewModel.IsAll() && contentTypeViewModel.IsSelected)
                {
                    contentTypeIds.Add(contentTypeViewModel.Id);
                }
            }
            return contentTypeIds;
        }

        public async void ExecuteRefreshCommand(object obj)
        {
            if (IsRefreshing) return;
            IsRefreshing = true;
            if (null == mInitialContentSettings)
            {
                mInitialContentSettings = new ContentfulInitialContentSettings();
            }

            if(0 == mInitialContentSettings.AllContentTypeDtos.Count)
            {
                IEnumerable<ContentType> contentTypes = await mCachingContentService.GetContentTypesAsync();
                foreach(ContentType contentType in contentTypes)
                {
                    mInitialContentSettings.AllContentTypeDtos.Add(new ContentTypeDto(contentType));
                }
            }

            SetState(mInitialContentSettings);
            IsRefreshing = false;
        }

        private void SetState(ContentfulInitialContentSettings contentfulInitialContentSettings)
        {
            ExpirationMinutes = contentfulInitialContentSettings.ExpirationMinutes;
            ContentTypeViewModels.Clear();
            bool allSelected = true;
            foreach (ContentTypeDto contentTypeDto in contentfulInitialContentSettings.AllContentTypeDtos)
            {
                ContentTypeViewModel contentTypeViewModel = new ContentTypeViewModel(contentTypeDto);
                contentTypeViewModel.IsSelected = contentfulInitialContentSettings.SelectedContentTypeIds.Contains(contentTypeViewModel.Id);
                contentTypeViewModel.PropertyChanged += ContentTypeViewModel_PropertyChanged;
                allSelected = allSelected && contentTypeViewModel.IsSelected;
                ContentTypeViewModels.Add(contentTypeViewModel);
            }
            ContentTypeViewModel contentTypeViewModelAll = ContentTypeViewModel.ConstructInstanceAll();
            contentTypeViewModelAll.IsSelected = allSelected;
            contentTypeViewModelAll.PropertyChanged += ContentTypeViewModel_PropertyChanged;
            ContentTypeViewModels.Insert(0, contentTypeViewModelAll);
            this.RecalculatePageTitle();
        }
    }
}
