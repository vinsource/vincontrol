using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using vincontrol.Data.Model;

namespace vincontrol.Application.Vinsocial.ViewModels.ReviewManagement
{
    [DataContract]
    public class ContactViewModel
    {
        public ContactViewModel() { }

        public ContactViewModel(VPContactInfo obj)
        {
            Options = obj.Options;
            vinnumber = obj.Vin;
            dealerId = obj.DealerId;
            contact_type = obj.ContactType ?? 0;
            contact_prefer = obj.ContactPrefer;
            lastname = obj.LastName;
            firstname = obj.FirstName;
            email_address = obj.Email;
            phone_number = obj.Phone;
            comment = obj.Comment;

            address = obj.Address;
            zipcode = obj.Postal ?? 0;
            city = obj.City;
            state = obj.State;

            ModelYear = obj.Year ?? 0;
            Make = obj.Make;
            Model = obj.Model;
            Trim = obj.Trim;
            StockNumber = obj.Stock;

            engine = obj.Engine;
            transmission = obj.Transmission;
            exterior_color = obj.ExteriorColor;
            mileage = obj.Mileage ?? 0;
            condition = obj.Condition;

            schedule_date = obj.ScheduleDate ?? new DateTime();
            schedule_time = obj.ScheduleTime;
            offer_value = obj.OfferValue ?? 0;
            friendname = obj.FriendName;
            friendemail = obj.FriendEmail;
            RegistDate = obj.RegisterDate ?? new DateTime();
            ServiceType = obj.ServiceType;
        }

        public int dealerId { get; set; }
        public int contact_type { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string contact_prefer { get; set; }
        public string email_address { get; set; }
        public string phone_number { get; set; }
        public string vinnumber { get; set; }
        public string comment { get; set; }

        public string address { get; set; }
        public int zipcode { get; set; }
        public string city { get; set; }
        public string state { get; set; }

        public int ModelYear { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Trim { get; set; }
        public string StockNumber { get; set; }
        public string ServiceType { get; set; }
        public string engine { get; set; }
        public string transmission { get; set; }
        public string exterior_color { get; set; }
        public int mileage { get; set; }
        public string condition { get; set; }

        public bool IsSolded { get; set; }

        public DateTime schedule_date { get; set; }
        public string schedule_time { get; set; }

        public int offer_value { get; set; }

        public string friendemail { get; set; }
        public string friendname { get; set; }
        public DateTime RegistDate { get; set; }

        public string DealerEmail { get; set; }

        public string DetailUrl { get; set; }
        public string DealerName { get; set; }
        public string Options { get; set; }
    }
}
