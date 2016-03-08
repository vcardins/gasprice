namespace GasPrice.Core.Models.Modules
{
    /// <summary>
    /// Abstract class for all dynamic properties.
    /// </summary>
    public abstract class DynamicProperty<T>
    {
        // public methods...
        /// <summary>
        /// Gets value of child property with the given name.
        /// </summary>
        /// <typeparam name="TValue">The value type of dynamic property.</typeparam>
        /// <param name="name">The name of the property to get.</param>
        public abstract TValue GetValue<TValue>(string name);

        /// <summary>
        /// Sets value of child property with the given name.
        /// </summary>
        /// <typeparam name="TValue">The value type of dynamic property.</typeparam>
        /// <param name="name">The name of the property to get.</param>
        /// <param name="value">The value to set.</param>
        public abstract void SetValue<TValue>(string name, TValue value);

        /// <summary>
        /// Returns true if there is a child property
        /// with the given name inside this dynamic property.
        /// </summary>
        /// <param name="name">The name of the property to check.</param>
        public abstract bool HasProperty(string name);

        /// <summary>
        /// Adds child property to this dynamic property.
        /// </summary>
        /// <typeparam name="TValue">The value type of dynamic property.</typeparam>
        /// <param name="name"> </param>
        /// <param name="property">The property to add.</param>
        public abstract void AddProperty<TValue>(string name, string property);

        /// <summary>
        /// Removes child property from this dynamic property.
        /// </summary>
        /// <param name="name">The name of the property to remove.</param>
        public abstract void RemoveProperty(string name);

        /// <summary>
        /// Gets child dynamic property with the given name.
        /// </summary>
        /// <typeparam name="TValue">The value type of dynamic property.</typeparam>
        /// <param name="name">The name of the child property to get.</param>
        public abstract DynamicProperty<TValue> GetProperty<TValue>(string name);

        // public properties...
        /// <summary>
        /// Gets or sets value of this property.
        /// </summary>
        public abstract T Value
        {
            get;
            set;
        }
    }
}
