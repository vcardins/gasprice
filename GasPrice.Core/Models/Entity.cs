using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using GasPrice.Core.Data.Infrastructure;
using GasPrice.Core.Filters;

namespace GasPrice.Core.Models
{
    public abstract class Entity : IObjectState, IValidatableObject
    {
        [NotMapped]
        [IgnoreDataMember]
        public ObjectState ObjectState { get; set; }

        /// <summary>
        /// Create datetime.
        /// </summary>
        [AutoPopulate]
        public DateTime Created { get; set; }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Override this method to implement custom validation in your entities
            // This is only for making it compile... and returning null will give an exception.
            if (false)
                yield return new ValidationResult("Well, this should not happend...");
        }
    }
}