using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WhitmanEnterpriseMVC.DatabaseModel;

namespace WhitmanEnterpriseMVC.Models
{
    public class DealershipActivityViewModel
    {
        public DealershipActivityViewModel(){}

        public DealershipActivityViewModel(vincontroldealershipactivity obj)
        {
            Id = obj.Id;
            DealerId = obj.DealerId.GetValueOrDefault();
            UserStamp = obj.UserStamp;
            DateStamp = obj.DateStamp.GetValueOrDefault();
            Type = obj.Type;
            Title = obj.Title;
            Detail = obj.Detail;
        }

        public int Id { get; set; }
        public int DealerId { get; set; }
        public string UserStamp { get; set; }
        public DateTime DateStamp { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
    }
}