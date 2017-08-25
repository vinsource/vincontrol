using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Vincontrol.Vinsell.WindesktopVersion.AutomativeDescriptionService7;
using vincontrol.Helper;

namespace Vincontrol.Vinsell.WindesktopVersion.Helper
{
    public class ChromeAutoService
    {
        private readonly AccountInfo _mAi;

        private readonly Description7aPortTypeClient _vinService = new Description7aPortTypeClient();

        public ChromeAutoService()
        {
            _mAi = new AccountInfo
            {
                number = ConfigurationManager.AppSettings["accountNumber"],
                secret = ConfigurationManager.AppSettings["accountSecret"],
                country = ConfigurationManager.AppSettings["country"],
                language = ConfigurationManager.AppSettings["language"]
            };
        }

        public AccountInfo GetAccountInfo()
        {
            return _mAi;
        }

        public int[] GetModelYears()
        {
            var baseReuqest = new BaseRequest { accountInfo = GetAccountInfo() };

            ModelYears temp = _vinService.getModelYears(baseReuqest);

            if (temp != null && temp.modelYear.Any())
                return temp.modelYear;

            return null;
        }

        public IdentifiedString[] GetDivisions(int mModelYear)
        {
            var mDr = new DivisionsRequest { accountInfo = GetAccountInfo(), modelYear = mModelYear };

            Divisions temp = _vinService.getDivisions(mDr);

            if (temp != null && temp.division.Any())
                return temp.division;

            return null;
        }

        public IdentifiedString[] GetSubdivisions(int mModelYear)
        {
            var mSdr = new SubdivisionsRequest { accountInfo = GetAccountInfo(), modelYear = mModelYear };

            Subdivisions temp = _vinService.getSubdivisions(mSdr);

            if (temp != null && temp.subdivision.Any())
                return temp.subdivision;

            return null;
        }

        public VehicleDescription GetVehicleInformationFromVin(string mVin)
        {
            var mSifsir = new VehicleDescriptionRequest
            {
                accountInfo = GetAccountInfo(),
                Items = new object[] { mVin },
                ItemsElementName = new[] { ItemsChoiceType.vin },
                @switch =
                    new[]
                            {
                                Switch.DisableSafeStandards, Switch.IncludeDefinitions, Switch.IncludeRegionalVehicles,
                                Switch.ShowAvailableEquipment, Switch.ShowConsumerInformation,
                                Switch.ShowExtendedDescriptions, Switch.ShowExtendedTechnicalSpecifications
                            }
            };

            VehicleDescription temp = _vinService.describeVehicle(mSifsir);

            if (temp == null) return null;

            if (temp.responseStatus.responseCode.Equals(ResponseStatusResponseCode.Successful) && temp.modelYear > 0)
                return temp;
            return null;
        }

        public VehicleDescription GetVehicleInformationFromVin(string mVin, int reducingStyleId)
        {
            var mSifsir = new VehicleDescriptionRequest
            {
                accountInfo = GetAccountInfo(),
                Items = new object[] { mVin, reducingStyleId },
                ItemsElementName = new[] { ItemsChoiceType.vin, ItemsChoiceType.reducingStyleId, },
                @switch =
                    new[]
                            {
                                Switch.DisableSafeStandards, Switch.IncludeDefinitions, Switch.IncludeRegionalVehicles,
                                Switch.ShowAvailableEquipment, Switch.ShowConsumerInformation,
                                Switch.ShowExtendedDescriptions, Switch.ShowExtendedTechnicalSpecifications
                            }
            };

            VehicleDescription temp = _vinService.describeVehicle(mSifsir);

            if (temp == null) return null;

            return temp.responseStatus.responseCode.Equals(ResponseStatusResponseCode.Successful) ? temp : null;
        }

        public VehicleDescription GetVehicleInformationFromYearMakeModel(int mYear, string mMake, string mModel)
        {
            var mSifsir = new VehicleDescriptionRequest
            {
                accountInfo = GetAccountInfo(),
                Items = new object[] { mYear, mMake, mModel },
                ItemsElementName =
                    new[] { ItemsChoiceType.modelYear, ItemsChoiceType.makeName, ItemsChoiceType.modelName, },
                @switch =
                    new[]
                            {
                                Switch.DisableSafeStandards, Switch.IncludeDefinitions, Switch.IncludeRegionalVehicles,
                                Switch.ShowAvailableEquipment, Switch.ShowConsumerInformation,
                                Switch.ShowExtendedDescriptions, Switch.ShowExtendedTechnicalSpecifications
                            }
            };

            VehicleDescription temp = _vinService.describeVehicle(mSifsir);

            if (temp == null) return null;

            return temp.responseStatus.responseCode.Equals(ResponseStatusResponseCode.Successful) ? temp : null;
        }

        public VehicleDescription GetStyleInformationFromStyleId(int styleId)
        {
            var mSifsir = new VehicleDescriptionRequest
            {
                accountInfo = GetAccountInfo(),
                Items = new object[] { styleId },
                ItemsElementName = new[] { ItemsChoiceType.styleId },
                @switch =
                    new[]
                            {
                                Switch.DisableSafeStandards, Switch.IncludeDefinitions, Switch.IncludeRegionalVehicles,
                                Switch.ShowAvailableEquipment, Switch.ShowConsumerInformation,
                                Switch.ShowExtendedDescriptions, Switch.ShowExtendedTechnicalSpecifications
                            }
            };

            VehicleDescription temp = _vinService.describeVehicle(mSifsir);

            if (temp == null) return null;

            return temp.responseStatus.responseCode.Equals(ResponseStatusResponseCode.Successful) ? temp : null;
        }

        public bool ValidateVin(string mVin)
        {
            var mSifsir = new VehicleDescriptionRequest
            {
                accountInfo = GetAccountInfo(),
                Items = new object[] { mVin },
                ItemsElementName = new[] { ItemsChoiceType.vin },
                @switch =
                    new[]
                            {
                                Switch.DisableSafeStandards, Switch.IncludeDefinitions, Switch.IncludeRegionalVehicles,
                                Switch.ShowAvailableEquipment, Switch.ShowConsumerInformation,
                                Switch.ShowExtendedDescriptions, Switch.ShowExtendedTechnicalSpecifications
                            }
            };

            VehicleDescription temp = _vinService.describeVehicle(mSifsir);
            if (temp != null)
            {
                if (temp.responseStatus.responseCode.Equals(ResponseStatusResponseCode.Successful) ||
                    temp.responseStatus.responseCode.Equals(ResponseStatusResponseCode.ConditionallySuccessful))
                    return true;
            }

            return false;
        }

      
    }
}
