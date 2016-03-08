
namespace GasPrice.Core.Common.Infrastructure
{
    public interface IRazorTemplateRender
    {
        string Render<T>(T model) where T : new();

        string RenderString<T>(T model) where T : new();
    }

}
