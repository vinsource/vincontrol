using Vincontrol.Vinsell.WindesktopVersion.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Vincontrol.Vinsell.WindesktopVersion.Models
{
    public class CarfaxModel:BaseModel
    {
        private string _numberofOwners;
        public string NumberofOwners
        {
            get { return _numberofOwners; }
            set
            {
                if (_numberofOwners != value)
                {
                    _numberofOwners = value;
                    OnPropertyChanged("NumberofOwners");
                }
            }
        }

        private string _serviceRecords;
        public string ServiceRecords
        {
            get { return _serviceRecords; }
            set
            {
                if (_serviceRecords != value)
                {
                    _serviceRecords = value;
                    OnPropertyChanged("ServiceRecords");
                }
            }
        }
    }


    [DataContract(Name = "carfax")]
    public class CarFax
    {
        [DataMember(Name = "owner")]
        public string Owner { get; set; }

        [DataMember(Name = "service")]
        public string Service { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "accidents")]
        public int AccidentCounts { get; set; }

        [DataMember(Name = "windowStickers")]
        public string[] WindowStickers { get; set; }
    }

}