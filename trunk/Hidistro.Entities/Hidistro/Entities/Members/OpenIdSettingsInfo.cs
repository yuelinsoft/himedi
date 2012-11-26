namespace Hidistro.Entities.Members
{
    using System;
    using System.Runtime.CompilerServices;

    public class OpenIdSettingsInfo
    {
        
       string _Description ;
        
       string _Name ;
        
       string _OpenIdType ;
        
       string _Settings ;

        public string Description
        {
            
            get
            {
                return this._Description ;
            }
            
            set
            {
                this._Description  = value;
            }
        }

        public string Name
        {
            
            get
            {
                return this._Name ;
            }
            
            set
            {
                this._Name  = value;
            }
        }

        public string OpenIdType
        {
            
            get
            {
                return this._OpenIdType ;
            }
            
            set
            {
                this._OpenIdType  = value;
            }
        }

        public string Settings
        {
            
            get
            {
                return this._Settings ;
            }
            
            set
            {
                this._Settings  = value;
            }
        }
    }
}

