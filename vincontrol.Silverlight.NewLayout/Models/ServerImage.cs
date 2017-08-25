using System.Runtime.Serialization;

namespace vincontrol.Silverlight.NewLayout.Models
{
    [DataContract]
    public class ServerImage
    {
        [DataMember]
        public string ThumbnailUrl { get; set; }
        [DataMember]
        public string FileUrl { get; set; }
    }
}
