//================================================================================
//  FileName: ServiceMain.cs
//  Desc:Window服务主类。
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2010/1/17
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
using System.ComponentModel;
using System.ServiceProcess;

using iPower.WinService;
using iPower.WinService.Config;
using iPower.WinService.Jobs;
using iPower.WinService.Logs;
namespace iPower.WinService.Shell
{
    /// <summary>
    /// Window服务主类。
    /// </summary>
    public sealed class ServiceMain : ServiceBase
    {
        #region 成员变量，构造函数.
        /// <summary>
        /// 服务任务缓存集合.
        /// </summary>
        private Hashtable allJobsCache = Hashtable.Synchronized(new Hashtable());
        /// <summary>
        /// 服务主定时器.
        /// </summary>
        private System.Timers.Timer timer;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public ServiceMain(WinServiceLog servlog)
            : base()
        {
            try
            {
                this.ServiceLog = servlog;
                this.Runing = false;
                //服务名称。
                this.ServiceName = ServiceConfig.ModuleConfig.ServiceName;
                //初始化主定时器.
                this.timer = new System.Timers.Timer(ModuleConst.CONST_DOU_TIMEINTERVAL);
                this.timer.Elapsed += this.ServiceTimerMain;
                this.timer.Enabled = false;
            }
            catch (Exception e)
            {
                servlog.ErrorLog(e);
            }
        }
        #endregion

        #region 属性.
        /// <summary>
        /// 获取服务器日志对象.
        /// </summary>
        private WinServiceLog ServiceLog { get; set; }
        /// <summary>
        /// 获取启动参数.
        /// </summary>
        private string[] Args { get; set; }
        /// <summary>
        /// 获取是否运行状态.
        /// </summary>
        private bool Runing { get; set; }
        #endregion

