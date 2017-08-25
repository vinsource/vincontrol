using System.Windows;
using System.Windows.Controls;

namespace vincontrol.Backend.Helper
{
    public class DataGridExtensions
    {
        static DataGridExtensions()
        {
            //Allows to set DataContextProperty on the columns. Must only be invoked once per application.
            FrameworkElement.DataContextProperty.AddOwner(typeof(DataGridColumn));
        }

        public static object GetDataContextForColumns(DependencyObject obj)
        { return obj.GetValue(DataContextForColumnsProperty); }

        public static void SetDataContextForColumns(DependencyObject obj, object value)
        { obj.SetValue(DataContextForColumnsProperty, value); }

        /// <summary>
        /// Allows to set DataContext property on columns of the DataGrid (DataGridColumn)
        /// </summary>
        /// <example><DataGridTextColumn Header="{Binding DataContext.ColumnHeader, RelativeSource={RelativeSource Self}}" /></example>
        public static readonly DependencyProperty DataContextForColumnsProperty =
            DependencyProperty.RegisterAttached(
            "DataContextForColumns", 
            typeof(object), 
            typeof(DataGridExtensions), 
            new UIPropertyMetadata(OnDataContextChanged));

        /// <summary>
        /// Propogates the context change to all the DataGrid's columns
        /// </summary>
        private static void OnDataContextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = d as DataGrid;
            if (grid == null) return;

            foreach (DataGridColumn col in grid.Columns)
                col.SetValue(FrameworkElement.DataContextProperty, e.NewValue);
        }

    }
}
