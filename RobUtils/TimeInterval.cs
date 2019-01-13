/*
 * 由SharpDevelop创建。
 * 用户： nowind
 * 日期: 2018/8/24
 * 时间: 0:47
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Threading;

namespace RobUtils
{
	/// <summary>
	/// Description of TimeInterval.
	/// </summary>
	public class TimeInterval
	{
		public delegate int RunnerFunc(object a);
		public delegate void AOPFunc();
		public enum errTime{BeforeRun,Run};
		public delegate int ErrHandler(errTime time,Exception e,int count);
		public int sHour=10,sMin=0,sSec=0;
		public int intMicroSec=0;
		public int intMicroSecBefore=0;
		public int Max=9999999;
		protected RunnerFunc rf=null,bf=null;
		protected bool isStop=false,isFinsh=false;
		protected AOPFunc beg=null;
		protected AOPFunc end=null;
		protected ErrHandler eh=null;
		public TimeInterval(RunnerFunc r)
		{
			rf=r;
		}
		void RealRunnerFunc(object a)
		{
			try{
				const int maxErr=25;
				int ErrCount=maxErr,result=0;
				if(intMicroSecBefore<1)intMicroSecBefore=1000;
				while(!isStop)
				{
					if(Utils.NowPass(sHour,sMin,sSec))break;
					try{
					if(bf!=null)result=bf(a);
					if(result==0&&intMicroSecBefore>0)
						Thread.Sleep(intMicroSecBefore);
					else if(result>0)
						Thread.Sleep(result);
					}
					catch(Exception ex2)
					{
						--ErrCount;
						if(eh!=null)
						{
							if(eh(errTime.BeforeRun,ex2,maxErr-ErrCount)<0)break;
						}
						else if(ErrCount<0)break;
					}
				}
				if(isStop)return;
				if(beg!=null)beg();
				ErrCount=maxErr;
				for(int i=0;i<Max;i++)
				{
					if(isStop)break;
					try{
						result=rf(a);
						if(result<0)
						{
							isFinsh=true;
							isStop=true;
							break;
							
						}
					}
					catch(Exception ex1)
					{
						--ErrCount;
						if(eh!=null)
						{
							if(eh(errTime.Run,ex1,maxErr-ErrCount)<0)break;
						}
						else if(ErrCount<0)break;
					}
					if(result==0&&intMicroSec>0)
						Thread.Sleep(intMicroSec);
					else if(result>0)
						Thread.Sleep(result);
				}
				
			}
			catch(Exception ex){}
			if(end!=null)end();
		}
		public void run(object a)
		{
			isStop=false;
			isFinsh=false;
			ThreadPool.QueueUserWorkItem(new WaitCallback(RealRunnerFunc),a);
		}
		public void run()
		{
			run(null);
		}
		public void stop()
		{
			isStop=true;
			isFinsh=true;
		}
		public void setBeg(AOPFunc a)
		{
			beg=a;
		}
		public void setEnd(AOPFunc a)
		{
			end=a;
		}
		public void setErrHandler(ErrHandler a)
		{
			eh=a;
		}
		public void setBeforeRun(RunnerFunc a)
		{
			bf=a;
		}
		public bool isOK()
		{
			return isFinsh;
		}
	}
}
