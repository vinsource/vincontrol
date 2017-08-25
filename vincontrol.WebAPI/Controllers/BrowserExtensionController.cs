using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.CarFax;
using vincontrol.ChromeAutoService.AutomativeService;
using vincontrol.Constant;
using vincontrol.Data.Model;
using vincontrol.DomainObject;
using vincontrol.WebAPI.Helper;

namespace vincontrol.WebAPI.Controllers
{
    public class BrowserExtensionController : ApiController
    {
        private ICarFaxService _carFaxService;

        public BrowserExtensionController()
        {
            _carFaxService = new CarFaxService();
        }

        [HttpPost]
        [HttpGet]
        public ChromeVinStyleCheck CheckMultipleTrim(string vin)
        {
            var result = new ChromeVinStyleCheck();
            var autoService = new ChromeAutoService.ChromeAutoService();

            var vehicleInfo = autoService.GetVehicleInformationFromVin(vin);

            if (vehicleInfo != null &&
                (vehicleInfo.responseStatus.responseCode == ResponseStatusResponseCode.Successful ||
                 vehicleInfo.responseStatus.responseCode == ResponseStatusResponseCode.ConditionallySuccessful))
            {
                if (vehicleInfo.style != null && vehicleInfo.style.Length > 1)
                {
                    result = new ChromeVinStyleCheck()
                    {
                        DecodeSuccess = true,
                        TrimNumber = vehicleInfo.style.Select(x=>x.trim).Distinct().Count()
                    };
                    if (result.TrimNumber > 1)
                    {
                        var trimList = new List<ExtendedSelectListItem>();
                        var hashSet = new HashSet<string>();
                        foreach (var tmp in vehicleInfo.style)
                        {
                            if (!String.IsNullOrEmpty(tmp.trim))
                            {
                                if (!hashSet.Contains(tmp.trim))
                                {
                                    trimList.Add(new ExtendedSelectListItem()
                                    {
                                        Text = tmp.trim,
                                        Value = tmp.id.ToString()
                                    });

                                   
                                }

                            }
                            else
                            {

                                trimList.Add(new ExtendedSelectListItem()
                                {
                                    Text = "Base/Other Trims",
                                    Value = tmp.id.ToString()
                                });
                            }

                            hashSet.Add(tmp.trim);
                        }
                        result.TrimList = trimList;


                    }
                    
                    return result;

                }
            }

            return result;

        }


    }
}
