using System;

namespace GasPrice.Core.Common.Infrastructure
{
    public interface ILogger
    {
        void Log(string message);
        void Log(Exception ex);
    }
}
