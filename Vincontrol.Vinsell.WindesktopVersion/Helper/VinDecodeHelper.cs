using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vincontrol.Vinsell.WindesktopVersion.AutomativeDescriptionService7;

namespace Vincontrol.Vinsell.WindesktopVersion.Helper
{
    public class VinDecodeHelper
    {
        public static VehicleDescription DecodeProcessingByVin(string vin)
        {
            var autoService = new ChromeAutoService();

            VehicleDescription vehicleInfo = autoService.GetVehicleInformationFromVin(vin);

            if (vehicleInfo != null &&
                (vehicleInfo.responseStatus.responseCode == ResponseStatusResponseCode.Successful ||
                 vehicleInfo.responseStatus.responseCode == ResponseStatusResponseCode.ConditionallySuccessful))
            {
                return vehicleInfo;
            }

            return null;
        }
    }
}
