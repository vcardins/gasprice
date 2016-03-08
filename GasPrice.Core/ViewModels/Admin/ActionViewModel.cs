using GasPrice.Core.Common.Enums;

namespace GasPrice.Core.ViewModels.Admin
{
    public class ActionViewModel
    {
        public ModelAction Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LogTemplate { get; set; }
    }
}
