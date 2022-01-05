using System;
using System.Collections.Generic;
using Contentful.Core.Models;
using contentfulsyncapi.Dto;
using Newtonsoft.Json.Linq;

namespace contenfulsyncapi.ViewModel
{
    public class ResultViewModel
    {
        public string Name { get; private set; }

        public bool Updated { get; internal set; }

        public SyncType SyncType { get; private set; }

        public string ContentTypeId { get; private set; }

        public string Id { get; private set; }

        public int? Revision { get; private set; }

        public DateTime? UpdatedAt { get; private set; }

        public RecipeFields Fields { get; private set; }

        public ResultViewModel(Entry<dynamic> entry)
        {
            SyncType = (SyncType)Enum.Parse(typeof(SyncType), entry.SystemProperties.Type);
            ContentTypeId = entry.SystemProperties.ContentType.SystemProperties.Id;
            Id = entry.SystemProperties.Id;
            Revision = entry.SystemProperties.Revision;
            UpdatedAt = entry.SystemProperties.UpdatedAt.Value.ToLocalTime();
            Fields = RecipeFields.FromJson(entry.Fields.ToString());

            //Fields = new Dictionary<string, string>();
            //foreach(JProperty property in entry.Fields)
            //{
            //    JToken token = null;
            //    do
            //    {
            //        token = property.Value;
            //    }
            //    while (token.HasValues);
            //    Fields.Add(property.Name, token.Value);
            //}
            //InternalName = entry.Fields.InternalName;
        }

        public override bool Equals(object obj)
        {
            return (null != obj)
                && obj.GetType().Equals(this.GetType())
                && ((ResultViewModel)obj).Id.Equals(this.Id);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
