using Hidistro.Membership.Core.Enums;
using System;
using System.Runtime.Serialization;

namespace Hidistro.Membership.Core
{
    public class CreateUserException : Exception
    {
        Hidistro.Membership.Core.Enums.CreateUserStatus status;

        public CreateUserException()
        {
        }

        public CreateUserException(Hidistro.Membership.Core.Enums.CreateUserStatus status)
        {
            this.status = status;
        }

        public CreateUserException(string message)
            : base(message)
        {
        }

        public CreateUserException(Hidistro.Membership.Core.Enums.CreateUserStatus status, string message)
            : base(message)
        {
            this.status = status;
        }

        protected CreateUserException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public CreateUserException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public CreateUserException(Hidistro.Membership.Core.Enums.CreateUserStatus status, string message, Exception inner)
            : base(message, inner)
        {
            this.status = status;
        }

        public Hidistro.Membership.Core.Enums.CreateUserStatus CreateUserStatus
        {
            get
            {
                return status;
            }
        }
    }
}

