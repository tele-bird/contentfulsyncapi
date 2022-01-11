using System;
using System.Collections.Generic;

namespace contenfulsyncapi.Model
{
    public class EntryCollectionShell
    {
        public Nullable<DateTime> LastUpdatedUtc { get; set; }

        public Nullable<DateTime> ExpiresUtc { get; set; }

        public IEnumerable<EntryShell> EntryShells { get; set; }

        public string DeltaUrl { get; set; }

        public int SyncVersion { get; set; }

        public EntryCollectionShell()
        {
            EntryShells = new List<EntryShell>();
        }

        public void UpdateWithDelta(EntryCollectionShell delta)
        {
            LastUpdatedUtc = delta.LastUpdatedUtc;
            ExpiresUtc = delta.ExpiresUtc;
            DeltaUrl = delta.DeltaUrl;
            ++SyncVersion;
            SortedSet<EntryShell> originalEntryShellsSet = new SortedSet<EntryShell>(this.EntryShells);
            originalEntryShellsSet.ExceptWith(delta.EntryShells);
            foreach (EntryShell deltaEntryShell in delta.EntryShells)
            {
                originalEntryShellsSet.Add(deltaEntryShell);
            }
            EntryShells = new List<EntryShell>(originalEntryShellsSet);
        }
    }
}
