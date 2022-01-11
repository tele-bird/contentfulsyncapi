using System;
namespace contenfulsyncapi.ViewModel
{
    public abstract class SyncedDataViewModel : BaseViewModel
    {
        private string mPageTitle;
        public string PageTitle
        {
            get { return mPageTitle; }
            protected set
            {
                base.SetProperty<string>(ref mPageTitle, value, "PageTitle", null);
            }
        }

        public bool Updated { get; internal set; }

        private Nullable<DateTime> mPreviousUpdated;
        public Nullable<DateTime> PreviousUpdated
        {
            get { return mPreviousUpdated; }
            protected set
            {
                base.SetProperty<Nullable<DateTime>>(ref mPreviousUpdated, value, "PreviousUpdated");
                string valueString = value.HasValue ? value.Value.ToString() : "(never)";
                PreviousUpdatedText = $"Previous update: {valueString}";
            }
        }

        private string mPreviousUpdatedText;
        public string PreviousUpdatedText
        {
            get { return mPreviousUpdatedText; }
            protected set
            {
                base.SetProperty<string>(ref mPreviousUpdatedText, value, "PreviousUpdatedText", null);
            }
        }

        private Nullable<DateTime> mLastUpdated;
        public Nullable<DateTime> LastUpdated
        {
            get { return mLastUpdated; }
            protected set
            {
                base.SetProperty<Nullable<DateTime>>(ref mLastUpdated, value, "LastUpdated");
                string valueString = value.HasValue ? value.Value.ToString() : "(never)";
                LastUpdatedText = $"Last update: {valueString}";
            }
        }

        private string mLastUpdatedText;
        public string LastUpdatedText
        {
            get { return mLastUpdatedText; }
            protected set
            {
                base.SetProperty<string>(ref mLastUpdatedText, value, "LastUpdatedText", null);
            }
        }

        private Nullable<DateTime> mExpires;
        public Nullable<DateTime> Expires
        {
            get { return mExpires; }
            protected set
            {
                base.SetProperty<Nullable<DateTime>>(ref mExpires, value, "Expires");
                string valueString = value.HasValue ? value.Value.ToString() : "(never)";
                ExpiresText = $"Expires: {valueString}";
            }
        }

        private string mExpiresText;
        public string ExpiresText
        {
            get { return mExpiresText; }
            protected set
            {
                base.SetProperty<string>(ref mExpiresText, value, "ExpiresText", null);
            }
        }

        public int SyncVersion { get; set; }

        protected SyncedDataViewModel()
        {
        }
    }
}
