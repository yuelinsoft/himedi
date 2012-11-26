using System;
using Hidistro.Core;

namespace Hidistro.AccountCenter.Comments
{
    public abstract class CommentSubsiteDataProvider : CommentDataProvider
    {
        static readonly CommentSubsiteDataProvider _defaultInstance = (DataProviders.CreateInstance("Hidistro.AccountCenter.DistributionData.CommentData,Hidistro.AccountCenter.DistributionData") as CommentSubsiteDataProvider);

        protected CommentSubsiteDataProvider()
        {
        }

        public static CommentSubsiteDataProvider CreateInstance()
        {
            return _defaultInstance;
        }
    }
}

