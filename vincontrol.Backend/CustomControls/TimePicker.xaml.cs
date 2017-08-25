using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using vincontrol.Backend.Helper;
using VIBlend.WPF.Controls;


namespace vincontrol.Backend.CustomControls
{
    /// <summary>
    /// Interaction logic for TimePicker.xaml
    /// </summary>
// ReSharper disable RedundantExtendsListEntry
    public partial class TimePicker : UserControl
// ReSharper restore RedundantExtendsListEntry
    {
        public TimePicker()
        {
            InitializeComponent();
        }

        public DateTime SelectedItem
        {
            get { return (DateTime)GetValue(SelectedItemProperty); }
            set
            {
                SetValue(SelectedItemProperty, value);
            }
        }

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(DateTime), typeof(TimePicker), new UIPropertyMetadata(DateTime.MinValue,SelectedItemChanged));

        private static void SelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (TimePicker)d;
            var dt = (DateTime)e.NewValue;

            if (control.dateTimeEditor.Value.Equals(dt))
                return;
            
            control.dateTimeEditor.Value = dt.Equals(dt.Midnight()) ? new DateTime(dt.Year, dt.Month, dt.Day, control.dateTimeEditor.Value.Hour, control.dateTimeEditor.Value.Minute, control.dateTimeEditor.Value.Second) : dt;

        }

        private void DateTimeEditor_ValueChanged(object sender, EventArgs e)
        {
            var editor = sender as DateTimeEditor;
            if (editor != null && !SelectedItem.Equals(editor.Value))
               SelectedItem = SelectedItem.SetTime(editor.Value.Hour, editor.Value.Minute, editor.Value.Second);
        }

        public ICommand SetCurrentTimeCommand { get { return new RelayCommand(param => 
        {
            
                DateTime x = DateTime.Now;
                SelectedItem = SelectedItem.SetTime(x.Hour, x.Minute, x.Second);
            

        });} }

        public ICommand UpCommand
        {
            get
            {
                return new RelayCommand(param =>
                {
                    if (!(SelectedItem < DateTime.MaxValue && SelectedItem > DateTime.MinValue))
                    {
                        SelectedItem = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, SelectedItem.Hour, SelectedItem.Minute, SelectedItem.Second);
                    }

                   
                        if(dateTimeEditor.SelectionLength > 0)
                            dateTimeEditor.PerformSpin(true);
                        else
                            dateTimeEditor.Value = SelectedItem.AddHours(1);
                   

                    
                        
                });
            }
        }

        public ICommand DownCommand
        {
            get
            {
                return new RelayCommand(param =>
                {
                    if (!(SelectedItem < DateTime.MaxValue && SelectedItem > DateTime.MinValue))
                    {
                        SelectedItem = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, SelectedItem.Hour, SelectedItem.Minute, SelectedItem.Second);
                    }

                    
                        if (dateTimeEditor.SelectionLength > 0)
                            dateTimeEditor.PerformSpin(false);
                        else
                            dateTimeEditor.Value = SelectedItem.AddHours(-1);
                   
                    
                });
            }
        }
        

      
    }
}
