using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace SampleDynamicGridWithMultipleFieldTypes.ViewModel
{
    public class GridRowViewModel
    {
        private ObservableCollection<FieldViewModelBase> _fields;
        public object Instance { get; set; }

        public GridRowViewModel(object instanceForRow)
        {
            Instance = instanceForRow;
        }

        public ObservableCollection<FieldViewModelBase> Fields
        {
            get
            {
                if(_fields == null)
                {
                    CreateFieldViewModels();
                }
                return _fields;
            }

            set
            {
                _fields = value;
            }
        }

        private void CreateFieldViewModels()
        {
            var fields = new ObservableCollection<FieldViewModelBase>();
            var type = Instance.GetType();
            foreach(var property in type.GetProperties())
            {
                if(property.PropertyType == typeof(bool))
                {
                    fields.Add(new BooleanFieldViewModel(property.Name, Instance));
                }
                else
                {
                    fields.Add(new StringFieldViewModel(property.Name, Instance));
                }
            }
            _fields = fields;
        }

        public FieldViewModelBase this[string fieldName]
        {
            get
            {
                if (Fields.Any(gr => gr.PropertyName == fieldName))
                    return Fields.First(gr => gr.PropertyName == fieldName);
                return null;
            }
            set { throw new NotImplementedException(); }
        }
    }
}
