/*
 * 由SharpDevelop创建。
 * 用户： hzxudi
 * 日期: 2018/10/24
 * 时间: 12:39
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using EasyHttp.Http;

namespace RobUtils
{
	/// <summary>
	/// Description of VerCodeBox.
	/// </summary>
	public class VerCodeBox
	{
		PictureBox pb=null;
		public string msg="";
		MemoryStream ms=null;
		public VerCodeBox(PictureBox p)
		{
			pb=p;
		}
		static public  HttpClient preHttp(string cookie)
		{
			HttpClient hc=new HttpClient();
			//hc.Request.PersistCookies=true;
			//hc.StreamResponse=true;
			hc.Request.AddExtraHeader("Cookie",cookie);
			return hc;
		}
		public HttpClient show(string url,string cookie)
		{
			HttpClient hc=preHttp(cookie);
			return show(url,hc);
		}
		public HttpClient show(string url,HttpClient hc)
		{
			hc.StreamResponse=true;
			try{
			hc.Get(url);
			show(hc);
			}
			catch(Exception ex)
			{
				msg=ex.Message;
			}
			return hc;
		}
		public MemoryStream show(HttpClient hc)
		{
			ms=new MemoryStream();
			byte[] buffer = new byte[1024];  
  
        	int actual = 0;  
        	while ((actual = hc.Response.ResponseStream.Read(buffer, 0, 1024)) > 0)  
        	{  
            	ms.Write(buffer, 0, actual);  
        	}
        	if(pb!=null)
        		pb.Image=Image.FromStream(ms);
        	return ms;
		}
		public MemoryStream getImgMem()
		{
			return ms;
		}
		public void save(string path)
		{
			File.WriteAllBytes(path,ms.ToArray());
		}
	}
}