        #region 辅助函数。
        /// <summary>
        /// 加载WinServiceJob配置。
        /// </summary>
        private void LoadJobsConfigruation()
        {
            this.allJobsCache.Clear();
            List<String> configJobs = ServiceJobsConfig.ModuleConfig.AllJobs;
            if (configJobs != null && configJobs.Count > 0)
            {
                foreach (String jobKey in configJobs)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(jobKey))
                        {
                            //获取Job配置。
                            JobConfiguration configuration = new JobConfiguration(jobKey);
                            if (configuration != null && !string.IsNullOrEmpty(configuration.ModuleAssembly)
                                && !string.IsNullOrEmpty(configuration.ModuleClassName))
                            {
                                string moduleAssembly = configuration.ModuleAssembly;
                                Assembly executingAssembly = null;
                                if (Assembly.GetExecutingAssembly().FullName.StartsWith(moduleAssembly))
                                    executingAssembly = Assembly.GetExecutingAssembly();
                                else
                                    executingAssembly = Assembly.Load(moduleAssembly);
                                //加载程序集。
                                if (executingAssembly != null)
                                {
                                    IJob job = executingAssembly.CreateInstance(configuration.ModuleClassName) as IJob;
                                    if (job != null)
                                    {
                                        this.allJobsCache[jobKey] = job;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        this.ServiceLog.ErrorLog(new Exception("加载[" + jobKey + "]配置时发生异常.", e));
                    }
                }
            }
        }
        /// <summary>
        /// 服务定时器主体.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServiceTimerMain(object sender, ElapsedEventArgs e)
        {
            try
            {
                //定时器暂停.
                this.timer.Stop();
                //执行服务任务调用.
                if (this.allJobsCache.Count > 0)
                {
                    string[] args = this.Args;
                    foreach (DictionaryEntry de in this.allJobsCache)
                    {
                        this.ExcuteServiceJob(de, args);
                    }
                }
            }
            catch (Exception ex)
            {
                this.ServiceLog.ErrorLog(new Exception("主定时器发生异常:" + ex.Message, ex));
            }
            finally
            {
                if (this.Runing)
                {
                    //重新启动定时器.
                    this.timer.Start();
                }
            }
        }
        /// <summary>
        /// 调度执行服务任务.
        /// </summary>
        /// <param name="de"></param>
        /// <param name="args"></param>
        private void ExcuteServiceJob(DictionaryEntry de, string[] args)
        {
            try
            {
                IJob job = de.Value as IJob;
                if (job != null && job.CanRun())
                {
                    //日志输出.
                    this.ServiceLog.CreateNormalLog(string.Format("准备启动任务[{0}({1})][{2:yyyyMMdd-HH/mm/ss.fff}]", de.Key, job.JobName, DateTime.Now));
                    //启动任务线程.
                    ThreadPool.QueueUserWorkItem(new WaitCallback(delegate(object sender)
                    {
                        try
                        {
                            //启动.
                            job.Start(args);
                            //日志输出.
                            this.ServiceLog.CreateNormalLog(string.Format("启动任务[{0}({1})]线程[{2},{3:yyyyMMdd-HH/mm/ss.fff}]",
                                de.Key,
                                job.JobName,
                                Thread.CurrentThread.ManagedThreadId,
                                DateTime.Now));
                            //执行.
                            job.Run();
                        }
                        catch (Exception e)
                        {
                            this.ServiceLog.ErrorLog(new Exception(string.Format("任务[{0}({1})]执行时发生异常:{2}", de.Key, job.JobName, e.Message), e));
                        }
                        finally
                        {
                            this.ServiceLog.CreateNormalLog(string.Format("任务[{0}({1})]线程[{2},{3:yyyyMMdd-HH/mm/ss.fff}]本次执行结束",
                                de.Key, job.JobName,
                                Thread.CurrentThread.ManagedThreadId,
                                DateTime.Now));
                        }
                    }));
                }
            }
            catch (Exception e)
            {
                this.ServiceLog.ErrorLog(new Exception("调度任务[" + de.Key + "]时发生异常:" + e.Message, e));
            }
        }
        /// <summary>
        /// 启动服务任务调度.
        /// </summary>
        private void StartJobs()
        {
            try
            {
                this.Runing = true;
                this.timer.Start();
            }
            catch (Exception e)
            {
                this.Runing = false;
                this.ServiceLog.ErrorLog(new Exception("启动服务任务主定时器失败:" + e.Message, e));
            }
        }
        #endregion

        #region 重载。
        /// <summary>
        /// 启动服务。
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            try
            {
                //启动服务。
                base.OnStart(this.Args = args);
                //加载服务任务配置。
                this.LoadJobsConfigruation();
                //启动任务。
                this.StartJobs();
            }
            catch (Exception e)
            {
                this.ServiceLog.ErrorLog(new Exception("启动服务异常:" + e.Message, e));
            }
        }
        /// <summary>
        /// 停止服务。
        /// </summary>
        protected override void OnStop()
        {
            try
            {
                this.Runing = false;
                #region 服务停止。
                if (this.allJobsCache.Count > 0)
                {
                    foreach (DictionaryEntry de in this.allJobsCache)
                    {
                        IJob job = de.Value as IJob;
                        if (job != null)
                        {
                            try
                            {
                                if (!job.CanRun())
                                {
                                    job.Stop();
                                }
                            }
                            catch (Exception e)
                            {
                                this.ServiceLog.ErrorLog(new Exception(string.Format("服务[{0}]停止时发生异常.", de.Key), e));
                            }
                        }
                    }
                }
                #endregion
            }
            catch (Exception e)
            {
                this.ServiceLog.ErrorLog(new Exception("停止服务异常:" + e.Message, e));
            }
            finally
            {
                //停止服务.
                base.OnStop();
            }
        }
        /// <summary>
        /// 暂停服务。
        /// </summary>
        protected override void OnPause()
        {
            try
            {
                this.Runing = false;
                #region 服务暂停。
                if (this.allJobsCache.Count > 0)
                {
                    foreach (DictionaryEntry de in this.allJobsCache)
                    {
                        IJob job = de.Value as IJob;
                        if (job != null)
                        {
                            try
                            {
                                if (!job.CanRun())
                                {
                                    job.Pause();
                                }
                            }
                            catch (Exception e)
                            {
                                this.ServiceLog.ErrorLog(new Exception(string.Format("服务[{0}]暂停时发生异常.", de.Key), e));
                            }
                        }
                    }
                }
                #endregion
            }
            catch (Exception e)
            {
                this.ServiceLog.ErrorLog(new Exception("暂停服务异常:" + e.Message, e));
            }
            finally
            {
                //暂停服务.
                base.OnPause();
            }
        }
        /// <summary>
        /// 继续服务。
        /// </summary>
        protected override void OnContinue()
        {
            try
            {
                //继续服务。
                base.OnContinue();
                //加载服务任务配置.
                this.LoadJobsConfigruation();
                //启动服务任务.
                this.StartJobs();
            }
            catch (Exception e)
            {
                this.ServiceLog.ErrorLog(new Exception("继续服务异常:" + e.Message, e));
            }
        }
        #endregion
    }
}