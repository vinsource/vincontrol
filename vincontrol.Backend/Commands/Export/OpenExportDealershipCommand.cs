using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Backend.Commands.Import;
using vincontrol.Backend.ViewModels;
using vincontrol.Backend.Windows;
using vincontrol.Backend.Windows.DataFeed.Export;

namespace vincontrol.Backend.Commands.Export
{
    public class OpenExportDealershipCommand : CommandBase
    {
        private ExportProfileViewModel _vm;
        public OpenExportDealershipCommand(ExportProfileViewModel vm)
            : base(null)
        {
            _vm = vm;
        }

        //private int _dealerId;
        //public OpenProfileSettingCommand(int dealerId)
        //{
        //    _dealerId = dealerId;
        //}

        #region Implementation of ICommand

        public override void Do(object parameter)
        {
            var chldWindow = new ViewExportDealerOfProfileWindow(_vm);
            chldWindow.ShowDialog();
        }

        #endregion
    }
}
