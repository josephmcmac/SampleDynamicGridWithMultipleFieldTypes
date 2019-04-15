using SampleDynamicGridWithMultipleFieldTypes.Model;
using SampleDynamicGridWithMultipleFieldTypes.ViewModel;
using System;
using System.Windows;

namespace SampleDynamicGridWithMultipleFieldTypes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            var myInstance = new MyDataGridType();
            myInstance.MyGridRows = new[]
            {
                new MyGridRowType() { MyStringField = "Initial Row 1" },
                new MyGridRowType() { MyStringField = "Initial Row 2", MyBooleanField = true },
                new MyGridRowType() { MyStringField = "Initial Row 3" },
            };
            var viewModel = new DataGridViewModel(myInstance, nameof(MyDataGridType.MyGridRows));
            DataContext = viewModel;
        }
    }
}
