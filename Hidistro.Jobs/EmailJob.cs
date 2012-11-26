using Hidistro.Core.Jobs;
using Hidistro.Membership.Context;
using Hidistro.Messages;
using System;
using System.Globalization;
using System.Xml;

namespace Hidistro.Jobs
{
    public class EmailJob : IJob
    {
        int failureInterval = 15;
        int numberOfTries = 5;

        public void Execute(XmlNode node)
        {
            if (null != node)
            {
                XmlAttribute attribute = node.Attributes["failureInterval"];
                XmlAttribute attribute2 = node.Attributes["numberOfTries"];
                if (attribute != null)
                {
                    try
                    {
                        failureInterval = int.Parse(attribute.Value, CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        failureInterval = 15;
                    }
                }
                if (attribute2 != null)
                {
                    try
                    {
                        numberOfTries = int.Parse(attribute2.Value, CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        numberOfTries = 5;
                    }
                }
                SendQueuedEmailJob();
            }
        }

        public void SendQueuedEmailJob()
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            if (masterSettings != null)
            {
                Emails.SendQueuedEmails(failureInterval, numberOfTries, masterSettings);
            }
        }
    }
}

