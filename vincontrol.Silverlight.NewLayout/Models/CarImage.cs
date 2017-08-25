using System.Runtime.Serialization;

namespace vincontrol.Silverlight.NewLayout.Models
{
    [DataContract]
    public class CarImage
    {
        [DataMember]
        public string ThumbnailUrLs { get; set; }
        [DataMember]
        public string FileUrLs { get; set; }
    }
}
