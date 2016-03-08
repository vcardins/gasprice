
using System.ComponentModel;

namespace GasPrice.Core.Common.Enums
{
    public enum ModelAction
    {
        [Description("Desconhecido")]
        Unknown = 0,
        [Description("Criado(a)")]
        Create = 1,
        [Description("Lido(a)")]
        Read = 2,
        [Description("Atualizado(a)")]
        Update = 3,
        [Description("Removido(a)")]
        Delete = 4,
        [Description("Exportado(a)")]
        Export = 5,
        [Description("Publicado(a)")]
        Publish = 6,
        [Description("Gerenciado(a)")]
        Manage = 7,
        [Description("Importado(a)")]
        Import = 8,
        [Description("Subscrito(a)")]
        Subscribe = 9,
        [Description("Subscrição cancelada")]
        Unsubscribe = 10,
        [Description("Tarefa Programada")]
        BackgroundTask = 12
    }
}