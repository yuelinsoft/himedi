namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class TbImage
    {
        
       string _Imgpath;
        
       long _TbProductId;

        public string Imgpath
        {
            
            get
            {
                return this._Imgpath;
            }
            
            set
            {
                this._Imgpath = value;
            }
        }

        public long TbProductId
        {
            
            get
            {
                return this._TbProductId;
            }
            
            set
            {
                this._TbProductId = value;
            }
        }
    }
}

