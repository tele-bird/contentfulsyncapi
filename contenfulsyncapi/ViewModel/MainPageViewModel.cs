using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using contenfulsyncapi.Dto.DB;
using contenfulsyncapi.Model;
using contenfulsyncapi.Util;
using Contentful.Core.Models;

namespace contenfulsyncapi.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {
        public ObservableCollection<ContentTypeViewModel> ContentTypes { get; set; }

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

        public MainPageViewModel()
        {
            ContentTypes = new ObservableCollection<ContentTypeViewModel>();
        }
    }
}
