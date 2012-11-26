using System;
using System.Data.SqlClient;
using System.Web;
using System.Web.Hosting;

namespace Hidistro.Membership.ASPNETProvider
{
    internal sealed class SqlConnectionHolder
    {
        internal SqlConnection _Connection;
        bool _Opened;

        internal SqlConnectionHolder(string connectionString)
        {
            try
            {
               _Connection = new SqlConnection(connectionString);
            }
            catch (ArgumentException exception)
            {
                throw new ArgumentException(Hidistro.Membership.ASPNETProvider.SR.GetString("An error occurred while attempting to initialize a System.Data.SqlClient.SqlConnection object. The value that was provided for the connection string may be wrong, or it may contain an invalid syntax."), "connectionString", exception);
            }
        }

        internal void Close()
        {
            if (_Opened)
            {
               Connection.Close();
               _Opened = false;
            }
        }

        internal void Open(HttpContext context, bool revertImpersonate)
        {
            if (_Opened) { return; }

            if (revertImpersonate)
            {
                using (HostingEnvironment.Impersonate())
                {
                   Connection.Open();
                   _Opened = true;
                }
            }
            else
            {
               Connection.Open();
            }
        }

        internal SqlConnection Connection
        {
            get
            {
                return _Connection;
            }
        }
    }
}

