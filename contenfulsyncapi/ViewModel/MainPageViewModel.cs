using System.Collections.ObjectModel;
using contenfulsyncapi.Model;

namespace contenfulsyncapi.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {
        private string mSpaceId;
        public string SpaceId
        {
            get { return mSpaceId; }
            set
            {
                base.SetProperty<string>(ref mSpaceId, value, "SpaceId");
            }
        }

        private string mAccessToken;
        public string AccessToken
        {
            get { return mAccessToken; }
            set
            {
                base.SetProperty<string>(ref mAccessToken, value, "AccessToken");
            }
        }

        private string mEnvironment;
        public string Environment
        {
            get { return mEnvironment; }
            set
            {
                base.SetProperty<string>(ref mEnvironment, value, "Environment");
            }
        }

        public MainPageViewModel(ContentfulAppSettings appSettings)
        {
            mSpaceId = appSettings?.SpaceId;
            mAccessToken = appSettings?.AccessToken;
            mEnvironment = appSettings?.Environment;
        }
    }
}
