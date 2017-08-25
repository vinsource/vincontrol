using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using vincontrol.Backend.Data;
using vincontrol.DataFeed.Helper;
using vincontrol.TaskScheduler;

//using WhitmanEnterpriseMVC.HelperClass;

namespace vincontrol.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //VINCustomSetting.SerializeTest();
            //var setting = MaintenanceInfo.GetServerMaintenance(44, Constanst.Path);
            //Console.WriteLine(setting.DateEnd);

            //EmailHelper.SendMaintenanceEmail();
            //var e = Expression.Lambda(Expression.Add(Expression.Constant(1), Expression.Constant(2)));
            //var f =e.Compile();
            //Console.Write(f.DynamicInvoke());

            //var xmlHelper = new XMLHelper();
            //var result = xmlHelper.LoadMappingTemplate(0).Mappings;

            //var exportXmlHelper = new ExportConvertHelper();
            //exportXmlHelper.ExportToFile(2,true);
            //exportXmlHelper.ExportToFile("autotrader", "local folder", "ftp folder");

            //var taskExecution = new TaskExecution();
            //taskExecution.CreateDailyTask("Export_autotrader", Environment.CurrentDirectory.Replace(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, "vincontrol.DataFeedService") + "\\vincontrol.DataFeedService.exe", "Export 2", DateTime.Now, 3
            //    , @System.Configuration.ConfigurationManager.AppSettings["DataFeedUserDomain"]
            //    , System.Configuration.ConfigurationManager.AppSettings["DataFeedPasswordDomain"]);

            //FTPHelper.ConnectToFtpVinServer();
            //FTPHelper.DownloadFromFtpServer("/public_html/vincontroldatafeed/incoming/10252.txt");
            //var stream = FTPHelper.DownloadedFile;
            
            //MarkDoneAllBucketJump(3636);
            MarkDoneAllBucketJump();
        }

        static void MarkDoneAllBucketJump()
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var settings = context.whitmanenterprisesettings.ToList();
                foreach (var dealer in settings)
                {
                    MarkDoneAllBucketJump(dealer, context);
                }
                
            }
        }

        static void MarkDoneAllBucketJump(int dealerId)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var setting = context.whitmanenterprisesettings.FirstOrDefault(i => i.DealershipId == dealerId);
                MarkDoneAllBucketJump(setting, context);
            }
        }

        static void MarkDoneAllBucketJump(whitmanenterprisesetting setting, whitmanenterprisewarehouseEntities context)
        {
            var avaiInventory = context.whitmanenterprisedealershipinventories.Where(e => e.DealershipId == setting.DealershipId && e.NewUsed.ToLower().Equals("used") && (e.Recon == null || !((bool)e.Recon))).ToList();

            foreach (var tmp in avaiInventory)
            {
                int daysInInventory = DateTime.Now.Subtract(tmp.DateInStock.GetValueOrDefault()).Days;
                //int bucketDay = tmp.BucketJumpCompleteDay.GetValueOrDefault();
                //int nextBucketDay = 0;
                //if (bucketDay == 0 || bucketDay < setting.FirstTimeRangeBucketJump.GetValueOrDefault())
                //    nextBucketDay = setting.FirstTimeRangeBucketJump.GetValueOrDefault();
                //else if (bucketDay < setting.SecondTimeRangeBucketJump.GetValueOrDefault())
                //    nextBucketDay = setting.SecondTimeRangeBucketJump.GetValueOrDefault();
                //else if (bucketDay >= setting.SecondTimeRangeBucketJump.GetValueOrDefault())
                //    nextBucketDay = bucketDay + setting.IntervalBucketJump.GetValueOrDefault();

                //bool flag = bucketDay == 0 || nextBucketDay <= daysInInventory;

                //if (flag)
                //{
                    if (daysInInventory < setting.FirstTimeRangeBucketJump.GetValueOrDefault())
                        tmp.BucketJumpCompleteDay = 0;
                    else if (daysInInventory > setting.FirstTimeRangeBucketJump.GetValueOrDefault() && daysInInventory < setting.SecondTimeRangeBucketJump.GetValueOrDefault())
                        tmp.BucketJumpCompleteDay = setting.FirstTimeRangeBucketJump.GetValueOrDefault();
                    else
                    {
                        tmp.BucketJumpCompleteDay = setting.IntervalBucketJump == 0 ? daysInInventory : setting.SecondTimeRangeBucketJump + (((daysInInventory - setting.SecondTimeRangeBucketJump) / setting.IntervalBucketJump) * setting.IntervalBucketJump);
                    }
                    
                    context.SaveChanges();
                //}
            }

        }
    }
}
