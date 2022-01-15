using System;
using System.Collections.ObjectModel;
using System.Timers;
using System.Windows.Input;
using contenfulsyncapi.Dto.DB;
using contenfulsyncapi.Model;
using contenfulsyncapi.Service;
using Xamarin.Forms;

namespace contenfulsyncapi.ViewModel
{
    public class ResultsTabViewModel : SyncedDataViewModel
    {
        private ContentTypeDto mContentTypeDto;
        private CachingContentService mCachingContentService;
        private Timer mDataExpirationTimer;
        private DateTime? mDataExpiresAtUtc;

        public ObservableCollection<EntryShell> EntryShells { get; set; }

        private string mResultsHeaderMessage;
        public string ResultsHeaderMessage
        {
            get { return mResultsHeaderMessage; }
            set
            {
                base.SetProperty<string>(ref mResultsHeaderMessage, value, "ResultsHeaderMessage");
            }
        }

        private bool mIsRefresherEnabled;
        public bool IsRefresherEnabled
        {
            get { return mIsRefresherEnabled; }
            set
            {
                base.SetProperty<bool>(ref mIsRefresherEnabled, value, "IsRefresherEnabled");
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

        private Color mResultsHeaderBackgroundColor;
        public Color ResultsHeaderBackgroundColor
        {
            get { return mResultsHeaderBackgroundColor; }
            set
            {
                base.SetProperty<Color>(ref mResultsHeaderBackgroundColor, value, "ResultsHeaderBackgroundColor");
            }
        }

        private Color mResultsHeaderTextColor;
        public Color ResultsHeaderTextColor
        {
            get { return mResultsHeaderTextColor; }
            set
            {
                base.SetProperty<Color>(ref mResultsHeaderTextColor, value, "ResultsHeaderTextColor");
            }
        }

        public ResultsTabViewModel(ContentTypeDto contentTypeDto, CachingContentService cachingContentService)
        {
            mContentTypeDto = contentTypeDto;
            PageTitle = mContentTypeDto.Name;
            mCachingContentService = cachingContentService;
            mDataExpirationTimer = new Timer(1000);
            mDataExpirationTimer.Stop();
            mDataExpirationTimer.Elapsed += DataExpirationTimer_Elapsed;
            EntryShells = new ObservableCollection<EntryShell>();
            RefreshCommand = new Command(ExecuteRefreshCommand);
        }

        private void DataExpirationTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            UpdateRefresher();
        }

        private void UpdateRefresher()
        {
            DateTime utcNow = DateTime.UtcNow;
            if (mDataExpiresAtUtc.HasValue)
            {
                if (mDataExpiresAtUtc.Value < utcNow)
                {
                    mDataExpirationTimer.Stop();
                    ResultsHeaderMessage = "Data is expired.  Pull down for delta update.";
                    ResultsHeaderBackgroundColor = Color.DarkRed;
                    ResultsHeaderTextColor = Color.White;
                    IsRefresherEnabled = true;
                    //System.Diagnostics.Debug.WriteLine($"Data is expired.  {utcNow} is after {mDataExpiresAtUtc.Value}");
                }
                else
                {
                    ResultsHeaderMessage = $"Data expires in {mDataExpiresAtUtc.Value.Subtract(utcNow).ToString(@"d\.hh\:mm\:ss")}";
                    ResultsHeaderBackgroundColor = Color.Black;
                    ResultsHeaderTextColor = Color.White;
                    IsRefresherEnabled = false;
                    //System.Diagnostics.Debug.WriteLine($"Data is not expired.  {utcNow} is before {mDataExpiresAtUtc.Value}");
                }
            }
            else
            {
                ResultsHeaderMessage = "Pull down for initial sync";
                ResultsHeaderBackgroundColor = Color.DarkRed;
                ResultsHeaderTextColor = Color.White;
                IsRefresherEnabled = true;
            }
        }

        public void PopulateResultsFromCache()
        {
            if (IsRefreshing) return;
            IsRefreshing = true;
            EntryCollectionShell entryCollectionShell = mCachingContentService.GetCachedEntriesByContentType(mContentTypeDto.Id);
            UpdateResults(entryCollectionShell, true);
            mDataExpirationTimer.Start();
            IsRefreshing = false;
        }

        private async void ExecuteRefreshCommand(object obj)
        {
            if (IsRefreshing) return;
            IsRefreshing = true;
            mDataExpirationTimer.Stop();
            EntryCollectionShell entryCollectionShell = await mCachingContentService.GetEntriesUpdateByContentTypeAsync(mContentTypeDto.Id);
            UpdateResults(entryCollectionShell, false);
            mDataExpirationTimer.Start();
            IsRefreshing = false;
        }

        private void UpdateResults(EntryCollectionShell entryCollectionShell, bool fromCache)
        {
            int i;
            EntryShell itemToBeReplaced;
            bool updated = false;
            if (entryCollectionShell != null && entryCollectionShell.EntryShells != null)
            {
                mDataExpiresAtUtc = entryCollectionShell.ExpiresUtc;
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
                        entryShell.Updated = !fromCache;
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
                SyncDescription = $"Version {SyncVersion} ({EntryShells.Count} entries)";
            }
            else
            {
                foreach (EntryShell entryShell in EntryShells)
                {
                    updated = false;
                }
            }
        }
    }
}
