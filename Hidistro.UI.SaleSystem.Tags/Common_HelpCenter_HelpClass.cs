using Hidistro.SaleSystem.Comments;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hidistro.Entities.Comments;
using System.Runtime.CompilerServices;

namespace Hidistro.UI.SaleSystem.Tags
{

    public class Common_HelpCenter_HelpClass : ThemedTemplatedRepeater
    {
        
       int _MaxNum;
        public const string TagID = "list_Common_HelpCenter_HelpClass";

        public Common_HelpCenter_HelpClass()
        {
            base.ID = "list_Common_HelpCenter_HelpClass";
        }

       IList<HelpCategoryInfo> GetDataSource()
        {
            IList<HelpCategoryInfo> helpCategorys = new List<HelpCategoryInfo>();
            helpCategorys = CommentBrowser.GetHelpCategorys();
            if ((this.MaxNum > 0) && (this.MaxNum < helpCategorys.Count))
            {
                for (int i = helpCategorys.Count - 1; i >= this.MaxNum; i--)
                {
                    helpCategorys.RemoveAt(i);
                }
            }
            return helpCategorys;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                base.DataSource = this.GetDataSource();
                base.DataBind();
            }
        }

        public override string ID
        {
            get
            {
                return base.ID;
            }
            set
            {
            }
        }

        public int MaxNum
        {
            
            get
            {
                return this._MaxNum;
            }
            
            set
            {
                this._MaxNum = value;
            }
        }
    }
}

