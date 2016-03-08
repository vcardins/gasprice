
using System.ComponentModel;

namespace GasPrice.Core.Events
{
    public enum EventAction
    {
        [Description("Created")]
        Create,
        [Description("Updated")]
        Update,
        [Description("Deleted")]
        Delete,
        [Description("Exported")]
        Export,
        [Description("Published")]
        Publish,
        [Description("Custom")]
        Custom = 99
    }
}