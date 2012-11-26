using System;
using System.Collections;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using Microsoft.Web.Services3;
using System.ComponentModel;

/// <summary>
///api 的摘要说明
/// </summary>
[WebService(Namespace = "http://www.92hi.com/"), WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1), ToolboxItem(false), Policy("HiServerPolicy")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
//若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。 
// [System.Web.Script.Services.ScriptService]
public class api : System.Web.Services.WebService
{

    public api()
    {
        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }

}

