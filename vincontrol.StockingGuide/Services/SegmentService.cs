using System.Collections;
using System.Collections.Generic;
using System.Linq;
using vincontrol.Data.Model;
using vincontrol.StockingGuide.Interfaces;
using vincontrol.StockingGuide.Repository.Interfaces;
using vincontrol.StockingGuide.Repository.UnitOfWorks;

namespace vincontrol.StockingGuide.Services
{
    public class SegmentService : ISegmentService
    {
        private IVincontrolUnitOfWork _vincontrolUnitOfWork;

        public SegmentService()
        {
            _vincontrolUnitOfWork = new VincontrolUnitOfWork();
        }
        public void AddSegments(ICollection values)
        {
            foreach (var value in values)
            {
                _vincontrolUnitOfWork.SegmentRepository.Add(new SGSegment() { Name = value.ToString() });
            }
            _vincontrolUnitOfWork.Commit();
        }

        public List<SGSegment> GetAllSegments()
        {
            return _vincontrolUnitOfWork.SegmentRepository.FindAll().ToList();
        }
    }
}
