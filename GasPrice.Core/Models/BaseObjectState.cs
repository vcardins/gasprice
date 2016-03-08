using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using GasPrice.Core.Data.Infrastructure;

namespace GasPrice.Core.Models
{

    public abstract class BaseObjectState : IObjectState
    {
        [NotMapped]
        [IgnoreDataMember]
        public ObjectState ObjectState { get; set; }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Override this method to implement custom validation in your entities
            // This is only for making it compile... and returning null will give an exception.
            if (false)
                yield return new ValidationResult("Well, this should not happend...");
        }
    }
}