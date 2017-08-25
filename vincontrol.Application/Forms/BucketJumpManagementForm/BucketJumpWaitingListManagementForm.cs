using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Interface;
using vincontrol.Data.Model;
using vincontrol.Data.Repository;

namespace vincontrol.Application.Forms.BucketJumpManagementForm
{
    public class BucketJumpWaitingListManagementForm : BaseForm, IBucketJumpWaitingListManagementForm
    {
           #region Constructors
        public BucketJumpWaitingListManagementForm() : this(new SqlUnitOfWork()) { }

        public BucketJumpWaitingListManagementForm(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        #endregion
        public IList<BucketJumpWaitingList> GetBucketJumpWaitingLists()
        {
            return UnitOfWork.BucketJump.GetBucketJumpWaitingLists();
        }

        public BucketJumpWaitingList GetBucketJumpWaitingList(int bucketJumpWaitingId)
        {
            return UnitOfWork.BucketJump.GetBucketJumpWaitingList(bucketJumpWaitingId);
        }
    }
}
