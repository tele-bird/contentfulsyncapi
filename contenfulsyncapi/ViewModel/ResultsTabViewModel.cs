using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using contenfulsyncapi.Model;
using contenfulsyncapi.Service;
using Contentful.Core.Models;
using Xamarin.Forms;

namespace contenfulsyncapi.ViewModel
{
    public class ResultsTabViewModel : SyncedDataViewModel
    {
        private ContentType mContentType;
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

        public ResultsTabViewModel(ContentType contentType, CachingContentService cachingContentService)
        {
            mContentType = contentType;
            mCachingContentService = cachingContentService;
            EntryShells = new ObservableCollection<EntryShell>();
            RefreshCommand = new Command(ExecuteRefreshCommand);
            RecalculatePageTitle();
        }

        private async void ExecuteRefreshCommand(object obj)
        {
            if (IsRefreshing) return;
            IsRefreshing = true;
            int i;
            EntryShell itemToBeReplaced;
            EntryCollectionShell entryCollectionShell = await mCachingContentService.GetEntriesByContentTypeAsync(mContentType.SystemProperties.Id);
            bool updated = false;
            if(entryCollectionShell.EntryShells != null)
            {
                foreach (EntryShell entryShell in entryCollectionShell.EntryShells)
                {
                    if (!entryShell.ContentTypeId.Equals(mContentType.SystemProperties.Id))
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
            PageTitle = $"{mContentType.Name} v{SyncVersion} ({EntryShells.Count})";
        }
    }
}
