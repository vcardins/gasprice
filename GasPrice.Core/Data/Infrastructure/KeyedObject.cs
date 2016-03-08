namespace GasPrice.Core.Data.Infrastructure
{
    public interface IKeyedObject : IObjectState
    {
        int Id { get; set; }
    }
}