using System;
using System.Collections.Generic;
using Contentful.Core.Models;

namespace contenfulsyncapi.Dto.DB
{
    public class SynchronizedContentTypes
    {
        public Dictionary<string, ContentType> ContentTypesByName = new Dictionary<string, ContentType>();

        public SynchronizedContentTypes()
        {
            ContentTypesByName = new Dictionary<string, ContentType>();
        }

        public void Update(IEnumerable<ContentType> contentTypes, bool clear)
        {
            if (clear) ContentTypesByName.Clear();
            foreach (var contentType in contentTypes)
            {
                if (ContentTypesByName.ContainsKey(contentType.Name))
                {
                    ContentTypesByName.Remove(contentType.Name);
                }
                ContentTypesByName.Add(contentType.Name, contentType);
            }
        }
    }
}
