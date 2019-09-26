using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCP104Library
{
    [Serializable]
    public class DataModel
    {
        public DataModel() { }
        public string Index { get; set; }
        public string Value { get; set; }
        public string Quality { get; set; }
        public string DateStr { get; set; }
    }
}
