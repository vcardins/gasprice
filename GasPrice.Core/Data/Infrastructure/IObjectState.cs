
using System.ComponentModel.DataAnnotations.Schema;

namespace GasPrice.Core.Data.Infrastructure
{
    public interface IObjectState
    {
        [NotMapped]
        ObjectState ObjectState { get; set; }
    }
}