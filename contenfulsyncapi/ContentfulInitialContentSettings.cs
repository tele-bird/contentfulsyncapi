using System;
using System.Collections.Generic;
using Contentful.Core.Models;

namespace contenfulsyncapi
{
    public class ContentfulInitialContentSettings
    {
        public List<string> ContentTypeIds { get; set; }

        public int ExpirationHours { get; set; }

        public ContentfulInitialContentSettings()
        {
            ContentTypeIds = new List<string>();
        }
    }
}
