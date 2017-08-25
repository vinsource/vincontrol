using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Application.ViewModels.ManheimAuctionManagement;
using vincontrol.Data.Model;
using vincontrol.DomainObject;

namespace Vincontrol.Vinsell.WindesktopVersion.Helper
{
    public class SessionVar
    {
        public static DealerUser CurrentDealer;

        public static VehicleViewModel CurrentVehiceViewModel;

        public static ChartGraph MemoryChartGraph;
    }
}
