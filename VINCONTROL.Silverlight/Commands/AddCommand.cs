using System;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using VINCONTROL.Silverlight.ViewModels;
using System.Windows.Media.Imaging;
using VINCONTROL.Silverlight.Helpers;

namespace VINCONTROL.Silverlight.Commands
{
    public class AddCommand : ICommand
    {
        private const long Maxfilesize = 1024 * 1024;
        private const long Maxnumberoffile = 75;
        private readonly UploadViewModel _vm;

        public AddCommand(UploadViewModel vm)
        {
            _vm = vm;
        }

        public void Execute(object parameter)
        {
            _vm.IsBusy = true;
            var dlg = new OpenFileDialog {Filter = "All Files (*.*)|*.*", FilterIndex = 1, Multiselect = true};
            bool? result = dlg.ShowDialog();

            if (result.HasValue && result.Value)
            {
                ValidateFiles(dlg);
                if (AddCommandComplete != null)
                    AddCommandComplete.Invoke(this, new EventArgs());

                _vm.StartAll.Execute(null);
            }

            _vm.IsBusy = false;

        }

        private void ValidateFiles(OpenFileDialog dlg)
        {
            bool exist = false;
            int totalNumberOfFile = _vm.Files.Count + dlg.Files.Count();
            if (totalNumberOfFile > Maxnumberoffile)
            {
                ErrorHandler.ShowWarning((Maxnumberoffile - _vm.Files.Count) > 0
                                             ? string.Format("You can only upload {0} images more.",
                                                             (Maxnumberoffile - _vm.Files.Count).ToString(CultureInfo.InvariantCulture))
                                             : "The image library is full.");
                return;
            }

            foreach (var file in dlg.Files)
            {
                if (file.Length > Maxfilesize)
                {
                    if (!exist)
                    {
                        ErrorHandler.ShowWarning("There are at least one file exceed 1 MB.");
                        exist = true;
                    }
                }
                else
                {
                    _vm.TotalNumberOfFiles += 1;
                    var image = new BitmapImage();
                    image.SetSource(file.OpenRead());
                    var fileViewModel = new FileViewModel(_vm, file.Length, file.Name, image, file) {IsFinish = false};
                    _vm.Files.Add(fileViewModel);
                }                                         
            }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
        public event EventHandler AddCommandComplete;
    }
}
