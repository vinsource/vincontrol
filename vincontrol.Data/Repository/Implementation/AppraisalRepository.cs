using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Constant;
using vincontrol.Data.Model;
using vincontrol.Data.Repository.Interface;

namespace vincontrol.Data.Repository.Implementation
{
    public class AppraisalRepository : IAppraisalRepository
    {
        private VincontrolEntities _context;

        public AppraisalRepository(VincontrolEntities context)
        {
            _context = context;
        }

        public void UpdateApprovedStatus(int appraisalId)
        {
            var searchAppraisal = _context.Appraisals.FirstOrDefault(x => x.AppraisalId == appraisalId);
            if(searchAppraisal!=null)
            {
                searchAppraisal.AppraisalStatusCodeId = Constanst.AppraisalStatus.Approved;
                searchAppraisal.LastUpdated = DateTime.Now;
            }
       
        }

        public void UpdateCustomerInfo(int appraisalId, AppraisalCustomer customer)
        {
            var searchAppraisal = _context.Appraisals.FirstOrDefault(x => x.AppraisalId == appraisalId);
            if (searchAppraisal != null)
            {
                searchAppraisal.AppraisalCustomer =customer;
                searchAppraisal.LastUpdated = DateTime.Now;
            }
        }

        public void UpdateInspection(int appraisalId, int userId, InspectionFormCost inspection)
        {
            var searchInpsection = _context.InspectionFormCosts.FirstOrDefault(x => x.AppraisalID == appraisalId);
            if (searchInpsection != null)
            {
                searchInpsection.Mechanical = inspection.Mechanical;
                searchInpsection.FrontBumper = inspection.FrontBumper;
                searchInpsection.RearBumper = inspection.RearBumper;
                searchInpsection.Glass = inspection.Glass;
                searchInpsection.Tires = inspection.Tires;
                searchInpsection.FrontEnd = inspection.FrontEnd;
                searchInpsection.RearEnd = inspection.RearEnd;
                searchInpsection.DriverSide = inspection.DriverSide;
                searchInpsection.PassengerSide = inspection.PassengerSide;
                searchInpsection.Interior = inspection.Interior;
                searchInpsection.LightsBulbs = inspection.LightsBulbs;
                searchInpsection.Other = inspection.Other;
                searchInpsection.LMA = inspection.LMA;
                searchInpsection.UpdatedDate = DateTime.Now;
                searchInpsection.UpdatedUser = userId;
            }
            else
            {
                var newInspection = new InspectionFormCost()
                {
                    AppraisalID = appraisalId,
                    Mechanical = inspection.Mechanical,
                    FrontBumper = inspection.FrontBumper,
                    RearBumper = inspection.RearBumper,
                    Glass = inspection.Glass,
                    Tires = inspection.Tires,
                    FrontEnd = inspection.FrontEnd,
                    RearEnd = inspection.RearEnd,
                    DriverSide = inspection.DriverSide,
                    PassengerSide = inspection.PassengerSide,
                    Interior = inspection.Interior,
                    LightsBulbs = inspection.LightsBulbs,
                    Other = inspection.Other,
                    LMA = inspection.LMA,
                    UpdatedDate = DateTime.Now,
                    UpdatedUser = userId,
                };
                
                _context.AddToInspectionFormCosts(newInspection);
            }
        }

        public void UpdateAcv(int appraisalId, decimal acv)
        {
            var searchAppraisal = _context.Appraisals.FirstOrDefault(x => x.AppraisalId == appraisalId);
            if (searchAppraisal != null)
            {
                searchAppraisal.ACV = acv;
                searchAppraisal.LastUpdated = DateTime.Now;
            }
        }

        public void UpdateMileage(int appraisalId, long mileage)
        {
            var searchAppraisal = _context.Appraisals.FirstOrDefault(x => x.AppraisalId == appraisalId);
            if (searchAppraisal != null)
            {
                searchAppraisal.Mileage = mileage;
                searchAppraisal.LastUpdated = DateTime.Now;
            }
        }

        public void UpdateChartSelection(int listingId, string isCarsCom, string options, string trims, bool? isCertified, string isAll,
            string isFranchise, string isIndependant)
        {
            var existingChartSelection =
               _context.ChartSelections.FirstOrDefault(
                   s => s.ListingId == listingId && s.VehicleStatusCodeId == Constanst.VehicleStatus.Appraisal);
            if (existingChartSelection != null)
            {
                existingChartSelection.IsAll = Convert.ToBoolean(isAll);
                existingChartSelection.IsCarsCom = Convert.ToBoolean(isCarsCom);
                existingChartSelection.IsCertified = isCertified;
                existingChartSelection.IsFranchise = Convert.ToBoolean(isFranchise);
                existingChartSelection.IsIndependant = Convert.ToBoolean(isIndependant);
                existingChartSelection.Options = options.IndexOf(',') > 0
                    ? (options.Split(',')[0].Equals("0") ? "0" : options)
                    : options.ToLower();
                existingChartSelection.Trims = trims.IndexOf(',') > 0
                    ? (trims.Split(',')[0].Equals("0") ? null : trims)
                    : trims.Equals("null") ? null : trims.ToLower();


            }
            else
            {
                var newSelection = new ChartSelection
                {
                    ListingId = Convert.ToInt32(listingId),
                    IsAll = Convert.ToBoolean(isAll),
                    IsCarsCom = Convert.ToBoolean(isCarsCom),
                    IsCertified = isCertified,
                    IsFranchise = Convert.ToBoolean(isFranchise),
                    IsIndependant = Convert.ToBoolean(isIndependant),
                    Options =
                        options.IndexOf(',') > 0
                            ? (options.Split(',')[0].Equals("0") ? "0" : options)
                            : options,
                    Trims =
                        trims.IndexOf(',') > 0
                            ? (trims.Split(',')[0].Equals("0") ? null : trims)
                            : trims.Equals("null") ? null : trims.ToLower(),
                    VehicleStatusCodeId = Constanst.VehicleStatus.Appraisal,

                };
                _context.AddToChartSelections(newSelection);

            }
        }

