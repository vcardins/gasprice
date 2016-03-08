
namespace GasPrice.Core.Models.Modules
{
    public class FuelServiceGasStation : BaseObjectState
    {
        public int GasStationId { get; set; }
        public virtual GasStation GasStation { get; set; }
        public int FuelServiceId { get; set; }
        public virtual FuelService Service { get; set; }
        public bool Active { get; set; }       
    }
}
