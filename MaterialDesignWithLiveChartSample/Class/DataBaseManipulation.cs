using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDesignWithLiveChartSample.Class
{
    public class DataBaseManipulation
    {
    }
    public class EmoticonList
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Emoticon { get; set; }
        public string Description { get; set; }

        public EmoticonList() 
        {
            Name = ""; Emoticon = ""; Description = "";
        }
    }
}
