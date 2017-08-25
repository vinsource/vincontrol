using System;
using System.Collections.Generic;
using System.Linq;
using vincontrol.Backend.Data;
using vincontrol.Backend.Interface;

namespace vincontrol.Backend.Pages
{
    public class TrackingPageViewModel
    {
        public List<TrackingItem> Files { get; set; }
        private IView _view;

        public TrackingPageViewModel(IView view)
        {
            _view = view;
            InitData();
            _view.SetDataContext(this);
        }

        private void InitData()
        {
            var context = new vincontrolwarehouseEntities();
            Files =
              (from b in  context.backendusertrackings.OrderByDescending(i => i.ActionDate)
              join c in context.backendusers
              on b.ActionBy equals  c.Id
              select
                    new TrackingItem
                        {
                            Action = b.Action,
                            ActionBy = c.UserName,
                            ActionDate = b.ActionDate,
                            ItemId = b.ItemId,
                            ItemName = b.ItemName,
                            ItemType = b.ItemType
                        }).ToList();

        }
    }

    public class TrackingItem
    {
        public string Action { get; set; }
        public string ActionBy { get; set; }
        public DateTime ActionDate { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemType { get; set; }
    }
}