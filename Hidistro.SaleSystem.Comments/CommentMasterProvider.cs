namespace Hidistro.SaleSystem.Comments
{
    using System;
    using Hidistro.Core;

    public abstract class CommentMasterProvider : CommentProvider
    {
       static readonly CommentMasterProvider _defaultInstance = (DataProviders.CreateInstance("Hidistro.SaleSystem.Data.CommentData,Hidistro.SaleSystem.Data") as CommentMasterProvider);

        protected CommentMasterProvider()
        {
        }

        public static CommentMasterProvider CreateInstance()
        {
            return _defaultInstance;
        }
    }
}

