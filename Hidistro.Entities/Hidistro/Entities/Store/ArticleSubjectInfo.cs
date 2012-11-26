namespace Hidistro.Entities.Store
{
    using System;
    using System.Runtime.CompilerServices;

    public class ArticleSubjectInfo
    {
        
       string _Categories ;
        
       string _CategoryName ;
        
       string _Keywords ;
        
       int _MaxNum ;
        
       int _SubjectId ;
        
       string _SubjectName ;

        public string Categories
        {
            
            get
            {
                return this._Categories ;
            }
            
            set
            {
                this._Categories  = value;
            }
        }

        public string CategoryName
        {
            
            get
            {
                return this._CategoryName ;
            }
            
            set
            {
                this._CategoryName  = value;
            }
        }

        public string Keywords
        {
            
            get
            {
                return this._Keywords ;
            }
            
            set
            {
                this._Keywords  = value;
            }
        }

        public int MaxNum
        {
            
            get
            {
                return this._MaxNum ;
            }
            
            set
            {
                this._MaxNum  = value;
            }
        }

        public int SubjectId
        {
            
            get
            {
                return this._SubjectId ;
            }
            
            set
            {
                this._SubjectId  = value;
            }
        }

        public string SubjectName
        {
            
            get
            {
                return this._SubjectName ;
            }
            
            set
            {
                this._SubjectName  = value;
            }
        }
    }
}

