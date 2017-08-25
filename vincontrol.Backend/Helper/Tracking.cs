using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Backend.Data;

namespace vincontrol.Backend.Helper
{
    public class Tracking
    {
        public static void Log(UserAction action, int actionBy, DateTime actionDate, int itemId, string itemName, ItemType itemType)
        {
            var context = new vincontrolwarehouseEntities();
            context.AddTobackendusertrackings(new backendusertracking() { Action = action.ToString(), ActionBy = actionBy, ActionDate = actionDate, ItemId = itemId, ItemName = itemName, ItemType = itemType.ToString() });
            context.SaveChanges();
        }
    }

    public enum UserAction
    {
        Delete, Insert, Update, Pause, Continue, CreateTask, PushManually
    }

    public enum ItemType
    {
        ImportProfile, ExportProfile, ImportProfileDealer, ExportProfileDealer
    }
}
