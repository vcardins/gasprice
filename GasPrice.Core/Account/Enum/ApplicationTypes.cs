using System.ComponentModel;

namespace GasPrice.Core.Account.Enum
{
    public enum ApplicationTypes
    {        
        [Description("Native")]
        NativeConfidential = 0,
        [Description("JavaScript")]
        JavaScript = 1
    }
}