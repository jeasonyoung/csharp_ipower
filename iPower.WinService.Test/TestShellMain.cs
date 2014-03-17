//================================================================================
//  FileName: TestShell.cs
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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Timers;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

using iPower.Logs;
using iPower.WinService;
using iPower.WinService.Config;
using iPower.WinService.Jobs;
using iPower.WinService.Logs;
namespace iPower.WinService.Test
{
    /// <summary>
    /// 测试主函数。
    /// </summary>
    public class TestShellMain : IWinShellTest, IDisposable
    {
        #region 成员变量，构造函数。
        private Hashtable allJobsCache = null;
        private WinServiceLog servlog = null;
        private string[] args = null; 
        private System.Timers.Timer mTimer; 
        private bool isRunTimer = true;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public TestShellMain()
        {
            try
            {
                //服务名称。
                this.ServiceName = ServiceConfig.ModuleConfig.ServiceName;
                //服务日志初始化。
                this.servlog = new WinServiceLog(ServiceConfig.ModuleConfig);
                this.servlog.LogRecordEvent += this.LogRecord;
                //初始化服务任务集合。
                this.allJobsCache = Hashtable.Synchronized(new Hashtable());
            }
            catch (Exception ex)
            {
                //错误日志记录。
                this.servlog.ErrorLog(ex);
            }
            finally
            {
                //定时器初始化。
                this.mTimer = new System.Timers.Timer();
                this.mTimer.AutoReset = true;
                this.mTimer.Enabled = false;
                this.mTimer.Interval = ModuleConst.CONST_DOU_TIMEINTERVAL;
                this.mTimer.Elapsed += this.OnTimerElapsed;
            }
        }
        #endregion

        #region 入口函数。
        /// <summary>
        /// 主程序入口。
        /// </summary>
        /// <param name="args"></param>
        [STAThread]
        public static void Main(params string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TestForm(new TestShellMain(), args));
        }
        #endregion

