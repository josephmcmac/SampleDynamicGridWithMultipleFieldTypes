using System.Collections.Generic;

namespace SampleDynamicGridWithMultipleFieldTypes.Model
{
    public class MyDataGridType
    {
        public MyDataGridType()
        {
            MyGridRows = new List<MyGridRowType>();
        }

        public IEnumerable<MyGridRowType> MyGridRows { get; set; }
    }
}
