namespace Hidistro.Entities
{
    using System;
    using System.Runtime.CompilerServices;

    public class ManageThemeInfo
    {
        
       string _Name ;
        
       string _ThemeImgUrl ;
        
       string _ThemeName ;

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

        public string ThemeImgUrl
        {
            
            get
            {
                return this._ThemeImgUrl ;
            }
            
            set
            {
                this._ThemeImgUrl  = value;
            }
        }

        public string ThemeName
        {
            
            get
            {
                return this._ThemeName ;
            }
            
            set
            {
                this._ThemeName  = value;
            }
        }
    }
}

