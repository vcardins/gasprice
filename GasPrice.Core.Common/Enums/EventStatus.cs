
using System.ComponentModel;

namespace GasPrice.Core.Common.Enums
{
    public enum EventStatus
    {
        [Description("Sucesso")]
        Success,
        [Description("Falha")]
        Failure,
        [Description("Pendência")]
        Pending,
        [Description("Sem ação")]
        NoAction
    }
}