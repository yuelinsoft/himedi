namespace Hidistro.Core.Configuration
{
    using System;
    using System.Collections.Specialized;
    using System.Xml;

    public class Provider
    {
       string name;
       NameValueCollection providerAttributes = new NameValueCollection();
       string providerType;

        public Provider(XmlAttributeCollection attributes)
        {
            if (attributes != null)
            {
                this.name = attributes["name"].Value;
                this.providerType = attributes["type"].Value;
                foreach (XmlAttribute attribute in attributes)
                {
                    if ((attribute.Name != "name") && (attribute.Name != "type"))
                    {
                        this.providerAttributes.Add(attribute.Name, attribute.Value);
                    }
                }
            }
        }

        public NameValueCollection Attributes
        {
            get
            {
                return this.providerAttributes;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public string Type
        {
            get
            {
                return this.providerType;
            }
        }
    }
}

