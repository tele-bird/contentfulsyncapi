using System;
namespace contenfulsyncapi
{
    public class ContentfulAppSettings
    {
        public string SpaceId { get; set; }
        public string AccessToken { get; set; }
        public string Environment { get; set; }

        public override bool Equals(object obj)
        {
            return (null != obj)
                && obj.GetType().Equals(this.GetType())
                && ((ContentfulAppSettings)obj).SpaceId.Equals(this.SpaceId)
                && ((ContentfulAppSettings)obj).AccessToken.Equals(this.AccessToken)
                && ((ContentfulAppSettings)obj).Environment.Equals(this.Environment);
        }

        public override int GetHashCode()
        {
            return this.SpaceId.GetHashCode() + this.AccessToken.GetHashCode() + this.Environment.GetHashCode();
        }
    }
}
