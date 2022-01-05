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

        public ObservableCollection<ContentTypeViewModel> ContentTypes { get; set; }

        public SelectFiltersPageViewModel(IEnumerable<ContentType> contentTypes)
        {
            SelectedSyncTypeIndex = 0;
            ContentTypes = new ObservableCollection<ContentTypeViewModel>();
            foreach (ContentType contentType in contentTypes)
            {
                ContentTypeViewModel contentTypeViewModel = new ContentTypeViewModel(contentType);
                contentTypeViewModel.PropertyChanged += ContentTypeViewModel_PropertyChanged;
                ContentTypes.Add(contentTypeViewModel);
            }
            ContentTypeViewModel contentTypeViewModelAll = ContentTypeViewModel.ConstructInstanceAll();
            contentTypeViewModelAll.PropertyChanged += ContentTypeViewModel_PropertyChanged;
            ContentTypes.Insert(0, contentTypeViewModelAll);
        }

        private void ContentTypeViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName.Equals("IsSelected"))
            {
                ContentTypeViewModel contentTypeViewModel = (ContentTypeViewModel)sender;
                if (contentTypeViewModel.IsAll())
                {
                    foreach (var contentType in ContentTypes)
                    {
                        contentType.IsSelected = contentTypeViewModel.IsSelected;
                    }
                }
            }
        }
    }
}
