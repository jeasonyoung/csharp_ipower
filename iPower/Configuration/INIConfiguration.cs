//================================================================================
//  FileName: INIConfiguration.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/9/26
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
using System.IO;
using System.Runtime.InteropServices;

namespace iPower.Configuration
{
    /// <summary>
    /// INI配置的写入和读取。
    /// </summary>
    /// <example>
    /// INI文件结构如下：
    /// [小节名]
    /// 关键字=值
    /// </example>
    public static class INIConfiguration
    {
        #region Kernel32
        /// <summary>
        /// 创建INI配置的API。
        /// </summary>
        /// <param name="section">小节名。</param>
        /// <param name="key">关键字。</param>
        /// <param name="val">值（可以是变量、字符串、整型、Bool型）。</param>
        /// <param name="filePath">文件所在路径。</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        /// <summary>
        /// 读取INI配置的API。
        /// </summary>
        /// <param name="section">小节名。</param>
        /// <param name="key">关键字。</param>
        /// <param name="def">默认值。</param>
        /// <param name="retValue">值。</param>
        /// <param name="size">值的最大长度。</param>
        /// <param name="filePath">文件所在路径。</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retValue, int size, string filePath);
        #endregion

        /// <summary>
        /// 写入配置。
        /// </summary>
        /// <param name="filePath">文件所在路径。</param>
        /// <param name="section">小节名。</param>
        /// <param name="key">关键字。</param>
        /// <param name="value">值。</param>
        public static void Write(string filePath, string section, string key, string value)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException("filePath", "文件所在路径为空。");
            if (string.IsNullOrEmpty(section))
                throw new ArgumentNullException("section", "小节名为空。");
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key", "关键字为空。");

            WritePrivateProfileString(section, key, value, filePath);
        }
        /// <summary>
        /// 读取配置。
        /// </summary>
        /// <param name="filePath">文件所在路径。</param>
        /// <param name="section">小节名。</param>
        /// <param name="key">关键字。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns>值。</returns>
        public static string Read(string filePath, string section, string key, string defaultValue)
        {
            if (!File.Exists(filePath))
                throw new ArgumentNullException("filePath", "文件不存在。");
            if (string.IsNullOrEmpty(section))
                throw new ArgumentNullException("section", "小节名为空。");
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key", "关键字为空。");

            StringBuilder temp = new StringBuilder();
            GetPrivateProfileString(section, key, defaultValue, temp, 255, filePath);
            return temp.ToString();
        }
    }

}
