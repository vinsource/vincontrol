using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using VINCapture.UploadImage.Commands;
using VINCapture.UploadImage.Interface;
using VINCapture.UploadImage.Models;
using VINCapture.UploadImage.View;
using vincontrol.Data.Model;

namespace VINCapture.UploadImage.ViewModels
{
    public class DetailPageViewModel : INotifyPropertyChanged
    {
        private string[] _imageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        public bool IsOverlay { get; set; }
        public string Path { get; set; }
        public bool IsPortableDevice { get; set; }
        public string DealerFolder { get; set; }
        public bool IsSendEmail { get; set; }

        public bool IsManager
        {
            get
            {
                return ((App) Application.Current).Dealer.Role.Equals(RoleType.Manager) ||
                       ((App)Application.Current).Dealer.Role.Equals(RoleType.Admin) ||
                       ((App)Application.Current).Dealer.Role.Equals(RoleType.Master);
            }
        }
        

        private SelectEmailCommand _selectEmail;
        public SelectEmailCommand SelectEmail
        {
            get { return _selectEmail ?? (_selectEmail = new SelectEmailCommand(this)); }
        }

        private SyncCommand _sync;
        public SyncCommand Sync
        {
            get { return _sync ?? (_sync = new SyncCommand(this)); }
        }

        private ShowFolderCommand _showFolder;
        public ShowFolderCommand ShowFolder
        {
            get { return _showFolder ?? (_showFolder = new ShowFolderCommand(this)); }
        }

        public bool IsEnableSync
        {
            get { return _isEnableSync; }
            set
            {
                if (_isEnableSync != value)
                {
                    _isEnableSync = value;
                    OnPropertyChanged("IsEnableSync");
                }
            }
        }

        private bool _isEnableSync = true; 

        public string ResultMessage
        {
            get { return _resultMessage; }
            set
            {
                if (_resultMessage != value)
                {
                    _resultMessage = value;
                    OnPropertyChanged("ResultMessage");
                }
            }
        }

        private string _resultMessage;


        public bool DoBackup { get; set; }
        public string BackupPath
        {
            get { return _backupPath; }
            set
            {
                if (_backupPath != value)
                {
                    _backupPath = value;
                    OnPropertyChanged("BackupPath");
                }
            }
        }

        private string _backupPath;

        public List<USBCarViewModel> Vehicles { get; set; }

        public List<EmailItem> EmailList { get; set; }
   
        public DetailPageViewModel(IView view)
        {
            if (!String.IsNullOrEmpty(view.Path))
            {
                Path = view.Path;
                InitializerUsb();
                DoBackup = true;
                IsPortableDevice = false;
                view.SetDataContext(this);
            }
            else
            {
                InitializerPortable();
                DoBackup = true;
                IsPortableDevice = true;
                view.SetDataContext(this);
            }
        }

        private void InitializerUsb()
        {
           
            BackupPath = @"C:\\VincaptureImageBackup\" +  ((App)Application.Current).Dealer.DealerId.ToString(CultureInfo.InvariantCulture);
            if(DoBackup)
            {
                Directory.CreateDirectory(BackupPath);
            }
            var folder = new DirectoryInfo(Path);
            var vinCaptureFolder = folder.GetDirectories().FirstOrDefault(i => i.Name.Equals("VinCapture"));
            if (vinCaptureFolder != null)
            {
                var dealerFolder =
                    vinCaptureFolder.GetDirectories().FirstOrDefault(
                        i => i.Name.Equals(((App)Application.Current).Dealer.DealerId.ToString(CultureInfo.InvariantCulture)));

                

                if (dealerFolder == null) return;
                else
                {
                    DealerFolder = dealerFolder.FullName;
                }

                int result;
                var tmpVehicles =
                    dealerFolder.GetDirectories().Select(
                        i =>
                        new USBCarViewModel
                            {
                                Name = i.Name,
                                Quantity = i.GetFiles().Count(ii => _imageExtensions.Contains(ii.Extension.ToLower())),
                                PhysicalFolderPath = i.FullName,
                                DestinationBackupFolder = _backupPath + "\\"+i.Name,
                                IsOverlay = IsOverlay,
                                Progress = 0,
                                ListingId = int.TryParse(i.Name, out result) ? result : 0,
                                Vin = String.Empty
                            }).ToList();

                var inventoryList = GetInventoryList(tmpVehicles);
                Vehicles=new List<USBCarViewModel>();
                foreach (var vehicle in tmpVehicles)
                {
                    var firstOrDefault = inventoryList.FirstOrDefault(i => i.InventoryId == vehicle.ListingId);
                    if (firstOrDefault != null)
                    {
                        vehicle.Vin = firstOrDefault.Vehicle.Vin;
                        vehicle.Name = String.Format("{0} {1} {2}", firstOrDefault.Vehicle.Year, firstOrDefault.Vehicle.Make,
                                                     firstOrDefault.Vehicle.Model);
                        vehicle.Year = firstOrDefault.Vehicle.Year.ToString();
                        vehicle.Make = firstOrDefault.Vehicle.Make;
                        vehicle.Model = firstOrDefault.Vehicle.Model;
                        vehicle.Stock = firstOrDefault.Stock;
                        vehicle.Trim = firstOrDefault.Vehicle.Trim;
                        vehicle.Color = firstOrDefault.ExteriorColor;
                        vehicle.DestinationBackupFolder = _backupPath + "\\" + firstOrDefault.Vehicle.Vin;

                        Vehicles.Add(vehicle);
                    }
                }
            }
        }

