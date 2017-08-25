using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Data.Interface;
using vincontrol.Data.Model;
using vincontrol.Data.Repository;

namespace vincontrol.Application.Forms.AppraisalManagement
{
    public class AppraisalManagementForm : BaseForm, IAppraisalManagementForm
    {
        #region Constructors

        public AppraisalManagementForm() : this(new SqlUnitOfWork())
        {
        }

        public AppraisalManagementForm(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        #endregion

        #region IAppraisalManagementForm Members

        #endregion

        public void UpdateApprovedStatus(int appraisalId)
        {
            UnitOfWork.Appraisal.UpdateApprovedStatus(appraisalId);
            UnitOfWork.CommitVincontrolModel();
        }

        public void UpdateCustomerInfo(int appraisalId, AppraisalCustomer customer)
        {
            UnitOfWork.Appraisal.UpdateCustomerInfo(appraisalId, customer);
            UnitOfWork.CommitVincontrolModel();
        }

        public void UpdateInspection(int appraisalId, int userId, InspectionFormCost inspection)
        {
            UnitOfWork.Appraisal.UpdateInspection(appraisalId, userId, inspection);
            UnitOfWork.CommitVincontrolModel();
        }

        public void UpdateMileage(int appraisalId, long mileage)
        {
            UnitOfWork.Appraisal.UpdateMileage(appraisalId, mileage);
            UnitOfWork.CommitVincontrolModel();
        }

        public ChartSelection GetChartSelection(int appraisalId)
        {
            return UnitOfWork.Appraisal.GetChartSelection(appraisalId);
        }

        public Appraisal GetAppraisal(int appraisalId)
        {
            return UnitOfWork.Appraisal.GetAppraisal(appraisalId);
        }

        public CarShortViewModel GetCarInfo(int appraisalId)
        {
            var existingRecord = UnitOfWork.Appraisal.GetAppraisal(appraisalId);
            return existingRecord != null ? new CarShortViewModel(existingRecord) : new CarShortViewModel();
        }

        public InspectionFormCost GetInspectionFormCost(int appraisalId)
        {
            return UnitOfWork.Appraisal.GetInspectionFormCost(appraisalId);
        }

        public void UpdateAcv(int appraisalId, decimal acv)
        {
            UnitOfWork.Appraisal.UpdateAcv(appraisalId,acv);
            UnitOfWork.CommitVincontrolModel();
        }

        public void UpdateCertifiedAmount(int appraisalId, decimal amount)
        {
            var existingRecord = UnitOfWork.Appraisal.GetAppraisal(appraisalId);
            if (existingRecord != null)
            {
                existingRecord.CertifiedAmount = amount;
                UnitOfWork.CommitVincontrolModel();
            }
        }
        
        public void UpdateMileageAdjustment(int appraisalId, decimal amount)
        {
            var existingRecord = UnitOfWork.Appraisal.GetAppraisal(appraisalId);
            if (existingRecord != null)
            {
                existingRecord.MileageAdjustment = amount;
                UnitOfWork.CommitVincontrolModel();
            }
        }

        public void UpdateNote(int appraisalId, string note)
        {
            var existingRecord = UnitOfWork.Appraisal.GetAppraisal(appraisalId);
            if (existingRecord != null)
            {
                existingRecord.Note = note;
                UnitOfWork.CommitVincontrolModel();
            }
        }

        public IQueryable<Appraisal> GetActiveAppraisals(int dealerId)
        {
            return UnitOfWork.Appraisal.GetActiveAppraisals(dealerId);
        }

        public IQueryable<Appraisal> GetActiveAppraisalsByUserInDateRange(int userId, DateTime startDate,
          DateTime endDate)
        {
            return UnitOfWork.Appraisal.GetActiveAppraisalsByUserInDateRange(userId, startDate, endDate);
        }


        public IQueryable<Appraisal> GetActiveAppraisalsInDateRange(int dealerId, DateTime startDate, DateTime endDate)
        {
            return UnitOfWork.Appraisal.GetActiveAppraisalsInDateRange(dealerId, startDate, endDate);
        }

      
        public IQueryable<Appraisal> GetActiveAppraisalsInDateRange(IEnumerable<int> dealerList, DateTime startDate, DateTime endDate)
        {
            return UnitOfWork.Appraisal.GetActiveAppraisalsInDateRange(dealerList, startDate, endDate);
        }

      

        public IQueryable<Appraisal> GetPendingAppraisals(int dealerId)
        {
            return UnitOfWork.Appraisal.GetPendingAppraisals(dealerId);
        }

        public IQueryable<Appraisal> GetPendingAppraisals(IEnumerable<int> dealerList)
        {
            return UnitOfWork.Appraisal.GetPendingAppraisals(dealerList);
        }

       

        public IQueryable<Appraisal> GetPendingAppraisalsByUser(int userId)
        {
            return UnitOfWork.Appraisal.GetPendingAppraisalsByUser(userId);
        }

        public void UpdateChartSelection(int listingId, string isCarsCom, string options, string trims, bool? isCertified, string isAll,
            string isFranchise, string isIndependant)
        {
            UnitOfWork.Appraisal.UpdateChartSelection(listingId, isCarsCom, options, trims, isCertified, isAll, isFranchise, isIndependant);
            UnitOfWork.CommitVincontrolModel();
        }

        public void UpdateSmallChartSelection(int listingId, string trims)
        {
            UnitOfWork.Appraisal.UpdateSmallChartSelection(listingId,  trims);
            UnitOfWork.CommitVincontrolModel();
        }
    }
}
