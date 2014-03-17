//================================================================================
//  FileName: WinServiceInstaller.cs
//  Desc:Widows服务安装配置类。
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
using System.ComponentModel;
using System.ServiceProcess;
using System.Configuration.Install;
using iPower.WinService.Config;
namespace iPower.WinService.Shell
{
    /// <summary>
    /// Widows服务安装配置类。
    /// </summary>
    [RunInstaller(true)]
    public class WinServiceInstaller :Installer
    {
        #region 构造函数，析购函数
        IContainer components = null;
        ServiceProcessInstaller serviceProcessInstaller = null;
        ServiceInstaller serviceInstaller = null;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public WinServiceInstaller()
        {
            this.InitializeComponent();
        }
        #endregion

        #region 辅助函数.
        /// <summary>
        /// 初始化.
        /// </summary>
        private void InitializeComponent()
        {
            this.serviceProcessInstaller = new ServiceProcessInstaller();
            this.serviceInstaller = new ServiceInstaller();

            string userName = ServiceConfig.ModuleConfig.Username;
            this.serviceProcessInstaller.Account = string.IsNullOrEmpty(userName) ? ServiceAccount.LocalSystem : ServiceAccount.User;
            if (!string.IsNullOrEmpty(userName))
            {
                this.serviceProcessInstaller.Username = userName;
                this.serviceProcessInstaller.Password = ServiceConfig.ModuleConfig.Password;
            }

            this.serviceInstaller.Description = ServiceConfig.ModuleConfig.Description;
            this.serviceInstaller.DisplayName = ServiceConfig.ModuleConfig.DisplayName;
            this.serviceInstaller.ServiceName = ServiceConfig.ModuleConfig.ServiceName;
            this.serviceInstaller.StartType = ServiceStartMode.Automatic;

            this.Installers.AddRange(new Installer[] { this.serviceProcessInstaller,
                                                       this.serviceInstaller});
        }
        #endregion

        #region 重载
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
                this.components.Dispose();
            base.Dispose(disposing);
        }
        #endregion
    }
}