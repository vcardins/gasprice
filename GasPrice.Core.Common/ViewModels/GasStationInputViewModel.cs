using System;
using System.ComponentModel.DataAnnotations;

namespace GasPrice.Core.Common.ViewModels
{
    public class GasStationInputViewModel
    {
        public int? PageId { get; set; }
        public int? FuelBrandId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        public Double GeoLocation { get; set; }

    }
}
