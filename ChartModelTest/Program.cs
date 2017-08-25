using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;

using System.Runtime.Serialization;
using System.Text;
using WhitmanEnterpriseMVC.Models;

/*
 * Person myPerson = new Person("Chris", "Pietschmann");

/// Serialize to JSON
 new System.Runtime.Serialization.Json.DataContractJsonSerializer(myPerson.GetType());
MemoryStream ms = new MemoryStream();
serializer.WriteObject(ms, myPerson);
string json = Encoding.Default.GetString(ms.ToArray());*/

namespace ChartModelTest
{
    class Program
    {
        static void Main()
        {
            var serializer = new DataContractJsonSerializer(typeof (ChartModel));
            var chart = new ChartModel
                            {
                                Title = new ChartModel.TitleInfo { Make = "Toyota", Model = "Camry", Year = 2004, Trim = "LE" },
                                Color = new ChartModel.ColorInfo(),
                                Certified = false,
                                VIN = "1271234vbhlfkhgeg",
                                Miles = 3000000,
                                Price = 15000,
                                Distance = Convert.ToInt32("2"),
                                Trims = new List<string> {"LE", "GE"},
                                Images = new List<string> {"POWERBOOK"},
                                Uptime = 0
                            };

            var ms = new MemoryStream();
            serializer.WriteObject(ms, chart);
            var info = Encoding.Default.GetString(ms.ToArray());
            Console.WriteLine(info);
            Console.WriteLine(info.Length * 100000);
            Console.ReadKey();
        }
    }
}
