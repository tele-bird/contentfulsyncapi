using System;
using Contentful.Core.Models;
using contentfulsyncapi.Dto;

namespace contenfulsyncapi.Model
{
    public class EntryShell : IComparable<EntryShell>
    {
        public string ContentTypeId { get; set; }

        public string ContentTypeName { get; set; }

        public string Id { get; set; }

        public int? Revision { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public RecipeFields Fields { get; set; }

        public bool Updated { get; set; }

        public EntryShell()
        {
        }

        public EntryShell(Entry<dynamic> entry)
        {
            ContentTypeId = entry.SystemProperties.ContentType.SystemProperties.Id;
            ContentTypeName = entry.SystemProperties.ContentType.Name;
            Id = entry.SystemProperties.Id;
            Revision = entry.SystemProperties.Revision;
            UpdatedAt = entry.SystemProperties.UpdatedAt.Value.ToLocalTime();
            Fields = RecipeFields.FromJson(entry.Fields.ToString());
        }

        public override bool Equals(object obj)
        {
            return (null != obj)
                && obj.GetType().Equals(this.GetType())
                && ((EntryShell)obj).Id.Equals(this.Id);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public int CompareTo(EntryShell other)
        {
            if (other == null) return 1;
            return this.Id.CompareTo(other.Id);
        }
    }
}
