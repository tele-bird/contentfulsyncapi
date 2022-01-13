using System;
using Contentful.Core.Models;

namespace contenfulsyncapi.Dto.DB
{
    public class ContentTypeDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public ContentTypeDto()
        {
        }

        public ContentTypeDto(ContentType contentType)
        {
            Id = contentType.SystemProperties.Id;
            Name = contentType.Name;
        }
    }
}
