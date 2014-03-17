//================================================================================
//  FileName: WinServiceJob.cs
//  Desc:Windows服务的Job抽象类。
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2010-1-19
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
using System.Text;
using System.Threading;
using iPower.Logs;
using iPower.WinService;
using iPower.WinService.Config;
using iPower.WinService.Logs;
namespace iPower.WinService.Jobs
{
    /// <summary>
    /// Windows服务的Job抽象类。
    /// </summary>
    public abstract class Job<T> : IJob
        where T : JobConfiguration
    {
        #region 成员变量，构造函数。
        private long prev_run_ticks = 0L, next_run_ticks = 0L;
        private JobStatus status = JobStatus.LoadSuccessfull;
        private string[] args = null;
        private WinServiceLog servLog, jobLog;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public Job()
        {
            this.servLog = new WinServiceLog(ServiceConfig.ModuleConfig);
            this.servLog.LogRecordEvent += this.OnLogChanged;
            this.jobLog = new WinServiceLog(this.JobConfig);
            this.jobLog.LogRecordEvent += this.OnLogChanged;
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取服务任务名称。
        /// </summary>
        public abstract string JobName { get; }
        /// <summary>
        /// 获取服务任务配置实例。
        /// </summary>
        protected abstract T JobConfig { get; }
        /// <summary>
        /// 获取服务执行函数。
        /// </summary>
        protected abstract IJobFunction JobFunction { get; }
        /// <summary>
        /// 获取服务日志。
        /// </summary>
        protected virtual WinServiceLog ServiceLog
        {
            get
            {
                return this.servLog;
            }
        }
        /// <summary>
        /// 获取任务日志。
        /// </summary>
        protected virtual WinServiceLog JobLog
        {
            get
            {
                return this.jobLog;
            }
        }
        #endregion

        #region IJob 成员
        /// <summary>
        /// 日志事件。
        /// </summary>
        public event LogChangedHandler LogChanged;
        /// <summary>
        /// 初始化启动任务。
        /// </summary>
        /// <param name="args"></param>
        public void Start(params string[] args)
        {
            try
            {
                this.args = args;
                this.next_run_ticks = this.createRunTicks();
                this.ServiceLog.StartSuccessfullLog(this.JobName);
                this.status = JobStatus.LoadSuccessfull;
            }
            catch (Exception e)
            {
                this.status = JobStatus.LoadFailure;
                this.ServiceLog.StartFailureLog(this.JobName);
                this.ServiceLog.ErrorLog(e);
            }
        }
        /// <summary>
        /// 暂停任务。
        /// </summary>
        public void Pause()
        {
            this.status = JobStatus.Stopped;
        }
        /// <summary>
        /// 停止任务。
        /// </summary>
        public void Stop()
        {
            this.status = JobStatus.Stopped;
        }
        /// <summary>
        /// 判断是否允许运行。
        /// </summary>
        /// <returns></returns>
        public bool CanRun()
        {
            if (!this.IsExpired() && (this.status == JobStatus.LoadSuccessfull || this.status == JobStatus.End || this.status == JobStatus.Stopped))
            {
                if (this.prev_run_ticks == 0)
                {
                    this.prev_run_ticks = DateTime.Now.Ticks;
                    return false;
                }
                if (this.prev_run_ticks > 0 && this.next_run_ticks > 0)
                {
                    return DateTime.Now.Ticks - this.next_run_ticks >= 0;
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// 运行任务。
        /// </summary>
        public void Run()
        {
            try
            {
                //设置状态。
                this.status = JobStatus.Running;
                //日志准备。
                this.ServiceLog.StartSuccessfullLog(string.Format("{0}(启动时间：{1:yyyy-MM-dd HH:mm:ss.fff})", this.JobName, DateTime.Now));
                //执行任务业务。
                this.RunJob(this.args);
                //日志记录。
                this.ServiceLog.RuuningLog(string.Format("任务{0}运行成功{1:yyyy-MM-dd HH:mm:ss}！", this.JobName, DateTime.Now));
                //重置状态。
                this.status = JobStatus.End;
            }
            catch (Exception x)
            {
                this.status = JobStatus.Error;
                this.ServiceLog.StartFailureLog(this.JobName);
                this.ServiceLog.ErrorLog(x);
            }
            finally
            {
                this.prev_run_ticks = DateTime.Now.Ticks;
                this.next_run_ticks = this.createRunTicks();
                this.ServiceLog.StopSuccessfullLog(string.Format("{0}(下次执行时间：{1:yyyy-MM-dd HH:mm:ss.fff})", this.JobName, new DateTime(this.next_run_ticks)));
            }
        }

        #endregion

        #region 辅助函数。
        /// <summary>
        /// 触发日志事件。
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="logContent"></param>
        private void OnLogChanged(EnumLogsType logType, string logContent)
        {
            LogChangedHandler handler = this.LogChanged;
            if (handler != null)
            {
                handler(logType, logContent);
            }
        }
        /// <summary>
        /// 是否过期。
        /// </summary>
        /// <returns></returns>
        private bool IsExpired()
        {
            bool result = false;
            long ticks = DateTime.Now.Ticks;
            if (result = ((ticks < this.JobConfig.StartTimeTicks) || (ticks > this.JobConfig.EndTimeTicks)))
            {
                this.status = JobStatus.Expired;
            }
            return result;
        }
        /// <summary>
        /// 创建任务的运行间隔。
        /// </summary>
        /// <returns></returns>
        protected virtual long createRunTicks()
        {
            long offset = 0L, ticks = DateTime.Now.Ticks + this.JobConfig.CycleTicks;
            if ((offset = this.JobConfig.RunTimeTicks) > 0)
            {
                ticks += offset;
            }
            return ticks;
        }
        /// <summary>
        /// 任务执行。
        /// </summary>
        /// <param name="args"></param>
        protected virtual void RunJob(params string[] args)
        {
            try
            {
                if (this.JobFunction != null)
                {
                    this.JobFunction.Run(args);
                }
            }
            catch (Exception e)
            {
                this.status = JobStatus.Error;
                this.jobLog.ErrorLog(e);
            }
        }
        #endregion
    }
}