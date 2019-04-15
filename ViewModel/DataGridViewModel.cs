using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace SampleDynamicGridWithMultipleFieldTypes.ViewModel
{
    public class DataGridViewModel
    {
        public object Instance { get; set; }
        public string PropertyForGrid { get; set; }

        public DataGridViewModel(object instance, string propertyForGrid)
        {
            Instance = instance;
            PropertyForGrid = propertyForGrid;

            Rows = new ObservableCollection<GridRowViewModel>();
            //read the enumerable property and load it into the Rows
            var propertyValue = Instance
                .GetType()
                .GetProperty(propertyForGrid)
                .GetGetMethod()
                .Invoke(Instance, new object[0]);
            var enumerable = ((IEnumerable)propertyValue);
            foreach (var item in enumerable)
            {
                Rows.Add(new GridRowViewModel(item));
            }

            AddButtonCommand = new MyCommand(AddRow);
        }

        public ICommand AddButtonCommand
        {
            get; set;
        }

        private void AddRow()
        {
            var rowPropertyType = Instance
                .GetType()
                .GetProperty(PropertyForGrid)
                .PropertyType
                .GenericTypeArguments[0];
            var newInstanceForRow = Activator.CreateInstance(rowPropertyType);
            var gridRow = new GridRowViewModel(newInstanceForRow);
            Rows.Add(gridRow);

            RefreshGridRowsIntoObjectEnumerable(rowPropertyType);
        }

        private void RefreshGridRowsIntoObjectEnumerable(Type rowPropertyType)
        {
            //this outputs an object of type object[]
            var rowInstances = Rows.Select(g => g.Instance).ToArray();
            //this casts the array above to our desired type
            var typedEnumerable = typeof(Enumerable)
                .GetMethod(nameof(Enumerable.Cast), new[] { typeof(IEnumerable) })
                .MakeGenericMethod(rowPropertyType)
                .Invoke(null, new object[] { rowInstances });
            //this forces enumeration of the cast array
            typedEnumerable = typeof(Enumerable)
                .GetMethod(nameof(Enumerable.ToArray))
                .MakeGenericMethod(rowPropertyType)
                .Invoke(null, new object[] { typedEnumerable });
            //now we have an enumerated array of the correct type
            //there will be no error setting it to the property on our target object
            Instance.GetType()
                .GetProperty(PropertyForGrid)
                .GetSetMethod()
                .Invoke(Instance, new[] { typedEnumerable });
        }

        public ObservableCollection<GridRowViewModel> Rows { get; set; }
    }
}
