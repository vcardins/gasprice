
using System.ComponentModel;

namespace GasPrice.Core.Common.Enums
{
    public enum ExportingFormat
    {
        [Description("Csv")]
        Csv,
        [Description("Json")]
        Json,
        [Description("Pdf")]
        Pdf,
        [Description("Excel")]
        Xls
    }
}