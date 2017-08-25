using System;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace vincontrol.Backend.CustomControls
{
    /// <summary>
    /// Interaction logic for NumericControl.xaml
    /// </summary>
// ReSharper disable RedundantExtendsListEntry
    public partial class NumericControl : UserControl
// ReSharper restore RedundantExtendsListEntry
    {
        /// <summary>
        /// Support binding on property IsReadOnly.
        /// </summary>
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(NumericControl),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender,
// ReSharper disable RedundantDelegateCreation
                new PropertyChangedCallback(OnIsReadOnlyChanged)));
// ReSharper restore RedundantDelegateCreation
 
        /// <summary>
        /// Support binding on property Value.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(decimal), typeof(NumericControl),
            new FrameworkPropertyMetadata(0M, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender,
// ReSharper disable RedundantDelegateCreation
                new PropertyChangedCallback(OnValueChanged), new CoerceValueCallback(OnCoerceValue)));
// ReSharper restore RedundantDelegateCreation
 
        private static readonly DoubleAnimation ErrorAnimation = new DoubleAnimation(0d, 1d, new Duration(TimeSpan.FromSeconds(0.5d)));
 
        /// <summary>
        /// Locker object for increment/decrement + repeater property.
        /// </summary>
        private readonly object _locker = new object();
 
        /// <summary>
        /// doCoerce = a value indicating whether to do coercing during callback; doGotFocus = a value indicating whether to do GotFocus.
        /// </summary>
        private bool _doCoerce = true, _doGotFocus = true;
 
        /// <summary>
        /// Number of decimal places to show.
        /// </summary>
        private int _decimalPlaces;
 
        /// <summary>
        /// The format used to display the decimal value.
        /// </summary>
        private string _decimalFormat = "0";
 
        /// <summary>
        /// Minimum, Maximum and Increment property values.
        /// </summary>
        private decimal _min, _max = 100M, _inc = 1M;
 
        static NumericControl()
        {
            ErrorAnimation.AutoReverse = true;
            if (ErrorAnimation.CanFreeze && !ErrorAnimation.IsFrozen) ErrorAnimation.Freeze();
        }
 
        /// <summary>
        /// Initializes a new instance of the NumericUpDown class.
        /// </summary>
        public NumericControl()
        {
            InitializeComponent();
        }
 
        /// <summary>
        /// Value changed event handler.
        /// </summary>
        public event EventHandler ValueChanged;
 
        /// <summary>
        /// Gets a value indicating whether the user control is focused.
        /// </summary>
        public bool IsUserControlFocused
        {
            get { return textBoxValue.IsFocused; }
        }
 
        /// <summary>
        /// Gets or sets the value of decimal places to show.
        /// </summary>
        /// <remarks>If the Increment property value has too many decimal places then it will be upscaled to match the new decimal places. Value will be automatically constrained to 0-28.</remarks>
        public int DecimalPlaces
        {
            get
            {
                return _decimalPlaces;
            }
 
            set
            {
                _decimalPlaces = Math.Max(0, Math.Min(28, value));
 
                var format = new StringBuilder("0");
 
                if (_decimalPlaces > 0)
                {
                    format.Append(".");
                    for (int i = 0; i < _decimalPlaces; i++)
                    {
                        format.Append("0");
                    }
                }
 
                _decimalFormat = format.ToString();
 
                string incr = _inc.ToString(CultureInfo.InvariantCulture);
                int decimalIndex = incr.IndexOf(".", StringComparison.Ordinal);
 
                if (incr.Contains(".") && incr.Substring(decimalIndex + 1).Length > _decimalPlaces) UpscaleIncrement(incr, decimalIndex);
 
                // double rounding occurs, but that's no big deal. Can be fixed with this.Value = this.Value, but that looks wiered...
                if (Value.ToString(CultureInfo.InvariantCulture).Contains(".")) Value = Math.Round(Value, _decimalPlaces, MidpointRounding.AwayFromZero);
 
                UpdateTextBox();
 
                // set min and max with decimal places, they will also do SetTextBoxMaxLength
                Minimum = _min;
                Maximum = _max;
            }
        }
 
        /// <summary>
        /// Gets or sets the incrementing value.
        /// </summary>
        /// <remarks>If the supplied value has too many decimal places then an upscaled value will be used instead (eg: DecimalPlaces = 2; Supplied value = 0.0005; Upscaled value = 0.01).
        /// Value will be automatically constrained to > 0.</remarks>
        public decimal Increment
        {
            get
            {
                return _inc;
            }
 
            set
            {
                if (value <= 0) _inc = 1M;
                else
                {
                    string val = value.ToString(CultureInfo.InvariantCulture);
 
                    int decimalIndex = val.IndexOf(".", StringComparison.Ordinal);
 
                    if (val.Contains(".") && val.Substring(decimalIndex + 1).Length > _decimalPlaces) UpscaleIncrement(val, decimalIndex);
                    else _inc = value;
                }
            }
        }
 
        /// <summary>
        /// Gets or sets a value indicating whether the control is readonly
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return (bool)GetValue(IsReadOnlyProperty);
            }
 
            set
            {
                SetCurrentValue(IsReadOnlyProperty, value);
            }
        }
 
        /// <summary>
        /// Gets or sets the minimum selectable value.
        /// </summary>
        /// <remarks>If provided value bigger than Maximum then Maximum is set to Minimum.</remarks>
        public decimal Minimum
        {
            get
            {
                return _min;
            }
 
            set
            {
                _min = Math.Round(value, DecimalPlaces, MidpointRounding.AwayFromZero);
 
                if (_min > _max) Maximum = _min; // Maximum does SetTextBoxMaxLength
                else SetTextBoxMaxLength();
 
                if (Value < _min) Value = _min;
            }
        }
 
        /// <summary>
        /// Gets or sets the maximum selectable value.
        /// </summary>
        /// <remarks>If provided value smaller than Minimum then Maximum is set to Minimum.</remarks>
        public decimal Maximum
        {
            get
            {
                return _max;
            }
 
            set
            {
                _max = Math.Max(_min, Math.Round(value, DecimalPlaces, MidpointRounding.AwayFromZero)); // Math.Max => max not smaller than min
 
                if (Value > _max) Value = _max;
 
                SetTextBoxMaxLength();
            }
        }
 
        /// <summary>
        /// Gets or sets the textBoxValue TextAlignment
        /// </summary>
        public TextAlignment TextAlignment
        {
            get
            {
                return textBoxValue.TextAlignment;
            }
 
            set
            {
                textBoxValue.TextAlignment = value;
            }
        }
 
        /// <summary>
        /// Gets or sets a value indicating whether to show a context menu when right clicking on the textbox.
        /// </summary>
        public bool ShowContextMenu
        {
            get
            {
                return ContextMenuService.GetIsEnabled(textBoxValue);
            }
 
            set
            {
                ContextMenuService.SetIsEnabled(textBoxValue, value);
            }
        }
 
        /// <summary>
        /// Gets or sets the spinner (up/down arrow) width. Should be an uneven number for perfect spacing. (Minimum value = 17d)
        /// </summary>
        //public double SpinnerWidth
        //{
        //    get
        //    {
        //        return this.spinner.Width;
        //    }
 
        //    set
        //    {
        //        this.spinner.Width = Math.Max(17d, value);
        //    }
        //}
 
        /// <summary>
        /// Gets or sets the NumericUpDown value.
        /// </summary>
        public decimal Value
        {
            get
            {
                return (decimal)GetValue(ValueProperty);
            }
 
            set
            {
                // do a manual coerce here, inorder for source to never get an out-of-sync value (otherwise gets bad value, then coerce callback comes and sets it back).
                _doCoerce = false;
                SetCurrentValue(ValueProperty, Coerce(Minimum, Maximum, value, DecimalPlaces));
                _doCoerce = true;
            }
        }
 
        /// <summary>
        /// Selects the entire text in the internal textbox.
        /// </summary>
        public void SelectTextBoxText()
        {
            // this bit required (unlike TimePicker), because textbox is not read only?
            if (IsTabStop == false)
            {
                _doGotFocus = false;
                textBoxValue.Focus();
                _doGotFocus = true;
            }
 
            textBoxValue.Select(0, textBoxValue.Text.Length);
        }
 
        /// <summary>
        /// On IsReadOnly changed event handler.
        /// </summary>
        /// <param name="property">Dependency object.</param>
        /// <param name="args">Arguments supplied.</param>
        private static void OnIsReadOnlyChanged(DependencyObject property, DependencyPropertyChangedEventArgs args)
        {
            var value = (bool)args.NewValue;
            var nupd = (NumericControl)property;
 
            nupd.textBoxValue.IsReadOnly = value;
            nupd.repeatButtonUp.IsEnabled = nupd.repeatButtonDown.IsEnabled = !value;
        }
 
        /// <summary>
        /// On value changed event handler.
        /// </summary>
        /// <param name="property">Dependency object.</param>
        /// <param name="args">Arguments supplied.</param>
        private static void OnValueChanged(DependencyObject property, DependencyPropertyChangedEventArgs args)
        {
            //// in XAML designer, if you remove Value="", then OnValueChanged is called, but no CoerceValue callback, thus there is a tiny mismatch between Min/Max and Value)

            var nupd = (NumericControl)property;
            nupd.UpdateTextBox();
 
            // if someone is listening to the value changed event and value differs then notify them
            if ((decimal)args.OldValue != (decimal)args.NewValue && nupd.ValueChanged != null) nupd.ValueChanged(nupd, new EventArgs());
        }
 
        /// <summary>
        /// Coerces the given value between Minimum and Maximum.
        /// </summary>
        /// <param name="property">Dependency object.</param>
        /// <param name="value">Value to be coerced.</param>
        /// <returns>The coerced value.</returns>
        private static object OnCoerceValue(DependencyObject property, object value)
        {
            var nupd = (NumericControl)property;
            decimal origValue = (decimal)value, newValue = origValue;
 
            // this is only done on initialization (this.Value is bypassed by .NET FX) and when bindings get changed
            if (nupd._doCoerce)
            {
                newValue = Coerce(nupd.Minimum, nupd.Maximum, origValue, nupd.DecimalPlaces);
 
                // CoerceValue callback is called after source has been updated, inorder to get source back in sync, I must manually update the source.
                // The only way to do this is via reflection. Creating another binding will throw a StackOverflowException, using Dispatcher.BeginInvoke will hang Visual Studio designer...
                if (newValue != origValue)
                {
                    try
                    {
                        var be = nupd.GetBindingExpression(ValueProperty);
                        if (be != null && be.DataItem != null && be.ParentBinding != null && be.ParentBinding.Path != null)
                        {
                            object currObj = be.DataItem;
                            Type currType = currObj.GetType();
                            string[] paths = be.ParentBinding.Path.Path.Split(new[] { ".", "[", "]" }, StringSplitOptions.RemoveEmptyEntries);
 
                            for (int i = 0; i < paths.Length; i++)
                            {
                                string currPath = paths[i];
 
                                if (currType.IsArray)
                                {
                                    string[] indices = currPath.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                                    var longIndices = new long[indices.Length]; // I'm using long incase some indexer is of long type (highly unlikely), long is at least compatible with int
 
                                    for (int j = 0; j < indices.Length; j++)
                                    {
                                        longIndices[j] = Convert.ToInt64(indices[j]);
                                    }
 
                                    var arr = (Array)currObj;
                                    currObj = arr.GetValue(longIndices);
                                    currType = currObj.GetType();
 
                                    if (i == paths.Length - 1) arr.SetValue(Convert.ChangeType(newValue, currType, CultureInfo.InvariantCulture), longIndices);
                                }
                                else
                                {
                                    System.Reflection.PropertyInfo currProperty = currType.GetProperty(currPath);
 
                                    // I can't set currObj until I know that the loop continues, I need it for value setting
                                    object newObj = currProperty.GetValue(currObj, null);
                                    currType = newObj.GetType();
 
                                    if (i == paths.Length - 1) currProperty.SetValue(currObj, Convert.ChangeType(newValue, currType, CultureInfo.InvariantCulture), null);
                                    else currObj = newObj;
                                }
                            }
                        }
                    }
                    catch (Exception ex) // possible exceptions include XAML errors (wrong Path) and perhaps custom indexers
                    {
                        System.Diagnostics.Trace.WriteLine(ex.ToString());
                    }
                }
            }
 
            if (nupd.Value == newValue) nupd.UpdateTextBox(); // if values are same, OnValueChanged will not be called.
 
            return newValue;
        }
 
        private static decimal Coerce(decimal min, decimal max, decimal value, int decPlaces)
        {
            return Math.Max(min, Math.Min(max, Math.Round(value, decPlaces, MidpointRounding.AwayFromZero)));
        }
 
        private void ucNUPD_GotFocus(object sender, RoutedEventArgs e)
        {
            // select only if tabbed into or focus set via code. Also ignore gotfocus from repeater button click.
            if (!IsReadOnly && _doGotFocus && !(IsMouseOver && Mouse.LeftButton == MouseButtonState.Pressed)) SelectTextBoxText();
        }
 
        private void repeatButtonUp_Click(object sender, RoutedEventArgs e)
        {
            IncrementValue();
        }
 
        private void repeatButtonDown_Click(object sender, RoutedEventArgs e)
        {
            DecrementValue();
        }
 
        private void repeatButton_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SelectTextBoxText();
        }
 
        private void textBoxValue_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                if (e.Text.Length == 1 && (textBoxValue.Text.Length < textBoxValue.MaxLength || textBoxValue.SelectionLength >= 1))
                {
                    if ((Char.IsDigit(e.Text, 0) ||
                        (e.Text == NumberFormatInfo.CurrentInfo.NumberDecimalSeparator && _decimalPlaces > 0 &&
                        !textBoxValue.Text.Contains(e.Text) && textBoxValue.SelectionStart >= 1) ||
                        (e.Text == NumberFormatInfo.CurrentInfo.NegativeSign && !textBoxValue.Text.Contains(e.Text) && textBoxValue.SelectionStart == 0)) == false)
                    {
                        e.Handled = true; // do not accept input if it has nothing to do with decimals
                    }
                }
                else e.Handled = true;
            }
            catch (FormatException)
            {
                e.Handled = true;
            }
            catch (OverflowException)
            {
                e.Handled = true;
            }
        }
 
        private void textBoxValue_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            try
            {
                var input = (string)e.DataObject.GetData(typeof(string));
                string currentText = (textBoxValue.SelectionLength > 0 ?
                    textBoxValue.Text.Remove(textBoxValue.SelectionStart, textBoxValue.SelectionLength) : textBoxValue.Text);
 
                decimal d = Convert.ToDecimal(currentText.Insert(textBoxValue.SelectionStart, input), CultureInfo.CurrentCulture);
                Value = d;
            }
            catch (FormatException)
            {
                borderError.BeginAnimation(OpacityProperty, ErrorAnimation, HandoffBehavior.SnapshotAndReplace);
            }
            catch (OverflowException)
            {
                borderError.BeginAnimation(OpacityProperty, ErrorAnimation, HandoffBehavior.SnapshotAndReplace);
            }
            finally
            {
                e.CancelCommand();
            }
        }
 
        private void textBoxValue_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up && !IsReadOnly) IncrementValue();
            else if (e.Key == Key.Down && !IsReadOnly) DecrementValue();
            else if (e.Key == Key.Enter) UpdateValue();
            else if (e.Key == Key.Space) e.Handled = true;
        }
 
        private void textBoxValue_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up || e.Key == Key.Down) SelectTextBoxText();
        }
 
        private void textBoxValue_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            textBoxValue.SelectAll();
        }
 
        private void textBoxValue_LostFocus(object sender, RoutedEventArgs e)
        {
            UpdateValue();
        }
 
        private void IncrementValue()
        {
            lock (_locker)
            {
                Value = Value + Increment;
            }
        }
 
        private void DecrementValue()
        {
            lock (_locker)
            {
                Value = Value - Increment;
            }
        }
 
        /// <summary>
        /// Updates the value after text has been typed.
        /// </summary>
        private void UpdateValue()
        {
            try
            {
                string currentText = textBoxValue.Text;
 
                if (_decimalPlaces > 0 && !currentText.Contains(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator))
                {
                    var sb = new StringBuilder(currentText);
                    sb.Append(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
 
                    for (int i = 0; i < _decimalPlaces; i++)
                    {
                        sb.Append("0");
                    }
 
                    currentText = sb.ToString();
                }
 
                decimal d = Convert.ToDecimal(currentText, CultureInfo.CurrentCulture);
 
                if (Value == d) UpdateTextBox(); // reset text to a correct value (it might be incorrect)
                else Value = d;
            }
            catch (FormatException)
            {
                borderError.BeginAnimation(OpacityProperty, ErrorAnimation, HandoffBehavior.SnapshotAndReplace);
                UpdateTextBox(); // wrong chars, so set it back to normal
            }
            catch (OverflowException)
            {
                borderError.BeginAnimation(OpacityProperty, ErrorAnimation, HandoffBehavior.SnapshotAndReplace);
 
                try
                {
                    string firstChar = textBoxValue.Text.Substring(0, 1);

                    Value = firstChar == NumberFormatInfo.CurrentInfo.NegativeSign ? _min : _max;
                }
                catch (Exception)
                {
                    UpdateTextBox(); // if in some mystical case text is empty
                }
            }
        }
 
        /// <summary>
        /// Updates the text box.
        /// </summary>
        private void UpdateTextBox()
        {
            textBoxValue.Text = Value.ToString(_decimalFormat, CultureInfo.CurrentCulture);
 
            // ignore selection start during repeat button pressed
            if (!(IsMouseOver && Mouse.LeftButton == MouseButtonState.Pressed)) textBoxValue.SelectionStart = textBoxValue.Text.Length;
        }
 
        private void UpscaleIncrement(string val, int decimalIndex)
        {
            string newVal = val.Substring(0, decimalIndex + 1 + _decimalPlaces);
 
            var sb = new StringBuilder("0.");
 
            for (int i = 0; i < _decimalPlaces - 1; i++)
            {
                sb.Append("0");
            }
 
            sb.Append("1");
 
            // If DecimalPlaces = 2 and value = 28.005 then increment will be 28.00, if value = 0.0005 then increment wil be 0.01
            _inc = Math.Max(Convert.ToDecimal(newVal, CultureInfo.InvariantCulture), Convert.ToDecimal(sb.ToString(), CultureInfo.InvariantCulture));
        }
 
        private void SetTextBoxMaxLength()
        {
            textBoxValue.MaxLength = Math.Max(_max.ToString(_decimalFormat, CultureInfo.CurrentCulture).Length, _min.ToString(_decimalFormat, CultureInfo.CurrentCulture).Length);
        }
    }
}
