namespace GasPrice.Core.Config
{
    #region

    #endregion
    public interface IBaseElementSettings<TType>
    {

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        TType Name { get; set; }

        //string ElementKey { get; set; }
        
    }
}