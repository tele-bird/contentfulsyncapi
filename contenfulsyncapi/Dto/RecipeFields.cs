using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace contentfulsyncapi.Dto
{
    public partial class RecipeFields
    {
        [JsonProperty("internalName")]
        public Description InternalName { get; set; }

        [JsonProperty("title")]
        public Description Title { get; set; }

        [JsonProperty("image")]
        public Image Image { get; set; }

        [JsonProperty("spotlights")]
        public ApplicableBrewerModels Spotlights { get; set; }

        [JsonProperty("description")]
        public Description Description { get; set; }

        [JsonProperty("relatedBeverages")]
        public ApplicableBrewerModels RelatedBeverages { get; set; }

        [JsonProperty("ingredients")]
        public ApplicableBrewerModels Ingredients { get; set; }

        [JsonProperty("preparationSteps")]
        public ApplicableBrewerModels PreparationSteps { get; set; }

        [JsonProperty("relatedRecipes")]
        public ApplicableBrewerModels RelatedRecipes { get; set; }

        [JsonProperty("subcategory")]
        public ApplicableBrewerModels Subcategory { get; set; }

        [JsonProperty("applicableBrewerModels")]
        public ApplicableBrewerModels ApplicableBrewerModels { get; set; }
    }

    public partial class ApplicableBrewerModels
    {
        [JsonProperty("en-US")]
        public List<EnUs> EnUs { get; set; }
    }

    public partial class EnUs
    {
        [JsonProperty("sys")]
        public Sys Sys { get; set; }
    }

    public partial class Sys
    {
        [JsonProperty("type")]
        public TypeEnum Type { get; set; }

        [JsonProperty("linkType")]
        public LinkType LinkType { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public partial class Description
    {
        [JsonProperty("en-US")]
        public string EnUs { get; set; }
    }

    public partial class Image
    {
        [JsonProperty("en-US")]
        public EnUs EnUs { get; set; }
    }

    public enum LinkType { Asset, Entry };

    public enum TypeEnum { Link };

    public partial class RecipeFields
    {
        public static RecipeFields FromJson(string json) => JsonConvert.DeserializeObject<RecipeFields>(json, contentfulsyncapi.Dto.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this RecipeFields self) => JsonConvert.SerializeObject(self, contentfulsyncapi.Dto.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                LinkTypeConverter.Singleton,
                TypeEnumConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class LinkTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(LinkType) || t == typeof(LinkType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Asset":
                    return LinkType.Asset;
                case "Entry":
                    return LinkType.Entry;
            }
            throw new Exception("Cannot unmarshal type LinkType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (LinkType)untypedValue;
            switch (value)
            {
                case LinkType.Asset:
                    serializer.Serialize(writer, "Asset");
                    return;
                case LinkType.Entry:
                    serializer.Serialize(writer, "Entry");
                    return;
            }
            throw new Exception("Cannot marshal type LinkType");
        }

        public static readonly LinkTypeConverter Singleton = new LinkTypeConverter();
    }

    internal class TypeEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(TypeEnum) || t == typeof(TypeEnum?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "Link")
            {
                return TypeEnum.Link;
            }
            throw new Exception("Cannot unmarshal type TypeEnum");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (TypeEnum)untypedValue;
            if (value == TypeEnum.Link)
            {
                serializer.Serialize(writer, "Link");
                return;
            }
            throw new Exception("Cannot marshal type TypeEnum");
        }

        public static readonly TypeEnumConverter Singleton = new TypeEnumConverter();
    }
}
