using System.Collections;
using System.Collections.Generic;
using vincontrol.Data.Model;

namespace vincontrol.StockingGuide.Interfaces
{
    public interface ISegmentService
    {
        void AddSegments(ICollection values);
        List<SGSegment> GetAllSegments();
    }
}
