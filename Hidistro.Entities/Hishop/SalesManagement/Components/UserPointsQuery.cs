using Hidistro.Core.Entities;
using System;
using System.Runtime.CompilerServices;

namespace Hishop.SalesManagement.Components
{
    public class UserPointsQuery : Pagination
    {

        long _JournalNumber;

        public long JournalNumber
        {

            get
            {
                return this._JournalNumber;
            }

            set
            {
                this._JournalNumber = value;
            }
        }
    }
}

