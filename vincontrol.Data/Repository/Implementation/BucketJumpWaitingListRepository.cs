using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;
using vincontrol.Data.Repository.Interface;

namespace vincontrol.Data.Repository.Implementation
{
    public class BucketJumpWaitingListRepository : IBucketJumpWaitingListRepository
    {
          private VincontrolEntities _context;

          public BucketJumpWaitingListRepository(VincontrolEntities context)
        {
            _context = context;
        }
        public IList<BucketJumpWaitingList> GetBucketJumpWaitingLists()
        {
            return _context.BucketJumpWaitingLists.ToList();
        }

        public BucketJumpWaitingList GetBucketJumpWaitingList(int bucketJumpWaitingId)
        {
            return _context.BucketJumpWaitingLists.FirstOrDefault(x => x.WatingListId == bucketJumpWaitingId);
        }
    }
}
