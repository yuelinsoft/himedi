using Hidistro.Entities.Members;
using System;
using System.Collections.Generic;
using Hidistro.Core;

namespace Hidistro.ControlPanel.Members
{
    public abstract class OpenIdProvider
    {
        static readonly OpenIdProvider _defaultInstance = (DataProviders.CreateInstance("Hidistro.ControlPanel.Data.OpenIdData,Hidistro.ControlPanel.Data") as OpenIdProvider);

        protected OpenIdProvider()
        {
        }

        public abstract void DeleteSettings(string openIdType);
        public abstract IList<string> GetConfigedTypes();
        public abstract OpenIdSettingsInfo GetOpenIdSettings(string openIdType);
        public static OpenIdProvider Instance()
        {
            return _defaultInstance;
        }

        public abstract void SaveSettings(OpenIdSettingsInfo settings);
    }
}