        public void UpdateSmallChartSelection(int listingId, string trims)
        {
            var existingChartSelection = _context.ChartSelections.FirstOrDefault(s => s.ListingId == listingId && s.VehicleStatusCodeId == Constanst.VehicleStatus.Appraisal);
            if (existingChartSelection != null)
            {
                existingChartSelection.Trims = trims.IndexOf(',') > 0
                                                   ? (trims.Split(',')[0].Equals("0") ? "0" : trims)
                                                   : trims.Equals("null") ? null : trims.ToLower();

                _context.SaveChanges();
            }
            else
            {
                var newSelection = new ChartSelection
                {
                    ListingId = Convert.ToInt32(listingId),
                    Trims =
                        trims.IndexOf(',') > 0
                            ? (trims.Split(',')[0].Equals("0") ? "0" : trims)
                            : trims.Equals("null") ? null : trims.ToLower(),
                    VehicleStatusCodeId = Constanst.VehicleStatus.Appraisal
                };
                _context.AddToChartSelections(newSelection);

            }
        }

        public Appraisal GetAppraisal(int appraisalId)
        {
            return _context.Appraisals.FirstOrDefault(x => x.AppraisalId == appraisalId);
        }

        public ChartSelection GetChartSelection(int appraisalId)
        {
            return _context.ChartSelections.FirstOrDefault(x => x.ListingId == appraisalId && x.VehicleStatusCodeId==Constanst.VehicleStatus.Appraisal);
        }

        public IQueryable<Appraisal> GetActiveAppraisals(int dealerId)
        {
            return
                _context.Appraisals.Where(
                    x =>
                        x.DealerId == dealerId && x.AppraisalStatusCodeId == null ||
                        x.AppraisalStatusCodeId != Constanst.AppraisalStatus.Pending);
        }

        public IQueryable<Appraisal> GetActiveAppraisalsInDateRange(int dealerId, DateTime startDate, DateTime endDate)
        {
            return
                _context.Appraisals.Where(
                    x =>
                        x.DealerId == dealerId &&
                        (x.AppraisalStatusCodeId == null || x.AppraisalStatusCodeId != Constanst.AppraisalStatus.Pending) &&
                        x.AppraisalDate.Value >= startDate && x.AppraisalDate.Value <= endDate);
        }

        public IQueryable<Appraisal> GetActiveAppraisalsInDateRange(IEnumerable<int> dealerList, DateTime startDate, DateTime endDate)
        {
            return _context.Appraisals.Where(LinqExtendedHelper.BuildContainsExpression<Appraisal, int>(e => e.DealerId, dealerList)).Where(x=>
                    (x.AppraisalStatusCodeId == null || x.AppraisalStatusCodeId != Constanst.AppraisalStatus.Pending) &&
                      x.AppraisalDate.Value >= startDate && x.AppraisalDate.Value <= endDate );
           
        }

       

        public IQueryable<Appraisal> GetActiveAppraisalsByUserInDateRange(int userId, DateTime startDate, DateTime endDate)
        {
            return
                   _context.Appraisals.Where(
                       x =>
                           x.AppraisalById == userId &&
                           (x.AppraisalStatusCodeId == null || x.AppraisalStatusCodeId != Constanst.AppraisalStatus.Pending) &&
                           x.AppraisalDate.Value >= startDate && x.AppraisalDate.Value <= endDate);
        }

        public IQueryable<Appraisal> GetPendingAppraisals(int dealerId)
        {
            return
                _context.Appraisals.Where(
                    x =>
                        x.DealerId == dealerId &&x.AppraisalStatusCodeId == Constanst.AppraisalStatus.Pending);
        }

        public IQueryable<Appraisal> GetPendingAppraisals(IEnumerable<int> dealerList)
        {
            
            return _context.Appraisals.Where(LinqExtendedHelper.BuildContainsExpression<Appraisal, int>(e => e.DealerId, dealerList)).Where(x=>
                   x.AppraisalStatusCodeId == Constanst.AppraisalStatus.Pending );
           
        }

        public IQueryable<Appraisal> GetPendingAppraisalsByUser(int userId)
        {
            return
              _context.Appraisals.Where(
                  x =>
                      x.AppraisalById == userId &&x.AppraisalStatusCodeId == Constanst.AppraisalStatus.Pending);
        }

        public InspectionFormCost GetInspectionFormCost(int appraisalId)
        {
            return _context.InspectionFormCosts.FirstOrDefault(x => x.AppraisalID == appraisalId);
        }
    }


}
