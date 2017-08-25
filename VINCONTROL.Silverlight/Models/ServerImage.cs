using System.Runtime.Serialization;

namespace VINCONTROL.Silverlight.Models
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
