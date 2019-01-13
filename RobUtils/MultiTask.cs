/*
 * 由SharpDevelop创建。
 * 用户： nowind
 * 日期: 2018/12/3
 * 时间: 21:20
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections.Generic;

namespace RobUtils
{
	/// <summary>
	/// Description of MultiTask.
	/// </summary>
	public class MultiTask 
	{
		List<TimeInterval> pool=null;
		List<object> data=null;
		TimeInterval.RunnerFunc real=null;
		public int sHour=10,sMin=0,sSec=0;
		public int intMicroSec=0;
		public int intMicroSecBefore=0;
		public int Max=9999999;
		protected bool isStop=false,isFinsh=false;
		TimeInterval main=null;
		int MutilRf(object o)
		{
			foreach (var element in pool) {
				if(!element.isOK())return 0;
			}
			return -1;
		}
		public MultiTask(TimeInterval.RunnerFunc rf)
		{
			real=rf;
			main=new TimeInterval(MutilRf);
		}
		public void run(ICollection<object> cols)
		{
			data=new List<object>(cols);
			pool=new List<TimeInterval>(data.Count);
			for (var i=0;i<data.Count;i++) {
				var e=new TimeInterval(real);
				e.sHour=sHour;
				e.sMin=sMin;
				e.sSec=sSec;
				e.Max=Max;
				e.intMicroSec=intMicroSec;
				pool.Add(e);
				e.run(data[i]);
			}
			main.sHour=sHour;
			main.sMin=sMin;
			main.sSec=sSec;
			main.Max=Max;
			main.intMicroSec=intMicroSec;
			main.intMicroSecBefore=intMicroSecBefore;
			main.run();
		}
		public void stop()
		{
			foreach (var element in pool) {
				element.stop();
			}
			main.stop();
		}
		public void setBeforeRun(TimeInterval.RunnerFunc a)
		{
			main.setBeforeRun(a);
		}
		public void setEnd(TimeInterval.AOPFunc a)
		{
			main.setEnd(a);
		}
		public void setErrHandler(TimeInterval.ErrHandler a)
		{
			main.setErrHandler(a);
		}
		public void setBeg(TimeInterval.AOPFunc a)
		{
			main.setBeg(a);
		}
	}
}
