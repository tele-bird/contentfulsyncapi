using System.Collections.Generic;
using contenfulsyncapi.Dto.DB;
using Contentful.Core.Models;

namespace contenfulsyncapi.Model
{
    public class ContentfulInitialContentSettings
    {
        public List<ContentTypeDto> AllContentTypeDtos { get; set; }

        public List<string> SelectedContentTypeIds { get; set; }

        public int ExpirationMinutes { get; set; }

        public ContentfulInitialContentSettings()
        {
            AllContentTypeDtos = new List<ContentTypeDto>();
            SelectedContentTypeIds = new List<string>();
        }
    }
}
