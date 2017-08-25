using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Data.Model;

namespace vincontrol.Application.Forms.AppraisalManagement
{
    public interface IAppraisalManagementForm
    {
        void UpdateApprovedStatus(int appraisalId);
        void UpdateCustomerInfo(int appraisalId, AppraisalCustomer customer);
        void UpdateInspection(int appraisalId, int userId, InspectionFormCost inspection);
        void UpdateMileage(int appraisalId, long mileage);
        ChartSelection GetChartSelection(int appraisalId);
        Appraisal GetAppraisal(int appraisalId);
        CarShortViewModel GetCarInfo(int appraisalId);
        InspectionFormCost GetInspectionFormCost(int appraisalId);
        void UpdateAcv(int appraisalId, decimal acv);
        void UpdateCertifiedAmount(int appraisalId, decimal amount);
        void UpdateMileageAdjustment(int appraisalId, decimal amount);
        void UpdateNote(int appraisalId, string note);
        IQueryable<Appraisal> GetActiveAppraisals(int dealerId);
        IQueryable<Appraisal> GetActiveAppraisalsInDateRange(int dealerId, DateTime startDate, DateTime endDate);
        IQueryable<Appraisal> GetActiveAppraisalsByUserInDateRange(int userId, DateTime startDate, DateTime endDate);
        IQueryable<Appraisal> GetActiveAppraisalsInDateRange(IEnumerable<int> dealerList, DateTime startDate, DateTime endDate);
        IQueryable<Appraisal> GetPendingAppraisals(int dealerId);
        IQueryable<Appraisal> GetPendingAppraisals(IEnumerable<int> dealerList);
        IQueryable<Appraisal> GetPendingAppraisalsByUser(int userId);
        void UpdateChartSelection(int listingId, string isCarsCom, string options, string trims,
                                bool? isCertified, string isAll, string isFranchise, string isIndependant);
        void UpdateSmallChartSelection(int listingId, string trims);
    }
}
