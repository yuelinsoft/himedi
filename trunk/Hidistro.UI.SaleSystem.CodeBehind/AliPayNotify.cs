namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text;

    public class AliPayNotify
    {
       string _input_charset = "";
       string _key = "";
       string _partner = "";
       string _sign_type = "";
       string _veryfy_url = "";
       string Https_veryfy_url = "https://www.alipay.com/cooperate/gateway.do?service=notify_verify&";
       string mysign = "";
       string preSignStr = "";
       string responseTxt = "";
       Dictionary<string, string> sPara = new Dictionary<string, string>();

        public AliPayNotify(SortedDictionary<string, string> inputPara, string notify_id, string partner, string key)
        {
            this._partner = partner;
            this._key = key;
            this._input_charset = "utf-8";
            this._sign_type = "MD5";
            this._veryfy_url = this.Https_veryfy_url + "partner=" + this._partner + "&notify_id=" + notify_id;
            this.sPara = OpenIdFunction.FilterPara(inputPara);
            this.preSignStr = OpenIdFunction.CreateLinkString(this.sPara);
            this.mysign = OpenIdFunction.BuildMysign(this.sPara, this._key, this._sign_type, this._input_charset);
            this.responseTxt = this.Get_Http(this._veryfy_url, 0x1d4c0);
        }

       string Get_Http(string strUrl, int timeout)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(strUrl);
                request.Timeout = timeout;
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
                StringBuilder builder = new StringBuilder();
                while (-1 != reader.Peek())
                {
                    builder.Append(reader.ReadLine());
                }
                return builder.ToString();
            }
            catch (Exception exception)
            {
                return ("错误：" + exception.Message);
            }
        }

        public string Mysign
        {
            get
            {
                return this.mysign;
            }
        }

        public string PreSignStr
        {
            get
            {
                return this.preSignStr;
            }
        }

        public string ResponseTxt
        {
            get
            {
                return this.responseTxt;
            }
        }
    }
}

