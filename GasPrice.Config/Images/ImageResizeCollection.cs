using System;
using System.Configuration;
using System.Linq;

namespace GasPrice.Config.Images
{
    /// <summary>
    /// The photo resize collection.
    /// </summary>
    [ConfigurationCollection(typeof(ImageResizeElement))]
    public class ImageResizeCollection : ConfigurationElementCollection
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

        /// <summary>
        /// The remove.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        public void Remove(string name)
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
        /// The create new element.
        /// </summary>
        /// <returns>
        /// The System.Configuration.ConfigurationElement.
        /// </returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new ImageResizeElement();
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
            return (ImageResizeElement)element;
        }

        /// <summary>
        /// The this.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The JamesRocks.Images.ImageResizeElement.
        /// </returns>
        public new ImageResizeElement this[string key]
        {
            get
            {
                return this.Cast<ImageResizeElement>().Single(ce => ce.Name == key);
                //return (ImageResizeElement)BaseGet(key);
            }
        }

        /// <summary>
        /// The this.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <returns>
        /// The JamesRocks.Images.ImageResizeElement.
        /// </returns>
        public ImageResizeElement this[int index]
        {
            get
            {
                return (ImageResizeElement)BaseGet(index);
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