        private void InitializerPortable()
        {
            BackupPath = @"C:\\VincaptureImageBackup\" + ((App)Application.Current).Dealer.DealerId.ToString(CultureInfo.InvariantCulture);
            const string vincapturePath = @"C:\\VincaptureTemporary";

            var folder = new DirectoryInfo(vincapturePath);
            
            var vinCaptureFolder = folder.GetDirectories().FirstOrDefault(i => i.Name.Equals("VinCapture"));
            
            if (vinCaptureFolder != null)
            {
                var dealerFolder =
                    vinCaptureFolder.GetDirectories().FirstOrDefault(
                        i => i.Name.Equals(((App)Application.Current).Dealer.DealerId.ToString(CultureInfo.InvariantCulture)));

                if (dealerFolder == null) return;

                int result;
                var tmpVehicles =
                    dealerFolder.GetDirectories().Select(
                        i =>
                        new USBCarViewModel
                        {
                            Name = i.Name,
                            Quantity = i.GetFiles().Count(),
                            PhysicalFolderPath = i.FullName,
                            DestinationBackupFolder = _backupPath + "\\" + i.Name,
                            IsOverlay = IsOverlay,
                            Progress = 0,
                            ListingId = int.TryParse(i.Name, out result) ? result : 0,
                            Vin = String.Empty
                        }).ToList();

                var inventoryList = GetInventoryList(tmpVehicles);
                Vehicles = new List<USBCarViewModel>();
                foreach (var vehicle in tmpVehicles)
                {
                    var firstOrDefault = inventoryList.FirstOrDefault(i => i.InventoryId == vehicle.ListingId);
                    if (firstOrDefault != null)
                    {
                        vehicle.Vin = firstOrDefault.Vehicle.Vin;
                        vehicle.Name = String.Format("{0} {1} {2}", firstOrDefault.Vehicle.Year, firstOrDefault.Vehicle.Make,
                                                      firstOrDefault.Vehicle.Model);
                        vehicle.Year = firstOrDefault.Vehicle.Year.ToString();
                        vehicle.Make = firstOrDefault.Vehicle.Make;
                        vehicle.Model = firstOrDefault.Vehicle.Model;
                        vehicle.Stock = firstOrDefault.Stock;
                        vehicle.Trim = firstOrDefault.Vehicle.Trim;
                        vehicle.Color = firstOrDefault.ExteriorColor;
                        vehicle.DestinationBackupFolder = _backupPath + "\\" + firstOrDefault.Vehicle.Vin;

                        Vehicles.Add(vehicle);
                    }
                }
            }
        }

        private List<Inventory> GetInventoryList(IEnumerable<USBCarViewModel> vehicles)
        {
            var context = new VincontrolEntities();

            return context.Inventories.Where(BuildContainsExpression
                                                                     <Inventory, int>(
                                                                         e => e.InventoryId,
                                                                         vehicles.Select(i => i.ListingId))).ToList();
        }

        public static Expression<Func<TElement, bool>> BuildContainsExpression<TElement, TValue>(Expression<Func<TElement, TValue>> valueSelector, IEnumerable<TValue> values)
        {
            if (null == valueSelector) { throw new ArgumentNullException("valueSelector"); }
            if (null == values) { throw new ArgumentNullException("values"); }
            ParameterExpression p = valueSelector.Parameters.Single();
            // p => valueSelector(p) == values[0] || valueSelector(p) == ...
            if (!values.Any())
            {
                return e => false;
            }
            var equals = values.Select(value => (System.Linq.Expressions.Expression)System.Linq.Expressions.Expression.Equal(valueSelector.Body, System.Linq.Expressions.Expression.Constant(value, typeof(TValue))));
            var body = equals.Aggregate(System.Linq.Expressions.Expression.Or);
            return System.Linq.Expressions.Expression.Lambda<Func<TElement, bool>>(body, p);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}