using System;
using Hidistro.Core;

namespace Hidistro.Messages
{

    public abstract class InnerMessageProvider
    {
       static readonly InnerMessageProvider DefaultInstance = (DataProviders.CreateInstance("Hidistro.Messages.Data.InnerMessageData,Hidistro.Messages.Data") as InnerMessageProvider);

        protected InnerMessageProvider()
        {
        }

        public static InnerMessageProvider Instance()
        {
            return DefaultInstance;
        }

        public abstract bool SendDistributorMessage(string subject, string message, string distributor, string sendto);
        public abstract bool SendMessage(string subject, string message, string sendto);
    }
}

