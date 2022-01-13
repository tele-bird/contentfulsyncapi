using contenfulsyncapi.Dto.DB;

namespace contenfulsyncapi.ViewModel
{
    public class ContentTypeViewModel : BaseViewModel
    {
        private const string ALL_ID = "all";
        private const string ALL_NAME = "All";

        public string Id { get; set; }

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

        public ContentTypeViewModel(ContentTypeDto contentTypeDto)
        {
            Id = contentTypeDto.Id;
            Name = contentTypeDto.Name;
        }

        private ContentTypeViewModel(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public static ContentTypeViewModel ConstructInstanceAll()
        {
            return new ContentTypeViewModel(ALL_ID, ALL_NAME);
        }

        public bool IsAll()
        {
            return Id.Equals(ALL_ID);
        }

        public override bool Equals(object obj)
        {
            return (null != obj)
                && obj.GetType().Equals(this.GetType())
                && ((ContentTypeViewModel)obj).Id.Equals(this.Id);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
