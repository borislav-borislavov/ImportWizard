using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportWizard.Models
{
    public class ColumnPreference
    {
        public bool Selected { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public int Index { get; set; }
        public int? MaxLength { get; set; }
        public SqlServerDataType Type { get; set; }
        public bool IsNullable { get; set; }
        public int? NumericPrecision { get; set; }
        public int? NumericScale { get; set; }
        public string? DefaultValue { get; internal set; }
    }
}
