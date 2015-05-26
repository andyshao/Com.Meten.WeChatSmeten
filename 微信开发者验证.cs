using MetenBlog.Admin.Models;
using MetenBlog.Common;
using MetenBlogBLL.WcPromoter;
using MetenBlogCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

namespace MetenBlog.Admin.wcPromoter
{
	/// <summary>
	/// WechatIndex 的摘要说明
	/// </summary>
	public class WechatIndex : IHttpHandler
	{

		private string const TokenString="metenwechat";
		
		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/plain";
			if (context.Request.RequestType.Equals("GET"))
			{
				WeChatValidate(context);	//微信验证
			}
			else
			{
				WeChatRequest(context);		//处理微信服务器推送数据
			}
			

		}

		/// <summary>
		/// 微信验证
		/// </summary>
		private void WeChatValidate(HttpContext context)
		{
			HttpRequest request = context.Request;
			string echostr = context.Request["echostr"];
			string signature = request["signature"];
			string timestamp = request["timestamp"];
			string nonce = request["nonce"];

			
			string[] arrStr = {TokenString, timestamp, nonce };
			Array.Sort(arrStr);
			string tempStr = string.Join("", arrStr);
			tempStr = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(tempStr, System.Web.Configuration.FormsAuthPasswordFormat.SHA1.ToString());
			//  tempStr = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(tempStr, "SHA1");

			if (tempStr.ToLower() == signature)
			{
				context.Response.Write(echostr);
				context.Response.End();
			}
		}

		///<summary>
		///处理微信服务器推送数据
		///</summary>
		private void WeChatRequest(HttpContext context)
		{
			//do something
		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}