using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using vincontrol.Backend.Models;
using vincontrol.Backend.ViewModels;
using vincontrol.Backend.ViewModels.Export;
using vincontrol.Backend.ViewModels.Import;
using vincontrol.DataFeed.Model;

namespace vincontrol.Backend.Helper
{
    public static class MappingHelper
    {
        public static MappingViewModel GetMappingViewModel(IncomingProfileTemplateViewModel vm)
        {
            return new MappingViewModel
                             {
                                 HasHeader = vm.HasHeader,
                                 Mappings =
                                     GetMappingList(
                                         vm.Mappings.Where(
                                             i => !string.IsNullOrEmpty(i.DBField) || !string.IsNullOrEmpty(i.XMLField))),
                                 ExportFileType = vm.ExportFileType,
                                 Delimeter = vm.Delimiter
                             };
        }

        public static MappingViewModel GetMappingViewModel(ExportProfileTemplateViewModel vm)
        {
            return new MappingViewModel
                             {
                                 HasHeader = vm.HasHeader,
                                 Mappings =
                                     GetMappingList(
                                         vm.Mappings.Where(
                                             i => !string.IsNullOrEmpty(i.DBField) || !string.IsNullOrEmpty(i.XMLField))),
                                 ExportFileType = vm.ExportFileType,
                                 Delimeter = vm.Delimiter,
                                 InventoryStatus = vm.InventoryStatus,
                                 SplitImage = vm.SplitImage

                             };
        }

        private static IList<Mapping> GetMappingList(IEnumerable<ProfileMappingViewModel> mappings)
        {
            return mappings.Select(item => new Mapping
            {
                Conditions = item.Conditions == null ? null : item.Conditions.Conditions.Select(i => new Condition { ComparedValue = i.ComparedValue, DBField = i.DBField, Operator = i.Operator, TargetValue = i.TargetValue, Type = i.Type, XMLField = i.XMLField }).ToList(),
                DBField = item.DBField,
                Expression = item.Expression,
                Order = item.Order
                ,
                Replaces = item.Replaces == null ? null : item.Replaces.Replaces.Where(i => !String.IsNullOrEmpty(i.From) && !String.IsNullOrEmpty(i.To)).Select(i => new Replacement { From = i.From, To = i.To }).ToList(),
                XMLField = item.XMLField
            }).ToList();
        }

        //private static IList<Mapping> GetMappingList(IEnumerable<ExportProfileMappingViewModel> mappings)
        //{
        //    return mappings.Select(item => new Mapping
        //    {
        //        Conditions = item.Conditions == null ? null : item.Conditions.Conditions.Select(i => new Condition { ComparedValue = i.ComparedValue, DBField = i.DBField, Operator = i.Operator, TargetValue = i.TargetValue, Type = i.Type, XMLField = i.XMLField }).ToList(),
        //        DBField = item.DBField,
        //        //Expression = item.Expression,
        //        Order = item.Order,
        //        Replaces = item.Replaces == null ? null : item.Replaces.Replaces.Where(i => !String.IsNullOrEmpty(i.From) && !String.IsNullOrEmpty(i.To)).Select(i => new Replacement { From = i.From, To = i.To }).ToList(),
        //        XMLField = item.XMLField
        //    }).ToList();
        //}

        public static ConditionListViewModel GetConditionsList(Mapping i)
        {
            if (i.Conditions == null)
                return null;
            var listViewModel = new ConditionListViewModel
            {
                Conditions = new ObservableCollection<ConditionModel>(
                    i.Conditions.Select(
                        ci => new ConditionModel
                        {
                            ComparedValue = ci.ComparedValue,
                            DBField = ci.DBField,
                            Operator = ci.Operator,
                            TargetValue = ci.TargetValue,
                            Type = ci.Type,
                        }))
            };
            return listViewModel;
        }

        public static ReplaceListViewModel GetReplacesList(Mapping i)
        {
            if (i.Replaces == null)
                return null;
            var listViewModel = new ReplaceListViewModel
            {
                Replaces =
                    new ObservableCollection<ReplaceModel>(
                    i.Replaces.Select(ci => new ReplaceModel { From = ci.From, To = ci.To }))
            };
            return listViewModel;
        }

    }
}