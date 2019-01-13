/*
 * 由SharpDevelop创建。
 * 用户： nowind
 * 日期: 2017/12/16
 * 时间: 0:14
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */

using System.Threading;
using System.Windows.Forms;
using log4net;
namespace FilmRob
{

public class AsyRichEdit
	{
	private System.Windows.Forms.RichTextBox rtb=null;
	SynchronizationContext _syncContext = null;  
	log4net.ILog log = log4net.LogManager.GetLogger("myLogging");
	bool logf=false;
	string w="";
	public AsyRichEdit(RichTextBox rtb,bool f)
	{
		this.rtb=rtb;
		logf=f;
		_syncContext=SynchronizationContext.Current; 
	}
	public AsyRichEdit(RichTextBox rtb):this(rtb,true)
	{
		
	}
	public void Logw(string s)
	{
		w=w+s+"\n";
	}
	public void render()
	{
		Log(w);
		
	}
	public void Log(string s)
	{
			_syncContext.Post(addLog,s);
			if(logf)
			log.Info(s);
	}
	public void Clear()
	{
			_syncContext.Post(clear,null);
	}
	void addLog(object t)
	{
			if(rtb==null||rtb.Disposing||t==null||rtb.IsDisposed)return;
			rtb.AppendText(t.ToString()+"\n");
			w="";
	}
	 void clear(object t)
	{
			rtb.Clear();
			w="";
	}
}
}