//================================================================================
//  FileName: TestForm.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2013-3-27
//================================================================================
//  Change History
//================================================================================
//  Date  Author  Description
//  ----    ------  -----------------
//
//================================================================================
//  Copyright (C) 2004-2009 Jeason Young Corporation
//================================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace iPower.WinService.Test
{
    /// <summary>
    /// Windows服务测试工具界面。
    /// </summary>
    public partial class TestForm : Form
    {
        #region 成员变量，构造函数。
        private IWinShellTest shell;
        private string[] args;
        private DateTime dtStart = DateTime.Now;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public TestForm(IWinShellTest shell, string[] args)
        {
            this.shell = shell;
            this.args = args;
            InitializeComponent();
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public TestForm()
            : this(null, null)
        {
        }
        #endregion
        
        #region 事件处理。
        private void TestForm_Load(object sender, EventArgs e)
        {
            this.btnStart.Enabled = (this.shell != null);
            this.btnStop.Enabled = this.btnPause.Enabled = false;
            this.lbRunTime.Text = string.Empty;
            this.updateTime();
            if (this.shell != null)
            {
                this.Text += "[" + this.shell.ServiceName + "]";
                this.shell.Changed += new ChangedHandler(delegate(string content)
                {
                    this.showMessage(content);
                });
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!this.btnStop.Enabled)
            {
                this.dtStart = DateTime.Now;
                this.timer.Start();
                this.shell.IsOnce = this.chkOnce.Checked;
                this.shell.OnStart(this.args);
                this.btnStart.Enabled = false;
                this.btnPause.Enabled = this.btnStop.Enabled = true;
            }
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (string.Equals(this.btnPause.Text, "暂停", StringComparison.InvariantCultureIgnoreCase))
            {
                //暂停。
                this.timer.Stop();
                this.shell.OnPause();
            }
            else
            {
                //继续。
                this.dtStart = DateTime.Now;
                this.timer.Start();
                this.shell.OnContinue();
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (!this.btnStart.Enabled)
            {
                this.timer.Stop();
                this.shell.OnStop();
                this.btnStop.Enabled = false;
                this.btnStart.Enabled = true;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            this.updateTime();
        }

        private void chkOnce_CheckedChanged(object sender, EventArgs e)
        {
            if (this.shell != null)
            {
                this.shell.IsOnce = this.chkOnce.Checked;
            }
        }
        #endregion

        #region 辅助函数。
        /// <summary>
        /// 显示消息。
        /// </summary>
        /// <param name="message"></param>
        private void showMessage(string message)
        {
            this.ThreadSafeMethod(this.richTextBoxMessage, new MethodInvoker(delegate()
            {
                this.richTextBoxMessage.Text = message + "\n" + this.richTextBoxMessage.Text;
                this.richTextBoxMessage.Update();
            }));
        }
        /// <summary>
        /// 更新时间。
        /// </summary>
        private void updateTime()
        {
            this.ThreadSafeMethod(this.lbTimer, new MethodInvoker(delegate()
            {
                this.lbTimer.Text = string.Format("{0:HH:mm:ss}", DateTime.Now);
                this.lbTimer.Update();
            }));
            this.ThreadSafeMethod(this.lbRunTime, new MethodInvoker(delegate()
            {
                TimeSpan ts = DateTime.Now - this.dtStart;
                this.lbRunTime.Text = string.Format("{0:##,###,###}", ts.TotalSeconds);
                this.lbRunTime.Update();
            }));
        }
        /// <summary>
        /// 线程安全方法调用。
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="method"></param>
        private void ThreadSafeMethod(Control ctrl, MethodInvoker method)
        {
            if (ctrl != null && method != null)
            {
                if (ctrl.InvokeRequired)
                {
                    ctrl.Invoke(method);
                }
                else
                {
                    method.Invoke();
                }
            }
        }
        /// <summary>
        /// 线程安全方法调用。
        /// </summary>
        /// <param name="method"></param>
        private void ThreadSafeMethod(MethodInvoker method)
        {
            this.ThreadSafeMethod(this, method);
        }
        #endregion
    }
}