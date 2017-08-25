using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;

namespace vincontrol.Application.ViewModels.TradeInManagement
{
    public class CustomerReview
    {
        public CustomerReview(){ }

        public CustomerReview(TradeInComment comment)
        {
            City = comment.City;
            Name = comment.Name;
            ReviewContent = comment.Content;
            State = comment.State;
        }

        public string ReviewContent { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
