namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.SaleSystem.Tags;
    using System;

    public class RegisterAgreement : HtmlTemplatedWebControl
    {
       Common_Register_Clause litlAgreemen;

        protected override void AttachChildControls()
        {
            this.litlAgreemen = (Common_Register_Clause) this.FindControl("div_Common_Register_Clause");
            if (!this.Page.IsPostBack && (this.litlAgreemen != null))
            {
                this.litlAgreemen.InnerHtml = "<div><p>尊敬的用户，欢迎您注册成为本网站用户。在注册前请您仔细阅读如下服务条款：</p> <div>　　本服务协议双方为本网站与本网站用户，本服务协议具有合同效力。</div> <div>　　您确认本服务协议后，本服务协议即在您和本网站之间产生法律效力。请您务必在注册之前认真阅读全部服务协议内容，如有任何疑问，可向本网站咨询。</div> <div>　　无论您事实上是否在注册之前认真阅读了本服务协议，只要您点击协议正本下方的&quot;注册&quot;按钮并按照本网站注册程序成功注册为用户，您的行为仍然表示您同意并签署了本服务协议。</div> <div>1．本网站服务条款的确认和接纳</div> <div>　　本网站各项服务的所有权和运作权归本网站拥有。</div> <div>2．用户必须：</div> <div>　　(1)自行配备上网的所需设备， 包括个人电脑、调制解调器或其他必备上网装置。</div> <div>　　(2)自行负担个人上网所支付的与此服务有关的电话费用、 网络费用。</div> <div>3．用户在本网站上交易平台上不得发布下列违法信息：</div> <div>(1)反对宪法所确定的基本原则的；</div> <div>(2).危害国家安全，泄露国家秘密，颠覆国家政权，破坏国家统一的；</div> <div>(3).损害国家荣誉和利益的；</div> <div>(4).煽动民族仇恨、民族歧视，破坏民族团结的；</div> <div>(5).破坏国家宗教政策，宣扬邪教和封建迷信的；</div> <div>(6).散布谣言，扰乱社会秩序，破坏社会稳定的；</div> <div>(7).散布淫秽、色情、赌博、暴力、凶杀、恐怖或者教唆犯罪的；</div> <div>(8).侮辱或者诽谤他人，侵害他人合法权益的；</div> <div>(9).含有法律、行政法规禁止的其他内容的。</div> <div>4． 有关个人资料</div> <div>用户同意：</div> <div>(1) 提供及时、详尽及准确的个人资料。</div> <div>(2).同意接收来自本网站的信息。</div> <div>(3) 不断更新注册资料，符合及时、详尽准确的要求。所有原始键入的资料将引用为注册资料。</div> <div>(4)本网站不公开用户的姓名、地址、电子邮箱和笔名，以下情况除外：</div> <div>　　（a）用户授权本网站透露这些信息。</div> <div>　　（b）相应的法律及程序要求本网站提供用户的个人资料。如果用户提供的资料包含有不正确的信息，本网站保留结束用户使用本网站信息服务资格的权利。</div> <div>5. 用户在注册时应当选择稳定性及安全性相对较好的电子邮箱，并且同意接受并阅读本网站发往用户的各类电子邮件。如用户未及时从自己的电子邮箱接受电子邮件或因用户电子邮箱或用户电子邮件接收及阅读程序本身的问题使电子邮件无法正常接收或阅读的，只要本网站成功发送了电子邮件，应当视为用户已经接收到相关的电子邮件。电子邮件在发信服务器上所记录的发出时间视为送达时间。</div> <div>6． 服务条款的修改</div> <p><span style=\"font-size: 10.5pt\">　　本网站有权在必要时修改服务条款，本网站服务条款一旦发生变动，将会在重要页面上提示修改内容。如果不同意所改动的内容，用户可以主动取消获得的本网站信息服务。如果用户继续享用本网站信息服务，则视为接受服务条款的变动。本网站保留随时修改或中断服</span></p></div></div>";
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-RegisterAgreement.html";
            }
            base.OnInit(e);
        }
    }
}

