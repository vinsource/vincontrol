using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;

namespace vincontrol.Data.Repository.Interface
{
    public interface IAppraisalRepository
    {
        void UpdateApprovedStatus(int appraisalId);
        void UpdateCustomerInfo(int appraisalId, AppraisalCustomer customer);
        void UpdateInspection(int appraisalId, int userId, InspectionFormCost inspection);
        void UpdateAcv(int appraisalId, decimal acv);
        void UpdateMileage(int appraisalId, long mileage);
        void UpdateChartSelection(int listingId, string isCarsCom, string options, string trims,
                           bool? isCertified, string isAll, string isFranchise, string isIndependant);
        void UpdateSmallChartSelection(int listingId, string trims);
        Appraisal GetAppraisal(int appraisalId);
        ChartSelection GetChartSelection(int appraisalId);
        IQueryable<Appraisal> GetActiveAppraisals(int dealerId);
        IQueryable<Appraisal> GetActiveAppraisalsInDateRange(int dealerId, DateTime startDate, DateTime endDate);
        IQueryable<Appraisal> GetActiveAppraisalsInDateRange(IEnumerable<int> dealerList, DateTime startDate, DateTime endDate);
        IQueryable<Appraisal> GetActiveAppraisalsByUserInDateRange(int userId, DateTime startDate, DateTime endDate);
        IQueryable<Appraisal> GetPendingAppraisals(int dealerId);
        IQueryable<Appraisal> GetPendingAppraisals(IEnumerable<int> dealerList);
        IQueryable<Appraisal> GetPendingAppraisalsByUser(int userId);
        InspectionFormCost GetInspectionFormCost(int appraisalId);
    }
}
