using Hidistro.Core.Jobs;
using Hidistro.Messages;
using System;
using System.Globalization;
using System.Xml;

namespace Hidistro.Jobs
{
    public class SubsiteEmailJob : IJob
    {
        int failureInterval = 15;
        int numberOfTries = 3;

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
                        numberOfTries = 3;
                    }
                }
                Emails.SendSubsiteEmails(failureInterval, numberOfTries);
            }
        }
    }
}

