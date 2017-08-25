using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;

namespace vincontrol.Application.Forms.EmailWaitingListManagement
{
    public interface IEmailWaitingListManagementForm
    {
        void AddNewEmailWaitingList(EmailWaitingList log);
    }
}
