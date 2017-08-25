using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;
using vincontrol.Data.Repository.Interface;

namespace vincontrol.Data.Repository.Implementation
{
    public class ManheimRepository : IManheimRepository
    {
        private VincontrolEntities _context;

        public ManheimRepository(VincontrolEntities context)
        {
            _context = context;
        }

        #region IManheimRepository' Members

        public int MatchingMake(string make)
        {
            var matchingMake = _context.ManheimMakes.FirstOrDefault(i => i.Name.ToLower().Equals(make.ToLower()));
            return matchingMake == null ? 0 : matchingMake.ServiceId;
        }

        public int MatchingModel(string model, int makeServiceId)
        {
            var makeId = GetMakeByServiceId(makeServiceId).ManheimMakeId;
            var matchingModel =
                _context.ManheimMakeModels.FirstOrDefault(
                    i => i.ManheimModel.Name.ToLower().Contains(model.ToLower()) && i.ManheimYearMake.MakeId == makeId);
            return matchingModel == null ? 0 : matchingModel.ManheimModel.ServiceId;
        }

        public int[] MatchingModels(string model, int makeServiceId)
        {
            var makeId = GetMakeByServiceId(makeServiceId).ManheimMakeId;
            var matchingModels =
                _context.ManheimMakeModels.Where(
                    i => i.ManheimModel.Name.ToLower().Contains(model.ToLower()) && i.ManheimYearMake.MakeId == makeId).
                    Select(i => i.ManheimModel.ServiceId).ToArray();
            return matchingModels;
        }

        public int MatchingTrim(string trim, int modelServiceId)
        {
            var modelId = GetModelByServiceId(modelServiceId).ManheimModelId;
            var matchingTrim = _context.ManheimModelTrims.FirstOrDefault(i => i.ManheimTrim.Name.ToLower().Contains(trim.ToLower()) && i.ManheimMakeModel.ModelId == modelId);
            return matchingTrim == null ? 0 : matchingTrim.ManheimTrim.ServiceId;
        }

        public List<ManheimTrim> MatchingTrimsByModelId(int modelServiceId)
        {
            var modelId = GetModelByServiceId(modelServiceId).ManheimModelId;
            var matchingTrims = _context.ManheimModelTrims.Where(i => i.ManheimMakeModel.ModelId == modelId).OrderByDescending(i => i.ManheimModelTrimId).Skip(0).Take(5).Select(i => i.ManheimTrim).Distinct();
            return matchingTrims.ToList();
        }

        private ManheimMake GetMakeByServiceId(int id)
        {
            return _context.ManheimMakes.FirstOrDefault(m => m.ServiceId == id);
        }

        private ManheimMake GetMakeById(int id)
        {
            return _context.ManheimMakes.FirstOrDefault(m => m.ManheimMakeId == id);
        }

        private List<ManheimMake> GetAllMakes()
        {
            return _context.ManheimMakes.ToList();
        }

        private ManheimModel GetModelByServiceId(int id)
        {
            return _context.ManheimModels.FirstOrDefault(m => m.ServiceId == id);
        }

        #endregion
    }
}
