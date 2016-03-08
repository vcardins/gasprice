
using System;
using GasPrice.Core.Account;

namespace GasPrice.Core.Models.Modules
{
    public class FuelPriceHistory : BaseObjectState
    {
        public int FuelServiceId { get; set; }
        public int GasStationId { get; set; }
        public virtual FuelServiceGasStation GasStationService { get; set; }
        public int UserId { get; set; }
        public virtual UserAccount User { get; set; }
        public DateTime DateTime { get; set; }
        public Decimal Price { get; set; }     
    }
}
