namespace Hidistro.Entities.Sales
{
    using System;
    using System.Runtime.CompilerServices;

    public class ExpressCompanyInfo
    {
        
       string _ExpressCompanyAbb ;
        
       string _ExpressCompanyName ;

        public string ExpressCompanyAbb
        {
            
            get
            {
                return this._ExpressCompanyAbb ;
            }
            
            set
            {
                this._ExpressCompanyAbb  = value;
            }
        }

        public string ExpressCompanyName
        {
            
            get
            {
                return this._ExpressCompanyName ;
            }
            
            set
            {
                this._ExpressCompanyName  = value;
            }
        }
    }
}

