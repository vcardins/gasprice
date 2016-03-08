using System.ComponentModel;

namespace GasPrice.Core.Common.Enums
{
    public enum ModelType
    {
        [Description("Ação")]
        Action = 1,
        [Description("Configuração")]
        AppSettings = 2,
        [Description("Notícias")]
        Article = 3,
        [Description("Grupo de Notícias")]
        FuelBrand = 4,
        [Description("Banner")]
        Banner = 5,
        [Description("Gerenciador de Medias")]
        MediaManager = 6,
        [Description("Log de Erro")]
        ExceptionLog = 7,
        [Description("Servidor FTP")]
        HostServer = 8,
        [Description("Galeria de Imagens")]
        ImageGallery = 9,
        [Description("Log")]
        Log = 10,
        [Description("Media")]
        Media = 11,
        [Description("Tipo de Media")]
        MediaType = 12,
        [Description("Modulo do Sistema")]
        Module = 13,
        [Description("Página")]
        Page = 14,
        [Description("Permissao")]
        Permission = 15,
        [Description("Atribuição")]
        Role = 16,
        [Description("Atribuição/Permissão")]
        RolePermission = 17,
        [Description("Rss Feed")]
        RssFeed = 18,
        [Description("Árvore de Site")]
        SiteTree = 19,
        [Description("Conta do Usuário")]
        UserAccount = 20,
        [Description("Usuário")]
        UserClaim = 21,
        [Description("Atribuição/Usuário")]
        UserRole = 22,
        [Description("Configuração de Usuário")]
        UserSettings = 23,
        [Description("Mensagem de Visitante")]
        VisitorMessage = 24,
        [Description("Grupo de Mensagem de Visitante")]
        VisitorMessageGroup = 25,
        [Description("Assinante")]
        Subscriber = 26,
        [Description("Boletim de Notícias")]
        Newsletter = 27,
        [Description("Desconhecido")]
        Unknown = 99
    }
}