        #region 辅助函数。
        /// <summary>
        /// 日志记录。
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="logContent"></param>
        private void LogRecord(EnumLogsType logType, string logContent)
        {
            this.OnChanged(string.Format("[{0:yyyy-MM-dd HH:mm:ss.fff}]{1}#{2}", DateTime.Now, logType, logContent));
        }
        /// <summary>
        /// 加载WinServiceJob。
        /// </summary>
        private void LoadJobs()
        {
            lock (this)
            {
                this.allJobsCache.Clear();
                List<String> configJobs = ServiceJobsConfig.ModuleConfig.AllJobs;
                if (configJobs != null && configJobs.Count > 0)
                {
                    foreach (String jobKey in configJobs)
                    {
                        try
                        {
                            this.OnChanged("任务[" + jobKey + "],状态:" + "启用");
                            if (!string.IsNullOrEmpty(jobKey))
                            {
                                //获取配置。
                                this.OnChanged("准备加载任务[" + jobKey + "]的配置...");
                                JobConfiguration configuration = new JobConfiguration(jobKey);
                                if (configuration != null && !string.IsNullOrEmpty(configuration.ModuleAssembly) && !string.IsNullOrEmpty(configuration.ModuleClassName))
                                {
                                    this.OnChanged(string.Format("加载任务程序集[{0}]，周期：{1}，同步时间点：{2}", configuration.ModuleAssembly, configuration.CycleTicks, configuration.RunTimeTicks));
                                    string moduleAssembly = configuration.ModuleAssembly;
                                    Assembly executingAssembly = null;
                                    if (Assembly.GetExecutingAssembly().FullName.StartsWith(moduleAssembly))
                                    {
                                        executingAssembly = Assembly.GetExecutingAssembly();
                                    }
                                    else
                                    {
                                        executingAssembly = Assembly.Load(moduleAssembly);
                                    }
                                    //加载程序集。
                                    if (executingAssembly != null)
                                    {
                                        IJob wjob = executingAssembly.CreateInstance(configuration.ModuleClassName) as IJob;
                                        if (wjob != null)
                                        {
                                            wjob.LogChanged += this.LogRecord;
                                            this.allJobsCache[jobKey] = wjob;
                                            this.OnChanged("配置加载完毕！");
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception x)
                        {
                            this.servlog.ErrorLog(x);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 启动任务。
        /// </summary>
        /// <param name="args"></param>
        private void startJobs(string[] args)
        {
            if (this.allJobsCache != null && this.allJobsCache.Count > 0)
            {
                foreach (DictionaryEntry de in this.allJobsCache)
                {
                    if (de.Value != null && (de.Key is IJob))
                    {
                        try
                        {
                            this.OnChanged("开始启动任务[" + de.Key + "]...");
                            ((IJob)de.Value).Start(args);
                            this.OnChanged("启动成功");
                        }
                        catch (Exception e)
                        {
                            this.OnChanged("启动失败");
                            this.servlog.ErrorLog(e);
                        }
                    }
                }
                //启动定时器。
                this.mTimer.Start();
            }
        }
        /// <summary>
        /// 定时函数。
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void OnTimerElapsed(object s, ElapsedEventArgs e)
        {
            lock (this)
            {
                try
                {
                    //停止定时器。
                    this.mTimer.Stop();
                    if (this.allJobsCache != null && this.allJobsCache.Count > 0)
                    {
                        foreach (DictionaryEntry de in this.allJobsCache)
                        {
                            try
                            {
                                IJob job = de.Value as IJob;
                                if (job != null && job.CanRun())
                                {
                                    this.OnChanged("时间间隔到达，任务[" + de.Key + "]开始执行...");
                                    new Thread(new ThreadStart(delegate()
                                    {
                                        job.Run();
                                        this.OnChanged("任务[" + de.Key + "]执行完毕！");
                                    })).Start();
                                    this.isRunTimer = !this.IsOnce;
                                }
                            }
                            catch (Exception x)
                            {
                                this.servlog.ErrorLog(x);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.servlog.ErrorLog(ex);
                }
                finally
                {
                    if (this.isRunTimer)
                    {
                        //启动定时器。
                        this.mTimer.Start();
                    }
                }
            }
        }
        #endregion

        #region IWinShellTest 成员
        /// <summary>
        /// 获取或设置服务名称。
        /// </summary>
        public string ServiceName
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsOnce
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public event ChangedHandler Changed;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        protected void OnChanged(string content)
        {
            ChangedHandler handler = this.Changed;
            if (handler != null)
            {
                handler(content);
            }
        }
        /// <summary>
        /// 启动服务。
        /// </summary>
        /// <param name="args"></param>
        public void OnStart(string[] args)
        {
            lock (this)
            {
                this.OnChanged("准备启动服务[" + this.ServiceName + "]...");
                //加载任务配置。
                this.LoadJobs();
                //启动任务。
                this.startJobs(args);
                this.OnChanged("启动完毕,等待任务时间间隔周期...");
            }
        }
        /// <summary>
        /// 服务继续。
        /// </summary>
        public void OnContinue()
        {
            this.OnChanged("继续服务[" + this.ServiceName + "]");
            this.startJobs(this.args);
        }
        /// <summary>
        /// 服务暂停。
        /// </summary>
        public void OnPause()
        {
            lock (this)
            {
                //停止定时器。
                this.mTimer.Stop();
                #region 服务暂停。
                this.OnChanged("准备停止服务[" + this.ServiceName + "]...");
                if (this.allJobsCache != null && this.allJobsCache.Count > 0)
                {
                    foreach (DictionaryEntry de in this.allJobsCache)
                    {
                        if (de.Value != null && (de.Value is IJob))
                        {
                            try
                            {
                                this.OnChanged("任务[" + de.Key + "]开始暂停...");
                                ((IJob)de.Value).Pause();
                                this.OnChanged("任务[" + de.Key + "]完成暂停!");
                            }
                            catch (Exception x)
                            {
                                this.servlog.ErrorLog(x);
                            }
                        }
                    }
                }
                this.OnChanged("停止服务[" + this.ServiceName + "]完成");
                #endregion
            }
        }
        /// <summary>
        /// 停止服务。
        /// </summary>
        public void OnStop()
        {
            lock (this)
            {
                this.OnChanged("准备停止服务[" + this.ServiceName + "]...");
                //停止定时器。
                this.mTimer.Stop();
                #region 服务停止。
                if (this.allJobsCache != null && this.allJobsCache.Count > 0)
                {
                    foreach (DictionaryEntry de in this.allJobsCache)
                    {
                        if (de.Value != null && (de.Value is IJob))
                        {
                            try
                            {
                                this.OnChanged("任务[" + de.Key + "]开始停止...");
                                ((IJob)de.Value).Stop();
                                this.OnChanged("任务[" + de.Key + "]完成停止!");
                            }
                            catch (Exception x)
                            {
                                this.servlog.ErrorLog(x);
                            }
                        }
                    }
                }
                #endregion
                this.OnChanged("服务停止!");
            }
        }
        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            if (this.mTimer != null)
            {
                this.mTimer.Stop();
                this.mTimer.Elapsed -= this.OnTimerElapsed;
                this.mTimer.Dispose();
                this.mTimer = null;
            }
        }

        #endregion
    }
}