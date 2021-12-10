using System;
using System.Collections.ObjectModel;
using contenfulsyncapi.Dto.DB;
using Contentful.Core.Models;

namespace contenfulsyncapi.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {
        public ObservableCollection<ContentTypeViewModel> ContentTypes { get; set; }

        private string mSpace;
        public string Space
        {
            get { return mSpace; }
            set
            {
                base.SetProperty<string>(ref mSpace, value, "Space");
            }
        }

        private string mAccessToken;
        public string AccessToken
        {
            get { return mAccessToken; }
            set
            {
                base.SetProperty<string>(ref mAccessToken, value, "AccessToken");
            }
        }

        private string mEnvironment;
        public string Environment
        {
            get { return mEnvironment; }
            set
            {
                base.SetProperty<string>(ref mEnvironment, value, "Environment");
            }
        }

        private bool mIsSpaceSelected;
        public bool IsSpaceSelected
        {
            get { return mIsSpaceSelected; }
            private set
            {
                base.SetProperty<bool>(ref mIsSpaceSelected, value, "IsSpaceSelected");
                RequestButtonText = IsSpaceSelected ? "Request Content" : "Request Content Types";
            }
        }

        private string mRequestButtonText;
        public string RequestButtonText
        {
            get { return mRequestButtonText; }
            set
            {
                base.SetProperty<string>(ref mRequestButtonText, value, "RequestButtonText");
            }
        }

        private bool mRecursive;
        public bool Recursive
        {
            get { return mRecursive; }
            set
            {
                base.SetProperty<bool>(ref mRecursive, value, "Recursive");
            }
        }

        public MainPageViewModel()
        {
            ContentTypes = new ObservableCollection<ContentTypeViewModel>();
            IsSpaceSelected = false;
        }

        public MainPageViewModel(SynchronizedContentTypes synchronizedContentTypes)
            : this()
        {
            Refresh(synchronizedContentTypes);
        }

        public void Refresh(SynchronizedContentTypes synchronizedContentTypes)
        {
            ContentTypes.Clear();
            foreach (ContentType contentType in synchronizedContentTypes.ContentTypesByName.Values)
            {
                ContentTypes.Add(new ContentTypeViewModel(contentType));
            }
            ContentTypes.Insert(0, ContentTypeViewModel.ALL_CONTENT_TYPES);
            IsSpaceSelected = true;
        }
    }
}
