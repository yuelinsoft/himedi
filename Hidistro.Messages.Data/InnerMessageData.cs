namespace Hidistro.Messages.Data
{
    using Hidistro.Messages;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;

    public class InnerMessageData : InnerMessageProvider
    {
       readonly Database database = DatabaseFactory.CreateDatabase();

        public override bool SendDistributorMessage(string subject, string message, string distributor, string sendto)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO distro_ReceivedMessages(Addresser, Addressee, Title, PublishContent, PublishDate, LastTime, IsRead) VALUES(@Addresser, @Addressee, @Title, @PublishContent, @SendTime, @SendTime, 0)");
            this.database.AddInParameter(sqlStringCommand, "Addresser", DbType.String, distributor);
            this.database.AddInParameter(sqlStringCommand, "Addressee", DbType.String, sendto);
            this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, subject);
            this.database.AddInParameter(sqlStringCommand, "PublishContent", DbType.String, message);
            this.database.AddInParameter(sqlStringCommand, "SendTime", DbType.DateTime, DateTime.Now);
            return (this.database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }

        public override bool SendMessage(string subject, string message, string sendto)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_ReceivedMessages(Addresser, Addressee, Title, PublishContent, PublishDate, LastTime, IsRead) VALUES('admin', @Addressee, @Title, @PublishContent, @SendTime, @SendTime, 0)");
            this.database.AddInParameter(sqlStringCommand, "Addressee", DbType.String, sendto);
            this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, subject);
            this.database.AddInParameter(sqlStringCommand, "PublishContent", DbType.String, message);
            this.database.AddInParameter(sqlStringCommand, "SendTime", DbType.DateTime, DateTime.Now);
            return (this.database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }
    }
}

