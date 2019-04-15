using SampleDynamicGridWithMultipleFieldTypes.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SampleDynamicGridWithMultipleFieldTypes.View
{
    /// <summary>
    /// Interaction logic for DynamicGrid.xaml
    /// </summary>
    public partial class DynamicGrid : UserControl
    {
        public DynamicGrid()
        {
            InitializeComponent();

            DataContextChanged += DynamicGrid_DataContextChanged;
        }

        public DataGridViewModel ViewModel
        {
            get
            {
                return DataContext as DataGridViewModel;
            }
        }

        private void DynamicGrid_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (ViewModel != null)
            {
                var mainType = ViewModel.Instance.GetType();
                var propertyForGrid = mainType.GetProperty(ViewModel.PropertyForGrid);
                var typeForGrid = propertyForGrid.PropertyType.GenericTypeArguments[0];
                //iterate each property in the type enumerated in the grid
                foreach (var gridColumnProperty in typeForGrid.GetProperties())
                {
                    //Binding for the column to the property indexer in our GridRow
                    var cellBinding = new Binding
                    {
                        Path = new PropertyPath(string.Concat("[", gridColumnProperty.Name, "]")),
                        Mode = BindingMode.TwoWay
                    };
                    //Create the column for the type of property
                    DataGridColumn dataGridField;
                    if (gridColumnProperty.PropertyType == typeof(bool))
                    {
                        dataGridField = new GridBooleanColumn()
                        {
                            Binding = cellBinding
                        };
                    }
                    else
                    {
                        dataGridField = new GridStringColumn()
                        {
                            Binding = cellBinding
                        };
                    }
                    //Column header label & width
                    dataGridField.Header = gridColumnProperty.Name;
                    dataGridField.Width = new DataGridLength(200, DataGridLengthUnitType.Pixel);
                    //Add to the column collection of the DataGrid
                    XamlDataGrid.Columns.Add(dataGridField);
                }
                //Bind the ItemsSource of the DataGrid to our Rows
                var dataGridBinding = new Binding
                {
                    Path = new PropertyPath(nameof(DataGridViewModel.Rows)),
                    Mode = BindingMode.TwoWay
                };
                XamlDataGrid.SetBinding(ItemsControl.ItemsSourceProperty, dataGridBinding);
            }
        }
    }
}
