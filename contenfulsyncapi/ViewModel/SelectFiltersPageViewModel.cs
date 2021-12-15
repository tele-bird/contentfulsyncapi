using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using contenfulsyncapi.Dto.DB;
using Contentful.Core.Models;

namespace contenfulsyncapi.ViewModel
{
    public class SelectFiltersPageViewModel : BaseViewModel
    {
        private SyncType mSelectedSyncTypeIndex;
        public SyncType SelectedSyncTypeIndex
        {
            get { return mSelectedSyncTypeIndex; }
            set
            {
                base.SetProperty<SyncType>(ref mSelectedSyncTypeIndex, value, "SelectedSyncTypeIndex");
            }
        }

        public bool Recursive { get; set; }

        public ObservableCollection<ContentTypeViewModel> ContentTypes { get; set; }

        public ContentTypeViewModel SelectedContentType { get; set; }

        public SelectFiltersPageViewModel(IEnumerable<ContentType> contentTypes)
        {
            SelectedSyncTypeIndex = 0;
            ContentTypes = new ObservableCollection<ContentTypeViewModel>();
            foreach (ContentType contentType in contentTypes)
            {
                ContentTypes.Add(new ContentTypeViewModel(contentType));
            }
            ContentTypes.Insert(0, ContentTypeViewModel.ALL_CONTENT_TYPES);
            SelectedContentType = ContentTypeViewModel.ALL_CONTENT_TYPES;
        }
    }
}
