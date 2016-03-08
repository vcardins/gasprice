using System.ComponentModel;

namespace GasPrice.Core.Common.Enums
{
    public enum NewsletterPeriodicity
    {
        [Description("Diariamente")]
        Daily = 1,

        [Description("Semanalmente")]
        Weekly = 2,

        [Description("Mensalmente")]
        Monthly = 3
    }
}