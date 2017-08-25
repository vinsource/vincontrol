using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;

namespace vincontrol.Data.Repository.Interface
{
    public interface IManheimRepository
    {
        int MatchingMake(string make);
        int MatchingModel(string model, int makeServiceId);
        int[] MatchingModels(string model, int makeServiceId);
        int MatchingTrim(string trim, int modelServiceId);
        List<ManheimTrim> MatchingTrimsByModelId(int modelServiceId);
    }
}
