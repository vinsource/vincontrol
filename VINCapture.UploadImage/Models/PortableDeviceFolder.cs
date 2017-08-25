using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VINCapture.UploadImage.Models
{
    public class PortableDeviceFolder : PortableDeviceObject
    {
        public PortableDeviceFolder(string id, string name)
            : base(id, name)
        {
            this.Files = new List<PortableDeviceObject>();
        }

        public IList<PortableDeviceObject> Files { get; set; }
    }
}