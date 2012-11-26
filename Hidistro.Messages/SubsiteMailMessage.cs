namespace Hidistro.Messages
{
    using System;
    using System.Net.Mail;
    using System.Runtime.CompilerServices;

    public class SubsiteMailMessage
    {
        
       int _DistributorUserId;
        
       MailMessage _Mail;

        public SubsiteMailMessage(int distributorUserId, MailMessage mail)
        {
            this.Mail = mail;
            this.DistributorUserId = distributorUserId;
        }

        public int DistributorUserId
        {
            
            get
            {
                return this._DistributorUserId;
            }
            
           set
            {
                this._DistributorUserId = value;
            }
        }

        public MailMessage Mail
        {
            
            get
            {
                return this._Mail;
            }
            
           set
            {
                this._Mail = value;
            }
        }
    }
}

