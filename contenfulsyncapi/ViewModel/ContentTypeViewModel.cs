using System;
using Contentful.Core.Models;

namespace contenfulsyncapi.ViewModel
{
    public class ContentTypeViewModel
    {
        public static ContentTypeViewModel ALL_CONTENT_TYPES = new ContentTypeViewModel("All");

        public string Name { get; set; }

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

        public override bool Equals(object obj)
        {
            return obj.GetType().Equals(this.GetType()) && ((ContentTypeViewModel)obj).Name.Equals(this.Name);
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
