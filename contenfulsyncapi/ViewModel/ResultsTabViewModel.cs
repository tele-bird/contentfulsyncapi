using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using contenfulsyncapi.Dto.DB;
using contenfulsyncapi.Model;
using contenfulsyncapi.Service;
using Contentful.Core.Models;
using Xamarin.Forms;

namespace contenfulsyncapi.ViewModel
{
    public class ResultsTabViewModel : SyncedDataViewModel
    {
        private ContentTypeDto mContentTypeDto;
        private CachingContentService mCachingContentService;

        public ObservableCollection<EntryShell> EntryShells { get; set; }

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

        public ResultsTabViewModel(ContentTypeDto contentTypeDto, CachingContentService cachingContentService)
        {
            mContentTypeDto = contentTypeDto;
            mCachingContentService = cachingContentService;
            EntryShells = new ObservableCollection<EntryShell>();
            RefreshCommand = new Command(ExecuteRefreshCommand);
            RecalculatePageTitle();
        }

        public async void ExecuteRefreshCommand(object obj)
        {
            if (IsRefreshing) return;
            IsRefreshing = true;
            int i;
            EntryShell itemToBeReplaced;
            EntryCollectionShell entryCollectionShell = await mCachingContentService.GetEntriesByContentTypeAsync(mContentTypeDto.Id);
            bool updated = false;
            if(entryCollectionShell.EntryShells != null)
            {
                foreach (EntryShell entryShell in entryCollectionShell.EntryShells)
                {
                    if (!entryShell.ContentTypeId.Equals(mContentTypeDto.Id))
                    {
                        throw new Exception("Unexpected scenario: content type ids don't match.");
                    }
                    if (EntryShells.Contains(entryShell))
                    {
                        i = EntryShells.IndexOf(entryShell);
                        itemToBeReplaced = EntryShells[i];
                        entryShell.Updated = entryShell.Revision > itemToBeReplaced.Revision;
                        EntryShells.RemoveAt(i);
                    }
                    else
                    {
                        entryShell.Updated = true;
                    }
                    EntryShells.Add(entryShell);
                    updated = true;
                }
            }
            if (updated)
            {
                PreviousUpdated = LastUpdated;
                LastUpdated = entryCollectionShell.LastUpdatedUtc.Value.ToLocalTime();
                Expires = entryCollectionShell.ExpiresUtc.Value.ToLocalTime();
                SyncVersion = entryCollectionShell.SyncVersion;
                RecalculatePageTitle();
            }
            else
            {
                foreach(EntryShell entryShell in EntryShells)
                {
                    updated = false;
                }
            }
            IsRefreshing = false;
        }

        private void RecalculatePageTitle()
        {
            PageTitle = $"{mContentTypeDto.Name} v{SyncVersion} ({EntryShells.Count})";
        }
    }
}
