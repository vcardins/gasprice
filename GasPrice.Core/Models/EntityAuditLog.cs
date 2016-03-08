using System;
using GasPrice.Core.Account;
using GasPrice.Core.Filters;

namespace GasPrice.Core.Models
{
   
    public abstract class EntityAuditLog : Entity
    {
        /// <summary>
        /// Create datetime.
        /// </summary>
        [AutoPopulate]
        public DateTime? Updated { get; set; }
        public int CreatedById { get; set; }
        public virtual UserAccount CreatedBy { get; set; }
        public int? UpdatedById { get; set; }
        public virtual UserAccount UpdatedBy { get; set; }

    }

}