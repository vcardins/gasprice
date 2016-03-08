
using System.Collections.Generic;

namespace GasPrice.Core.Models.Modules
{
    public class FuelService : BaseObjectState
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<FuelServiceGasStation> GasStations { get; set; }
    }
}
