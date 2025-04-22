using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportWizard.Models
{
    /// <summary>
    /// All currently supported SQL Server data types
    /// </summary>
    public enum SqlServerDataType
    {
        VARCHAR = 1,
        NVARCHAR,
        INT,
        DATETIME,
        BIT,
        DECIMAL,
    }
}
