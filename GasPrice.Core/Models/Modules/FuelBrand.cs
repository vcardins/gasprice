using System.Collections.Generic;

namespace GasPrice.Core.Models.Modules
{
    public class FuelBrand : BaseObjectState
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public virtual ICollection<GasStation> Articles { get; set; }
        public FuelBrand()
        {
            Articles = new List<GasStation>();
        }

    }
}
