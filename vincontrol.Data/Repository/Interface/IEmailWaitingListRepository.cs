using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;

namespace vincontrol.Data.Repository.Interface
{
    public interface IEmailWaitingListRepository
    {
        void AddNewEmailWaitingList(EmailWaitingList obj);
    }
}
