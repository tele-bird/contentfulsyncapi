using System;
using System.Collections.Generic;
using Contentful.Core.Models;

namespace contenfulsyncapi.Dto.DB
{
    public class SynchronizedEntries : BaseSynchronizedData
    {
        public Dictionary<string, Entry<dynamic>> Entries = new Dictionary<string, Entry<dynamic>>();

        public SynchronizedEntries()
            : base()
        {
            Entries = new Dictionary<string, Entry<dynamic>>();
        }

        public override void Update(SyncResult syncResult)
        {
            base.Update(syncResult);
            foreach(var entry in syncResult.Entries)
            {
                // add or update the Entry
                if(Entries.ContainsKey(entry.SystemProperties.Id))
                {
                    Entries.Remove(entry.SystemProperties.Id);
                }
                Entries.Add(entry.SystemProperties.Id, entry);
            }
        }
    }
}
