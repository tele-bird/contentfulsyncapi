﻿using System.Collections.ObjectModel;
using contenfulsyncapi.Dto.DB;
using Contentful.Core.Models;

namespace contenfulsyncapi.ViewModel
{
    public class ResultsListViewModel : SyncedDataViewModel
    {
        public ObservableCollection<EntryViewModel> Entries { get; set; }

        public ResultsListViewModel(SynchronizedEntries synchronizedData)
        {
            Entries = new ObservableCollection<EntryViewModel>();
            Refresh(synchronizedData);
        }

        public void Refresh(SynchronizedEntries synchronizedEntries)
        {
            int updatedCount = 0;
            int i;
            EntryViewModel itemToBeReplaced;
            foreach (Entry<dynamic> entry in synchronizedEntries.Entries.Values)
            {
                EntryViewModel newEntryViewModel = new EntryViewModel(entry);
                if(Entries.Contains(newEntryViewModel))
                {
                    i = Entries.IndexOf(newEntryViewModel);
                    itemToBeReplaced = Entries[i];
                    newEntryViewModel.Updated = newEntryViewModel.Revision > itemToBeReplaced.Revision;
                    Entries.Remove(itemToBeReplaced);
                }
                else
                {
                    newEntryViewModel.Updated = true;
                }
                Entries.Add(newEntryViewModel);
                if (newEntryViewModel.Updated) ++updatedCount;
            }
            PageTitle = $"Results: {synchronizedEntries.Entries.Count} entries ({updatedCount} updated)";
            IsLoadNextPageButtonEnabled = synchronizedEntries.HasNextPageUrl;
            IsUpdateResultsButtonEnabled = synchronizedEntries.HasNextSyncUrl;
            if(synchronizedEntries.PreviousUpdatedUtc.HasValue)
            {
                PreviousUpdated = synchronizedEntries.PreviousUpdatedUtc.Value.ToLocalTime();
            }
            else
            {
                PreviousUpdated = null;
            }
            if(synchronizedEntries.LastUpdatedUtc.HasValue)
            {
                LastUpdated = synchronizedEntries.LastUpdatedUtc.Value.ToLocalTime();
            }
            else
            {
                LastUpdated = null;
            }
        }

        public void ToggleButtonsEnabled()
        {
            this.IsLoadNextPageButtonEnabled = !this.IsLoadNextPageButtonEnabled;
            this.IsUpdateResultsButtonEnabled = !this.IsUpdateResultsButtonEnabled;
        }

        internal void Reset()
        {
            App.Reset();
        }
    }
}