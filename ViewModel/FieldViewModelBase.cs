namespace SampleDynamicGridWithMultipleFieldTypes.ViewModel
{
    public abstract class FieldViewModelBase
    {
        public FieldViewModelBase(string propertyName, object instance)
        {
            PropertyName = propertyName;
            Instance = instance;
        }
        public string PropertyName { get; set; }
        private object Instance { get; }

        public object ValueObject
        {
            get
            {
                return Instance
                    .GetType()
                    .GetProperty(PropertyName)
                    .GetGetMethod()
                    .Invoke(Instance, new object[0]);
            }
            set
            {
                Instance
                    .GetType()
                    .GetProperty(PropertyName)
                    .GetSetMethod()
                    .Invoke(Instance, new object[] { value });
            }
        }
    }
}
