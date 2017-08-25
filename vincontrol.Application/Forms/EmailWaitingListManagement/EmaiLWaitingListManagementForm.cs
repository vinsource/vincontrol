using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Interface;
using vincontrol.Data.Model;
using vincontrol.Data.Repository;

namespace vincontrol.Application.Forms.EmailWaitingListManagement
{
    public class EmaiLWaitingListManagementForm : BaseForm, IEmailWaitingListManagementForm
    {
           #region Constructors
        public EmaiLWaitingListManagementForm() : this(new SqlUnitOfWork()) { }

        public EmaiLWaitingListManagementForm(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        #endregion

        public void AddNewEmailWaitingList(EmailWaitingList log)
        {
            UnitOfWork.EmailWaitingLog.AddNewEmailWaitingList(log);
            UnitOfWork.CommitVincontrolModel();
        }
    }
}
