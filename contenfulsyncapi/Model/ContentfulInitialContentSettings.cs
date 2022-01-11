using System.Collections.Generic;

namespace contenfulsyncapi.Model
{
    public class ContentfulInitialContentSettings
    {
        public List<string> ContentTypeIds { get; set; }

        public int ExpirationMinutes { get; set; }

        public ContentfulInitialContentSettings()
        {
            ContentTypeIds = new List<string>();
        }
    }
}
