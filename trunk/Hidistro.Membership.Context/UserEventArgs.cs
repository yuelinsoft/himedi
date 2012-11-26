using System;
using System.Runtime.CompilerServices;

namespace Hidistro.Membership.Context
{
    public class UserEventArgs : EventArgs
    {
        public UserEventArgs(string username, string password, string dealPassword)
        {
           Username = username;
           Password = password;
           DealPassword = dealPassword;
        }

        public string DealPassword { get; set; }

        public string Password { get; set; }

        public string Username { get; set; }
    }
}

