using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Vincontrol.Vinsell.WindesktopVersion.Helper;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Helper;

namespace Vincontrol.Vinsell.WindesktopVersion
{
    public partial class KbbForm : Form
    {
        private readonly string _vin;
        private readonly string _trimId;
        private readonly int _mileage;
        private List<KeyValuePair<string, string>> _options;
        private KarPowerViewModel _karPowerViewModel;

        public KbbForm(string vin,int mileage, string trimId)
        {
            _vin = vin;
            _trimId = trimId;
            _mileage = mileage;
            
            InitializeComponent();
            basePanel.Paint += OnPaint;
            adjustPanel.Paint += OnPaint;
            afterPanel.Paint += OnPaint;
            Load += KbbForm_Load;
        }

        void KbbForm_Load(object sender, EventArgs e)
        {
            ShowLoading();
            var worker = new BackgroundWorker();
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_karPowerViewModel != null)
            {
                BindForm(_karPowerViewModel);
            }

            HideLoading();
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            //_karPowerViewModel = KarpowerHelper.CreateViewModelForKarPowerResult(SessionVar.CurrentDealer.DealerSetting.KBBUserName, SessionVar.CurrentDealer.DealerSetting.KBBPassword, _vin, _mileage.ToString(CultureInfo.InvariantCulture), _trimId, "Inventory", SessionVar.CurrentDealer);
        }

        private void BindForm(KarPowerViewModel karpower)
        {
            lblBaseWholeSale.Text = karpower.BaseWholesale;
            lblMileageAdjustment.Text = karpower.MileageAdjustment;
            lblTotal.Text = karpower.Wholesale;
            txtMiles.Text = _mileage.ToString(CultureInfo.InvariantCulture);
            _options = karpower.OptionalEquipmentMarkupList.Where(i => !String.IsNullOrEmpty(i.DisplayName)).Select(option => new KeyValuePair<string, string>(option.Id, option.DisplayName)).ToList();

            chlbxOptions.ItemCheck += chlbxOptions_ItemCheck;
           
            chlbxOptions.DataSource = new BindingSource(_options, null);
            chlbxOptions.DisplayMember = "Value";
            chlbxOptions.ValueMember = "Key";

            cbbDriveTrain.DataSource = new BindingSource(karpower.DriveTrains, null);
            cbbDriveTrain.DisplayMember = "Text";
            cbbDriveTrain.ValueMember = "Value";

            cbbTransmission.DataSource = new BindingSource(karpower.Transmissions, null);
            cbbTransmission.DisplayMember = "Text";
            cbbTransmission.ValueMember = "Value";

            cbbReportType.DataSource = new BindingSource(karpower.Reports,null);
            cbbReportType.DisplayMember = "Text";
            cbbReportType.ValueMember = "Value";
        }

        void chlbxOptions_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            //var model = KarpowerHelper.CreateViewModelForUpdateValuationByOptionalEquipment(chlbxOptions.SelectedValue.ToString(), e.NewValue == CheckState.Checked, _karPowerViewModel, SessionVar.CurrentDealer.KbbCookieContainer, SessionVar.CurrentDealer.KbbCookieCollection);
            //lblBaseWholeSale.Text = model.BaseWholesale;
            //lblMileageAdjustment.Text = model.MileageAdjustment;
            //lblTotal.Text = model.Wholesale;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            base.OnPaint(e);
            Rectangle r = ClientRectangle;
            r.Width -= 1;
            r.Height -= 1;
            e.Graphics.DrawRectangle(Pens.DeepSkyBlue, r);
        }

        private void pcbGetValue_Click(object sender, EventArgs e)
        {
            ShowLoading();
            int tranmission;
            int trimId;
            int.TryParse(cbbTransmission.SelectedValue.ToString(), out tranmission);
            int.TryParse(_trimId, out trimId);
            if (tranmission != 0 && trimId != 0)
            {
                //var model = KarpowerHelper.CreateViewModelForKarPowerResult(SessionVar.CurrentDealer.DealerSetting.KBBUserName, SessionVar.CurrentDealer.DealerSetting.KBBPassword, _vin, txtMiles.Text, _trimId, "Inventory", SessionVar.CurrentDealer);

                //_karPowerViewModel = KarpowerHelper.CreateViewModelForUpdateValuationByChangingTrim(trimId, tranmission,
                //                                                                           SessionVar.CurrentDealer
                //                                                                                     .KbbCookieContainer,
                //                                                                           SessionVar.CurrentDealer
                //                                                                                     .KbbCookieCollection, model);
                lblBaseWholeSale.Text = _karPowerViewModel.BaseWholesale;
                lblMileageAdjustment.Text = _karPowerViewModel.MileageAdjustment;
                lblTotal.Text = _karPowerViewModel.Wholesale;
            }
            HideLoading();
        }

        private void pcbPrint_Click(object sender, EventArgs e)
        {
            int transmissionId;
            int driveTrainId;
            if (int.TryParse(cbbTransmission.SelectedValue.ToString(), out transmissionId))
            {
                _karPowerViewModel.SelectedTransmissionId = transmissionId;
            }

            if (int.TryParse(cbbDriveTrain.SelectedValue.ToString(), out driveTrainId))
            {
                _karPowerViewModel.SelectedDriveTrainId = driveTrainId;
            }
            _karPowerViewModel.SelectedReport = cbbReportType.SelectedValue.ToString();

            var dlg = new SaveFileDialog {Filter = "pdf files (*.pdf)|*.pdf", Title = "PDF", CheckPathExists = true};
            
            if (dlg.ShowDialog() == DialogResult.OK
            && dlg.FileName.Length > 0)
            {
                using (var fileStream = (FileStream)dlg.OpenFile())
                {

                    //var content = KarpowerHelper.PrintReport(_karPowerViewModel, SessionVar.CurrentDealer.DealerSetting.KBBUserName,
                    //                             SessionVar.CurrentDealer.DealerSetting.KBBPassword,
                    //                             SessionVar.CurrentDealer.KbbCookieContainer,
                    //                             SessionVar.CurrentDealer.KbbCookieCollection, SessionVar.CurrentDealer.DealerId);
                    //if (content != null)
                    //{
                    //    content.CopyTo(fileStream);
                    //    content.Close();
                    //}
                }
            }
        }

        private void ShowLoading()
        {
            pbxLoading.Visible = true;
            pnlKBB.Enabled = false;
        }

        private void HideLoading()
        {
            pbxLoading.Visible = false;
            pnlKBB.Enabled = true;
        }
    }

    public class SelectItem
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }



}
