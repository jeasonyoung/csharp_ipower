//================================================================================
//  FileName: Program.cs
//  Desc: Windows服务壳程序入口类。
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
using System.ServiceProcess;
using System.Configuration.Install;
using System.Text;
using System.Reflection;

using iPower.WinService.Config;
using iPower.WinService.Logs;
namespace iPower.WinService.Shell
{
    /// <summary>
    /// Windows服务壳程序入口类。
    /// </summary>
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        /// <param name="args">命令行参数。</param>
        public static void Main(string[] args)
        {
            WinServiceLog servLog = null;
            try
            {
                servLog = new WinServiceLog(ServiceConfig.ModuleConfig);
                string svcName = ServiceConfig.ModuleConfig.ServiceName;
                bool startService = true;

                #region 安装与卸载服务.
                if (args != null && args.Length > 0)
                {
                    string assemblyName = Assembly.GetExecutingAssembly().ManifestModule.Name;
                    switch (args[0].ToLower())
                    {
                        case "-i":
                        case "-install":
                            {
                                #region 安装服务.
                                try
                                {
                                    servLog.ContentLog(string.Format("开始安装[{0}]服务...", svcName));
                                    if (ServiceIsExisted(svcName))
                                    {
                                        servLog.ContentLog(string.Format("服务[{0}]已经存在，无需重复安装。", svcName));
                                        break;
                                    }
                                    ManagedInstallerClass.InstallHelper(new string[] { assemblyName });
                                    ServiceController sc = new ServiceController(svcName);
                                    sc.Start();
                                    servLog.ContentLog(string.Format("安装服务[{0}]成功。", svcName));
                                    
                                    startService = true;
                                }
                                catch (Exception e)
                                {
                                    startService = false;
                                    servLog.ContentLog(string.Format("安装[{0}]服务失败[{1}]。", svcName, e.Message));
                                    servLog.ErrorLog(e);
                                }
                                #endregion
                            }
                            break;
                        case "-u":
                        case "-uninstall":
                            {
                                #region 卸载服务.
                                try
                                {
                                    if (ServiceIsExisted(svcName))
                                    {
                                        servLog.ContentLog(string.Format("开始卸载{0}服务...", svcName));
                                        ManagedInstallerClass.InstallHelper(new string[] { "/u", assemblyName });
                                        servLog.ContentLog(string.Format("卸载[{0}]服务成功。", svcName));
                                    }
                                    else
                                    {
                                        servLog.ContentLog(string.Format("服务{0}已经卸载。", svcName));
                                    }
                                   
                                }
                                catch (Exception e)
                                {
                                    servLog.ContentLog(string.Format("卸载[{0}]服务失败[{1}]。", svcName, e.Message));
                                    servLog.ErrorLog(e);
                                }
                                finally
                                {
                                    startService = false;
                                }
                                #endregion
                            }
                            break;
                        default:
                            {
                                servLog.ContentLog(string.Format("命令 {0} 不存在。", args));
                                startService = false;
                            }
                            break;
                    }
                }
                #endregion

                //运行服务.
                if (startService)
                {
                    servLog.ContentLog(string.Format("开始启动[{0}]服务...", svcName));
                    ServiceBase.Run(new ServiceBase[] { new ServiceMain(servLog) });
                    servLog.ContentLog(string.Format("[{0}]服务启动成功。", svcName));
                }
            }
            catch (Exception e)
            {
                if (servLog != null)
                {
                    servLog.ErrorLog(new Exception("Windows服务壳程序入口异常:" + e.Message, e));
                }
                else
                {
                    WriteExceptionLog(typeof(Program), new Exception("Windows服务壳程序入口异常:" + e.Message, e));
                }
            }
        }

        #region 辅助函数.
        /// <summary>
        /// 检查指定的服务是否存在。
        /// </summary>
        /// <param name="svcName">要查找的服务名字。</param>
        /// <returns></returns>
        private static bool ServiceIsExisted(string svcName)
        {
            ServiceController[] services = ServiceController.GetServices();
            if (services != null && services.Length > 0)
            {
                for (int i = 0; i < services.Length; i++)
                {
                    if ((services[i] != null) && (services[i].ServiceName == svcName))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 写入异常日志.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="e"></param>
        public static void WriteExceptionLog(Type type, Exception e)
        {
            if (type != null && e != null)
            {
                StringBuilder log = new StringBuilder();
                log.AppendLine("Message:" + e.Message)
                    .AppendLine("Source:" + e.Source)
                    .AppendLine("StackTrace:" + e.StackTrace);

                Exception ex = null;
                while ((ex = e.InnerException) != null)
                {
                    log.AppendLine(new string('-', 60))
                    .AppendLine("Message:" + ex.Message)
                    .AppendLine("Source:" + ex.Source)
                    .AppendLine("StackTrace:" + ex.StackTrace);
                }

                //
                WriteExceptionLog(type, log.ToString());
            }
        }
        /// <summary>
        /// 写入异常日志.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="content"></param>
        public static void WriteExceptionLog(Type type, string content)
        {
            if (type != null && !string.IsNullOrEmpty(content))
            {
                string path = string.Format("{0}/{1}_error_{2:yyyyMMddHHmmss}.log", AppDomain.CurrentDomain.BaseDirectory, type.FullName, DateTime.Now);
                using (System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                {
                    byte[] data = System.Text.UTF8Encoding.UTF8.GetBytes(content);
                    fs.Write(data, 0, data.Length);
                }
            }
        }
        #endregion
    }
}