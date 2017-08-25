using System.Collections.ObjectModel;
using System.Linq;
using vincontrol.Backend.Commands.Import;
using vincontrol.Backend.ViewModels;

namespace vincontrol.Backend.Commands.Export
{
    public class OrderChangedCommand : CommandBase
    {
        private readonly ExportProfileTemplateViewModel _vm;
        public OrderChangedCommand(ExportProfileTemplateViewModel vm)
            : base(null)
        {
            _vm = vm;
        }

        #region Overrides of CommandBase

        public override bool CanExecute(object parameter)
        {
            if (parameter.GetType() != typeof (ProfileMappingViewModel)) return false;
            return ((ProfileMappingViewModel)parameter).RaisedOrderEvent;
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public override void Do(object parameter)
        {
            var currentItem = (ProfileMappingViewModel)parameter;

            if (_vm.Mappings.FirstOrDefault(i => i != parameter && i.Order == currentItem.Order) == null) return;

            if (currentItem.IsIncrease)
            {
                if (currentItem.PreviousOrder != 0)
                {
                    foreach (var mapping in GetRange(parameter, currentItem.PreviousOrder, currentItem.Order))
                    {
                        DecreaseItem(mapping);
                    }
                }
                else
                {
                    var item = GetComparedDirection(parameter, currentItem.Order, _vm.Mappings);
                  
                    if (item.CompareDirection == CompareDirection.Right)
                    {
                        foreach (var mapping in GetRange(parameter, currentItem.Order, item.Value))
                        {
                            IncreaseItem(mapping);
                        }
                    }
                    else
                    {
                        foreach (var mapping in GetRange(parameter, item.Value, currentItem.Order))
                        {
                            DecreaseItem(mapping);
                        }
                    }
                }
            }
            else
            {
                foreach (var mapping in GetRange(parameter, currentItem.Order, currentItem.PreviousOrder))
                {
                    IncreaseItem(mapping);
                }
            }
        }

        private System.Collections.Generic.IEnumerable<ProfileMappingViewModel> GetRange(object currentItem, int start, int end)
        {
            return _vm.Mappings.Where(i => i != currentItem && i.Order >= start && i.Order <= end);
        }

        private static void DecreaseItem(ProfileMappingViewModel mapping)
        {
            mapping.RaisedOrderEvent = false;
            mapping.Order--;
            mapping.RaisedOrderEvent = true;
        }

        private static void IncreaseItem(ProfileMappingViewModel mapping)
        {
            mapping.RaisedOrderEvent = false;
            mapping.Order++;
            mapping.RaisedOrderEvent = true;
        }

        private SpaceItem GetComparedDirection(object parameter, int order, ObservableCollection<ProfileMappingViewModel> mappings)
        {
            var result = mappings.Where(i =>i!=parameter && i.Order <= order && i.Order>0).OrderByDescending(i => i.Order).ToList();
          
            for (var i = 0; i < result.Count; i++)
            {
                if (i != result.Count - 1 && result[i].Order - 1 != result[i + 1].Order)
                {
                    return new SpaceItem { CompareDirection = CompareDirection.Left, Value = result[i].Order - 1 };
                }

                if(i==result.Count-1 && result[i].Order>1)
                {
                    return new SpaceItem { CompareDirection = CompareDirection.Left, Value = 1 };
                }

            }

            result = mappings.Where(i => i != parameter && i.Order >= order && i.Order > 0).OrderBy(i => i.Order).ToList();

            for (int i = 0; i < result.Count; i++)
            {
                ProfileMappingViewModel t = result[i];
                if (i != result.Count - 1 && t.Order + 1 != result[i + 1].Order)
                {
                    return new SpaceItem {CompareDirection = CompareDirection.Right, Value = t.Order + 1};
                }

                if(i==result.Count-1)
                {
                    return new SpaceItem { CompareDirection = CompareDirection.Right, Value = result[i].Order + 1 };
                }
            }

          
            return null;
        }

        #endregion
    }

    public class SpaceItem
    {
        public CompareDirection CompareDirection { get; set; }
        public int Value { get; set; }
    }

    public enum CompareDirection
    {
        Left, Right
    }
}
