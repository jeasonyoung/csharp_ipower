//================================================================================
//  FileName: IdentityImpersonation.cs
//  Desc:用户模拟角色类。实现在程序段内进行用户角色模拟。
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2010-1-13
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
using System.Web;
using System.Security.Principal;
using System.Runtime.InteropServices;

namespace iPower.Utility
{
    /// <summary>
    /// 用户模拟角色类。实现在程序段内进行用户角色模拟。
    /// </summary>
    public sealed class IdentityImpersonation : IDisposable
    {
        #region 成员变量，构造函数。
        string imperUsername, imperPassword, imperDomain;
        WindowsImpersonationContext imperContext;//记录模拟上下文
        IntPtr adminToken, dupeToken;
        bool closed;//是否已停止模拟
        /// <summary>
        /// 构造函数。
        /// </summary>
        public IdentityImpersonation(string imperDomain,string imperUsername, string imperPassword)
        {
            this.imperDomain = imperDomain;
            this.imperUsername = imperUsername;
            this.imperPassword = imperPassword;

            this.adminToken = this.dupeToken = IntPtr.Zero;
            this.closed = true;
        }
        /// <summary>
        /// 析构函数。
        /// </summary>
        ~IdentityImpersonation()
        {
            if (!this.closed)
                this.StopImpersonate();
        }
        #endregion

        /// <summary>
        /// 开始身份角色模拟。
        /// </summary>
        /// <returns></returns>
        public bool BeginImpersonate()
        {
            bool bLogined = LogonUser(this.imperUsername, this.imperDomain, this.imperPassword, 2, 0, ref this.adminToken);
            if (!bLogined)
                return false;
            bool bDuped = DuplicateToken(this.adminToken, 2, ref this.dupeToken);
            if (!bDuped)
                return false;
            WindowsIdentity fakeid = new WindowsIdentity(this.dupeToken);
            this.imperContext = fakeid.Impersonate();
            this.closed = false;
            return true;
        }

        /// <summary>
        /// 停止身分角色模拟。
        /// </summary>
        public void StopImpersonate()
        {
            CloseHandle(this.dupeToken);
            CloseHandle(this.adminToken);
            this.closed = true;
        }

        #region Win32_API
        [DllImport("advapi32.dll", SetLastError = true)]
        extern static bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);
        
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        extern static bool DuplicateToken(IntPtr ExistingTokenHandle, int SECURITY_IMPERSONATION_LEVEL, ref IntPtr DuplicateTokenHandle);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        extern static bool CloseHandle(IntPtr handle);
        #endregion

        #region IDisposable 成员
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (!this.closed)
                this.StopImpersonate();
        }

        #endregion
    }
}
