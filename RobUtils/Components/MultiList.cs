/*
 * 由SharpDevelop创建。
 * 用户： hzxudi
 * 日期: 2018/12/3
 * 时间: 21:40
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RobUtils
{
	/// <summary>
	/// Description of MultiList.
	/// </summary>
	public class MultiList
	{
		CheckedListBox clb;
		public MultiList(CheckedListBox clb)
		{
			this.clb=clb;
		}
		public void set(ICollection<string> col)
		{
			clb.Items.Clear();
			clb.Items.AddRange(new List<string>(col).ToArray());
		}
		public ICollection<object> getSel()
		{
			var l=new List<object>();
			foreach (var element in clb.CheckedItems) {
				l.Add(element);
			}
			return l;
		}
		
	}
}
