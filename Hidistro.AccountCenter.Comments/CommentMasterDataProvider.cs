using System;
using Hidistro.Core;

namespace Hidistro.AccountCenter.Comments
{
    public abstract class CommentMasterDataProvider : CommentDataProvider
    {
        static readonly CommentMasterDataProvider _defaultInstance = (DataProviders.CreateInstance("Hidistro.AccountCenter.Data.CommentData,Hidistro.AccountCenter.Data") as CommentMasterDataProvider);

        protected CommentMasterDataProvider()
        {
        }

        public static CommentMasterDataProvider CreateInstance()
        {
            return _defaultInstance;
        }
    }
}

