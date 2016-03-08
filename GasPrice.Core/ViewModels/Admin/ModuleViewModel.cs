namespace GasPrice.Core.ViewModels.Admin
{
    public class ModuleViewModel
    {
        public int Id { get; set; }
        public int? ParentModuleId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Alias { get; set; }
        public string Description { get; set; }
        public string Klass { get; set; }
        public string Configurations { get; set; }
        public int SortOrder { get; set; }
        public bool Active { get; set; }
        //public IEnumerable<PermissionViewModel> Permissions { get; set; }
        //public IEnumerable<ModuleViewModel> Children { get; set; }      

    }
}
