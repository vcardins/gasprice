using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using GasPrice.Core.Config;

namespace GasPrice.Config
{
    public abstract class BaseElementCollection<TConfig, TType> : ConfigurationElementCollection, 
        IEnumerable<TConfig> where TConfig : ConfigurationElement, IBaseElementSettings<TType>, new()
    {
        /// <summary>
        /// The property name.
        /// </summary>
        internal const string PropertyName = "add";


        /// <summary>
        /// The is element name.
        /// </summary>
        /// <param name="elementName">
        /// The element name.
        /// </param>
        /// <returns>
        /// The System.Boolean.
        /// </returns>
        protected override bool IsElementName(string elementName)
        {
            return elementName.Equals(PropertyName, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Gets the element name.
        /// </summary>
        protected override string ElementName
        {
            get
            {
                return PropertyName;
            }
        }

        IEnumerator<TConfig> IEnumerable<TConfig>.GetEnumerator()
        {
            foreach (TConfig type in this)
            {
                yield return type;
            }
        }

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        public void Add(ProviderSettings provider)
        {
            if (provider != null)
            {
                BaseAdd(provider);
            }
        }
        protected override ConfigurationElement CreateNewElement()
        {
            return new TConfig();
        }

        /// <summary>
        /// The remove.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        public void Remove(object name)
        {
            BaseRemove(name);
        }

        /// <summary>
        /// The clear.
        /// </summary>
        public void Clear()
        {
            BaseClear();
        }

        /// <summary>
        /// The get element key.
        /// </summary>
        /// <param name="element">
        /// The element.
        /// </param>
        /// <returns>
        /// The System.Object.
        /// </returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TConfig)element).Name;
        }

        /// <summary>
        /// The this.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// </returns>
        public TConfig this[TType key]
        {
            get
            {
                return this.Cast<TConfig>().Single(ce => ce.Name.Equals(key));
            }
        }

        /// <summary>
        /// The this.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <returns>
        /// The JamesRocks.Reports.WebServiceElement.
        /// </returns>
        public ConfigurationElement this[int index]
        {
            get
            {
                return BaseGet(index);
            }

            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }

                BaseAdd(index, value);
            }
        }
    }
}