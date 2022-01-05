using System;
using Contentful.Core.Models;
using Xamarin.Forms;

namespace contenfulsyncapi.ViewModel
{
    public class ContentTypeViewModel : BaseViewModel
    {
        private const string ALL_NAME = "All";

        public string Name { get; set; }

        private bool mIsSelected;
        public bool IsSelected
        {
            get { return mIsSelected; }
            set
            {
                base.SetProperty<bool>(ref mIsSelected, value, "IsSelected");
            }
        }

        public ContentType ContentType { get; set; }

        public ContentTypeViewModel(ContentType contentType)
        {
            Name = contentType.Name;
            ContentType = contentType;
        }

        private ContentTypeViewModel(string name)
        {
            Name = name;
        }

        public static ContentTypeViewModel ConstructInstanceAll()
        {
            return new ContentTypeViewModel(ALL_NAME);
        }

        public bool IsAll()
        {
            return Name.Equals(ALL_NAME);
        }

        public override bool Equals(object obj)
        {
            return (null != obj)
                && obj.GetType().Equals(this.GetType())
                && ((ContentTypeViewModel)obj).Name.Equals(this.Name);
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
