using System;
using Contentful.Core.Models;

namespace contenfulsyncapi.Dto.DB
{
    public abstract class BaseSynchronizedData
    {
        public Nullable<DateTime> PreviousUpdatedUtc { get; private set; }

        public Nullable<DateTime> PreviousUpdated
        {
            get
            {
                Nullable<DateTime> previousUpdated = null;
                if (PreviousUpdatedUtc.HasValue)
                {
                    previousUpdated = PreviousUpdatedUtc.Value.ToLocalTime();
                }
                return previousUpdated;
            }
        }

        private Nullable<DateTime> mLastUpdatedUtc;
        public Nullable<DateTime> LastUpdatedUtc
        {
            get { return mLastUpdatedUtc; }
            private set
            {
                PreviousUpdatedUtc = LastUpdatedUtc;
                mLastUpdatedUtc = value;
            }
        }

        public string NextPageUrl { get; private set; }

        public string NextSyncUrl { get; private set; }

        public bool HasNextPageUrl => NextPageUrl != null;

        public bool HasNextSyncUrl => NextSyncUrl != null;

        protected BaseSynchronizedData()
        {
        }

        public virtual void Update(SyncResult syncResult)
        {
            NextPageUrl = syncResult.NextPageUrl;
            NextSyncUrl = syncResult.NextSyncUrl;
            LastUpdatedUtc = DateTime.UtcNow;
        }
    }
}