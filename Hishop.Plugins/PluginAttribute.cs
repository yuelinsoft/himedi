namespace Hishop.Plugins
{
    using System;
    using System.Runtime.CompilerServices;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=false)]
    public sealed class PluginAttribute : Attribute
    {
        public PluginAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get;set; }

        public int Sequence { get; set; }
    }
}

