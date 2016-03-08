
namespace GasPrice.Core.ViewModels.Admin
{
    public class ModuleConfig
    {
        
        public int? Width { get; set; }
        public int? Height { get; set; }
        public string ShorcutIconCls { get; set; }
        public string IconCls { get; set; }
        public byte Singleton { get; set; }
        public bool Maximizable { get; set; }
        public bool Minimizable { get; set; }
        public bool ShowInMenu { get; set; }
        public bool ShowInDesktop { get; set; }
    }
}
