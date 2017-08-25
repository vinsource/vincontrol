using System;
using System.Configuration;
using System.IO;
using Autofac;
using Autofac.Core;
using log4net;
using vincontrol.StockingGuide.Common;
using vincontrol.StockingGuide.Common.Helpers;
using vincontrol.StockingGuide.Interfaces;
using vincontrol.StockingGuide.MockServices;
using vincontrol.StockingGuide.Service;
using vincontrol.StockingGuide.Service.Contracts;
using vincontrol.StockingGuide.Services;

namespace vincontrol.StockingGuide.WindowService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ILog logEngine = LogManager.GetLogger(typeof(Program));
      
            var builder = new ContainerBuilder();
            RegisterToContainer(builder);
            LogHelper.InitLogger(logEngine);
            using (var container = builder.Build())
            {
                if (ConfigurationManager.AppSettings["RunCreateSegmentProcess"] == "true")
                {
                    var createSegmentProcess = container.Resolve<ICreateSegmentProcess>();
                    createSegmentProcess.Run();
                }

                if (ConfigurationManager.AppSettings["RunCreateSegmentWithMakeModelProcess"] == "true")
                {
                    var createSegmentWithMakeModelProcess = container.Resolve<ICreateSegmentWithMakeModelProcess>();
                    createSegmentWithMakeModelProcess.Run();
                }

                if (ConfigurationManager.AppSettings["RunUpdateSegmentChromeProcess"] == "true")
                {
                    var updateSegmentChromeProcess = container.Resolve<IUpdateSegmentChromeProcess>();
                    updateSegmentChromeProcess.Run();
                }

                if (ConfigurationManager.AppSettings["RunUpdateSegmentChromeWithMakeModelProcess"] == "true")
                {
                    var updateSegmentChromeWithMakeModelProcess = container.Resolve<IUpdateSegmentChromeWithMakeModelProcess>();
                    updateSegmentChromeWithMakeModelProcess.Run();
                }

                if (ConfigurationManager.AppSettings["RunUpdateBrandStockingGuideProcess"] == "true")
                {
                    ServiceLog.Info("Start IUpdateBrandStockingGuideProcess");
                    var updateBrandStockingGuideProcess = container.Resolve<IUpdateBrandStockingGuideProcess>();
                    updateBrandStockingGuideProcess.Run();
                    ServiceLog.Info("End IUpdateBrandStockingGuideProcess");
                }

                if (ConfigurationManager.AppSettings["RunUpdateSegmentDealerProcess"] == "true")
                {
                    ServiceLog.Info("Start IUpdateSegmentDealerProcess");
                    var updateSegmentDealerProcess = container.Resolve<IUpdateSegmentDealerProcess>();
                    updateSegmentDealerProcess.Run();
                    ServiceLog.Info("End IUpdateSegmentDealerProcess");
                }

                if (ConfigurationManager.AppSettings["RunUpdateSegmentInventoryDetailProcess"] == "true")
                {
                    ServiceLog.Info("Start IUpdateSegmentInventoryDetailProcess");
                    var updateSegmentInventoryDetailProcess = container.Resolve<IUpdateSegmentInventoryDetailProcess>();
                    updateSegmentInventoryDetailProcess.Run();
                    ServiceLog.Info("End IUpdateSegmentInventoryDetailProcess");
                }

                //ERROR HERE
                if (ConfigurationManager.AppSettings["RunUpdateSegmentMarketDetailProcess"] == "true")
                {
                    ServiceLog.Info("Start IUpdateSegmentMarketDetailProcess");
                    var updateSegmentMarketDetailProcess = container.Resolve<IUpdateSegmentMarketDetailProcess>();
                    updateSegmentMarketDetailProcess.Run();
                    ServiceLog.Info("End IUpdateSegmentMarketDetailProcess");
                }

                if (ConfigurationManager.AppSettings["RunUpdateWeeklyTurnOverByMonthProcess"] == "true")
                {
                    logEngine.Info("Start IUpdateWeeklyTurnOverByMonthProcess");
                    var updateTurnOverByMonthProcess = container.Resolve<IUpdateWeeklyTurnOverByMonthProcess>();
                    updateTurnOverByMonthProcess.Run();
                    logEngine.Info("End IUpdateWeeklyTurnOverByMonthProcess");
                }
            }
        }

        private static void RegisterToContainer(ContainerBuilder builder)
        {
            builder.RegisterType<UpdateWeeklyTurnOverByMonthProcess>().As<IUpdateWeeklyTurnOverByMonthProcess>();
            builder.RegisterType<UpdateBrandStockingGuideProcess>().As<IUpdateBrandStockingGuideProcess>();
            builder.RegisterType<UpdateSegmentChromeProcess>().As<IUpdateSegmentChromeProcess>();
            builder.RegisterType<UpdateSegmentDealerProcess>().As<IUpdateSegmentDealerProcess>();
            builder.RegisterType<UpdateSegmentInventoryDetailProcess>().As<IUpdateSegmentInventoryDetailProcess>();
            builder.RegisterType<UpdateSegmentMarketDetailProcess>().As<IUpdateSegmentMarketDetailProcess>();
            builder.RegisterType<CreateSegmentProcess>().As<ICreateSegmentProcess>();
            builder.RegisterType<CreateSegmentWithMakeModelProcess>().As<ICreateSegmentWithMakeModelProcess>();
            builder.RegisterType<UpdateSegmentChromeProcess>().As<IUpdateSegmentChromeProcess>();
            builder.RegisterType<UpdateSegmentChromeWithMakeModelProcess>().As<IUpdateSegmentChromeWithMakeModelProcess>();
            builder.RegisterType<UpdateSegmentChromeProcess>().As<IUpdateSegmentChromeProcess>();

            builder.RegisterType<UpdateMarketDetailWithTrimProcess>().As<IUpdateMarketDetailWithTrimProcess>();
          
            builder.RegisterType<InventoryService>().As<IInventoryService>();
            builder.RegisterType<WeeklyTurnOverService>().As<IWeeklyTurnOverService>();
            if (ConfigurationManager.AppSettings["Run3636DealerOnly"] == "true")
            {
                builder.RegisterType<MockDealerService>().As<IDealerService>();
            }
            else
            {
                builder.RegisterType<DealerService>().As<IDealerService>();
            }
            builder.RegisterType<ChromeService>().As<IChromeService>();
            builder.RegisterType<DealerBrandService>().As<IDealerBrandService>();
            builder.RegisterType<DealerSegmentService>().As<IDealerSegmentService>();
            builder.RegisterType<InventorySegmentDetailService>().As<IInventorySegmentDetailService>();
            //builder.RegisterType<KPIInfoService>().As<IKPIInfoService>();
            if (ConfigurationManager.AppSettings["Debug"] == "true")
            {
                builder.RegisterType<MockSoldMarketVehicleService>().As<ISoldMarketVehicleService>();
                builder.RegisterType<MockMarketVehicleService>().As<IMarketVehicleService>();
                builder.RegisterType<MockSoldMarketTruckService>().As<ISoldMarketTruckService>();
                builder.RegisterType<MockMarketTruckService>().As<IMarketTruckService>();
            }
            else
            {
                builder.RegisterType<SoldMarketVehicleService>().As<ISoldMarketVehicleService>();
                builder.RegisterType<MarketVehicleService>().As<IMarketVehicleService>();
                builder.RegisterType<SoldMarketTruckService>().As<ISoldMarketTruckService>();
                builder.RegisterType<MarketTruckService>().As<IMarketTruckService>();
            }

            builder.RegisterType<MarketSegmentDetailService>().As<IMarketSegmentDetailService>();
            builder.RegisterType<SegmentService>().As<ISegmentService>();
            builder.RegisterType<SettingService>().As<ISettingService>();
            //builder.RegisterType<StockingGuideInfoService>().As<IStockingGuideInfoService>();
        }
    }
}
