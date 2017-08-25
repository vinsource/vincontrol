using System.Windows.Controls;

namespace vincontrol.Backend.Controls.DataFeed.Import
{
    /// <summary>
    /// Interaction logic for IncomingProfileTemplateMapping.xaml
    /// </summary>
// ReSharper disable RedundantExtendsListEntry
    public partial class IncomingProfileTemplateMapping : UserControl
// ReSharper restore RedundantExtendsListEntry
    {
        //public bool IsAdded { get; set; }

        //private IncomingProfileTemplateViewModel _vm;

        public IncomingProfileTemplateMapping()
        {
            InitializeComponent();

            //Loaded += delegate
            //              {
            //                  _vm = new IncomingProfileTemplateViewModel(this,null);
            //              };
        }

        //#region Implementation of IView

        //public void SetDataContext(object context)
        //{
        //    DataContext = context;
        //}

        //public void Close()
        //{
        //    throw new NotImplementedException();
        //}

        //#endregion

        //#region Implementation of IIdentityView

        //public int Id { get; set; }

        //#endregion
    }
}
