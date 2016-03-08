﻿
namespace GasPrice.Core.Data.UnitOfWork
{
    public enum DbIsolationLevel
    {
        Chaos,
        ReadCommitted,
        ReadUncommitted,
        RepeatableRead,
        Serializable,
        Snapshot,
        Unspecified
    }
}