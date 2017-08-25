using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;

namespace vincontrol.Application.ViewModels.CommonManagement
{
    public class StateViewModel
    {
        public StateViewModel(){}

        public StateViewModel(State obj)
        {
            StateId = obj.StateId;
            Name = obj.Name;
            Cities = obj.Cities.Any()
                ? obj.Cities.Select(i => new CityViewModel(i)).ToList()
                : new List<CityViewModel>();
        }

        public int StateId { get; set; }
        public string Name { get; set; }
        public List<CityViewModel> Cities { get; set; }
    }

    public class CityViewModel
    {
        public CityViewModel(){}

        public CityViewModel(City obj)
        {
            CityId = obj.CityId;
            StateId = obj.StateId;
            Name = obj.Name;
            Url = obj.Url;
        }

        public int CityId { get; set; }
        public int StateId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class VSRScheduleViewModel
    {
        public VSRScheduleViewModel(){}

        public VSRScheduleViewModel(VSRSchedule obj)
        {
            ScheduleId = obj.ScheduleId;
            StartTime = obj.StartTime.GetValueOrDefault();
            FinishTime = obj.FinishTime.GetValueOrDefault();
            Day = obj.Day.GetValueOrDefault();
            Status = obj.Status.GetValueOrDefault();
            DealerId = obj.DealerId.GetValueOrDefault();
            TeamId = obj.TeamId.GetValueOrDefault();
        }

        public int ScheduleId { get; set; }
        public int StartTime { get; set; }
        public int FinishTime { get; set; }
        public int Day { get; set; }
        public int Status { get; set; }
        public int DealerId { get; set; }
        public int TeamId { get; set; }
    }
}
