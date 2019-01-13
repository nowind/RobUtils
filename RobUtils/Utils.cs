/*
 * 由SharpDevelop创建。
 * 用户： nowind
 * 日期: 2017/11/13
 * 时间: 0:46
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using EasyHttp.Http;
using JsonFx.Json;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;
namespace RobUtils
{
 public static class  Utils
{
	[DllImport("urlmon.dll", CharSet = CharSet.Ansi)] 
    private static extern int UrlMkSetSessionOption(int dwOption, string pBuffer, int dwBufferLength, int dwReserved); 
   private static  int URLMON_OPTION_USERAGENT = 0x10000001; 
    
	static public Dictionary<string,object>  CopyDicWithKey(Dictionary<string,object> a,string[] b)
	{
		var n=new Dictionary<string,object>();
		foreach(var i in b)
		{
			if(a.ContainsKey(i))
			{
				n.Add(i,a[i]);
			}
		}
		return n;
	}
	public static T GetPrivateField<T>(object instance, string fieldname)
{
    BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
    Type type = instance.GetType();
    FieldInfo field = type.GetField(fieldname, flag);
    return (T)field.GetValue(instance);
}
	
	 public static  HttpWebResponse getRawResponse(HttpResponse hrsp)
		
	{
	 	return GetPrivateField<HttpWebResponse>(hrsp,"_response");
	}
	  public static void ChangeUserAgent(string userAgent) 
    { 
      UrlMkSetSessionOption(URLMON_OPTION_USERAGENT, userAgent, userAgent.Length, 0); 
    }
	 public static bool NowPass(int Hour,int Min,int Sec) 
    { 
     DateTime n=DateTime.Now;
     int day=n.Day;
     if(Hour<0)return true;
     if(n.Hour>12&&Hour<5)day++;
	 DateTime sTime=new DateTime(n.Year,n.Month,day,Hour,Min,Sec);
	 int d=n.CompareTo(sTime);
	 return d>=0;
    }
	 public static string now()
	 {
	 	DateTime n=DateTime.Now;
	 	return string.Format("{0}:{1}",n.Hour,n.Minute);
	 }
	 public static void Message(object m)
	 {
	 	System.Windows.Forms.MessageBox.Show(m.ToString());
	 }
	 public static void UiMessage(SynchronizationContext c,string msg)
	 {
	 	c.Post(Message,msg);
	 }
	 public static Dictionary<string,string> iniSection2map(IniFile f,string section)
	 {
	 	var itms=new Dictionary<string,string>();
	 	IniSection inis;
			
			if(f.TryGetSection(section,out inis))
			{
				itms.Clear();
				foreach (var element in inis) {
					itms.Add(element.Key,element.Value.Value);
				}
				return itms;
			}
			return null;
	 }
}
}