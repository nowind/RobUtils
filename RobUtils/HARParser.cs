/*
 * 由SharpDevelop创建。
 * 用户： nowind
 * 日期: 2018/9/16
 * 时间: 1:08
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Text;
using System.Web;
using EasyHttp.Http;

namespace RobUtils
{
	/// <summary>
	/// Description of XHRParse.
	/// </summary>
	public class HARParser
	{
		public IDictionary<string,List<string>> Headers {get;set;}
		public string Method {get;set;}
		public string Url {get;set;}
		public string Body {get;set;}
		public string PostData {get;set;}
		public string Mime {get;set;}
		public IDictionary<string,string> postParmas {get;set;}
		public bool OK {get;set;}
		public string Err {get;set;}
		public IDictionary<string,string> queryString {get;set;}
		private static Dictionary<string,dynamic> cache=new Dictionary<string,dynamic>();
		
		
		
		public static HARParser quickLoad()
		{
			return quickLoad(0);
		}
		
		public static HARParser quickLoad(int no)
		{
			const string path = @"d:\tmp\1.har";// one path for quick read
			if(File.Exists(path))
			{
				var result=new HARParser(path,no);
				if(result.OK)
					return result;
			}
			return null;
		}
		public static string reGenPayload(IDictionary<string,string> postParmas,Encoding enc)
		{
			var sb=new StringBuilder();
			foreach (var e in postParmas) {
							sb.Append(HttpUtility.UrlEncode(e.Key,enc));
							sb.Append("=");
							sb.Append(HttpUtility.UrlEncode(e.Value,enc));
							sb.Append("&");
						}
			return sb.ToString(0,sb.Length-1);
		}
		public string reGenUrl(IDictionary<string,string> postParmas,Encoding enc)
		{
			var simpUrl=Url.Split('?')[0];
			return simpUrl+"?"+reGenPayload(postParmas,enc);
		}
		public  string reGenUrl(IDictionary<string,string> postParmas)
		{
			return reGenUrl(postParmas,Encoding.UTF8);
		}
		public static string reGenPayload(IDictionary<string,string> postParmas)
		{
			return reGenPayload(postParmas,Encoding.UTF8);
		}
		
		public HARParser(string pathorstr):this(pathorstr,0)
		{
		}
		public HARParser(string pathorstr,int no)
		{
			string harjson=pathorstr;
			if(pathorstr==null)return;
			if(!pathorstr.StartsWith("{"))
			{
				harjson=System.IO.File.ReadAllText(pathorstr);
			}
			Headers=new Dictionary<string,List<string>>();
			dynamic result;
			var json=new JsonFx.Json.JsonReader();
			if(!cache.ContainsKey(harjson))
			{
				result= json.Read(harjson);
				cache[harjson]=result;
			}
			else
				result=cache[harjson];
			try
			{
				if(no>=result.log.entries.Length)return;
				dynamic ent= result.log.entries[no].request;
				IDictionary<string,dynamic> _ent=ent;
				Method=ent.method;
				Url=ent.url;
				Mime="";
				foreach (dynamic e in ent.headers) {
					if(Headers.ContainsKey(e.name))
					{
						Headers[e.name].Add(e.value);
					}
					else
					{
						List<string> data=new List<string>();
						data.Add(e.value);
						Headers.Add(e.name,data);
					}
				}
				queryString=new Dictionary<string, string>();
				if(_ent.ContainsKey("queryString"))
				{
					var _q=(dynamic[])_ent["queryString"];
					foreach (var e in _q) {
						queryString.Add(e.name,e.value);
					}
					//queryString=(IDictionary<string,string>)_ent["queryString"];
				}
				if(_ent.ContainsKey("postData"))
				{
					Mime=ent.postData.mimeType;
					if(Mime!=null)
					{
						if((!Mime.Contains("x-www-form-urlencoded")))
						{
							PostData=ent.postData.text;
						}
						else
						{
							IDictionary<string,dynamic>  pd=ent.postData;
							dynamic pParmas=pd["params"];
							postParmas=new Dictionary<string,string >();
							foreach (var e in pParmas) {
								postParmas.Add(e.name,e.value);
							}
							PostData=reGenPayload(postParmas);
							
						}
					}
				}
			}
			catch(Exception ex)
			{
				OK=false;
				Err=ex.Message;
				return;
			}
			OK=true;
			
		}
		public HttpClient genEnv()
		{
			return genEnv(null,null);
		}
		public HttpClient genEnv(HttpClient hc)
		{
			return genEnv(hc,null);
		}
		public HttpClient genEnv(string[] genHeader)
		{
			return genEnv(null,genHeader);
		}
		public void resetCookie(string cookie)
		{
			if(Headers.ContainsKey("Cookie"))
			{
				Headers["Cookie"]=new List<string>(){cookie};
			}
		}
		public HttpClient genEnv(HttpClient hc,string[] genHeader)
		{
			if(hc==null)hc=new HttpClient();
			var l=new List<string>(){"Cookie","User-Agent","Accept","Referer","Accept-Language","Accept-Encoding","Origin","X-Requested-With"};
			if(genHeader!=null){
				l.AddRange(genHeader);
			}
			foreach (string e in l) {
				
				if(!Headers.ContainsKey(e))
				{
					continue;
				}
				else if(e=="User-Agent")
				{
					hc.Request.UserAgent=Headers[e][0];
					continue;
				}
				else if(e=="Referer")
				{
					hc.Request.Referer=Headers[e][0];
					continue;
				}
				else if(e=="Accept")
				{
					hc.Request.Accept=Headers[e][0];
					continue;
				}
				else {
					foreach (string e1 in Headers[e]) {
						hc.Request.AddExtraHeader(e,e1);
					}
				}
				
			}
			return hc;
		}
		public HttpClient repeat(HttpClient hc)
		{
			return repeat(hc,null);
		}
		
		public HttpClient repeat(HttpClient hc,IDictionary<string,string> dic)
		{
			try{
			if(Method.ToLower().Contains("post"))
			{
				String PostData=this.PostData;
				if(dic!=null)PostData=reGenPayload(dic);
				hc.Post(Url,PostData,Mime);
			}
			else if(Method.ToLower().Contains("get"))
			{
				String Url=this.Url;
				if(dic!=null)Url=reGenUrl(dic);
				hc.Get(Url);
			}
			}
			catch(Exception ex){}
			return hc;
		}
	}
}
