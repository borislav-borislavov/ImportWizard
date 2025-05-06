using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportWizard.Tests.Builder.Models
{
    public class CellData
    {
        public object Value { get; set; }
        public string Format { get; set; }
        public string Formula { get; set; }

        public CellData(object value)
        {
            Value = value;
        }
    }
}
