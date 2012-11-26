using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Hidistro.Membership.Context
{
    internal class ExtensionContainer
    {
        static readonly Hashtable Extensions = new Hashtable();

        static volatile ExtensionContainer instance = null;

        static readonly object Sync = new object();

        ExtensionContainer()
        {

            Extensions.Clear();

            XmlNode configSection = HiContext.Current.Config.GetConfigSection("Hishop/Extensions");

            if (configSection != null)
            {

                string key = "";
                string typeName = "";
                Type type = null;

                foreach (XmlNode node2 in configSection.ChildNodes)
                {

                    if ((node2.NodeType != XmlNodeType.Comment) && node2.Name.Equals("add"))
                    {

                        key = node2.Attributes["name"].Value;

                        typeName = node2.Attributes["type"].Value;

                        XmlAttribute attribute = node2.Attributes["enabled"];

                        if ((attribute == null) || (attribute.Value != "false"))
                        {
                            type = Type.GetType(typeName);

                            if (type == null)
                            {
                                throw new Exception(typeName + " does not exist");
                            }

                            IExtension extension = Activator.CreateInstance(type) as IExtension;

                            if (extension == null)
                            {
                                throw new Exception(typeName + " does not implement IExtension or is not configured correctly");
                            }

                            extension.Init();

                            Extensions.Add(key, extension);

                        }

                    }

                }

            }

        }

        internal static void LoadExtensions()
        {
            if (instance == null)
            {
                lock (Sync)
                {

                    if (instance == null)
                    {
                        instance = new ExtensionContainer();
                    }

                }

            }

        }

    }

}

