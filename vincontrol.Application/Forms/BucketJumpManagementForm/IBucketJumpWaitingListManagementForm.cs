using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;

namespace vincontrol.Application.Forms.BucketJumpManagementForm
{
    public interface IBucketJumpWaitingListManagementForm
    {
        IList<BucketJumpWaitingList> GetBucketJumpWaitingLists();
        BucketJumpWaitingList GetBucketJumpWaitingList(int bucketJumpWaitingId);
    }
}
