using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using WhitmanEnterpriseMVC.HelperClass;

namespace WhitmanEnterpriseMVC.Objects
{
    [Serializable]
    public class VINCustomSetting
    {
        [XmlAttribute]
        public string Key { get; set; }
        [XmlAttribute]
        public string Value { get; set; }

        public static List<VINCustomSetting> GetSetting(string configPath)
        {
            var serializer = new XmlSerializer(typeof(List<VINCustomSetting>));

            using (var reader = new StreamReader(configPath))
            {
                return (List<VINCustomSetting>)serializer.Deserialize(reader);
            }
        }

        public static void SerializeTest()
        {
            var vincustomSetting = new List<VINCustomSetting>()
                                       {
                                           new VINCustomSetting() {Key = "key 1", Value = "value 1"}
                                       };
            var x = new XmlSerializer(vincustomSetting.GetType());
            x.Serialize(Console.Out, vincustomSetting);
        }
    }
}