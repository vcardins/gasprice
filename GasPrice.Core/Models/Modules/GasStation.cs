using System;
using System.Collections.Generic;

namespace GasPrice.Core.Models.Modules
{
    public class GasStation : Entity
    {
        public int Id { get; set; }
        public int? FuelBrandId { get; set; }
        public virtual FuelBrand Brand { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public Double GeoLocation { get; set; }
        public bool Enabled { get; set; }
        public virtual ICollection<FuelServiceGasStation> Services { get; set; }
        
        public GasStation()
        {
            Services = new List<FuelServiceGasStation>();
        }

    }
}
