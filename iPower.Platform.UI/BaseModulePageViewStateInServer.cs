//================================================================================
//  FileName: BaseModulePageViewStateInServer.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/7/25
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
using System.Web;
using System.Web.UI;
using System.IO;
using System.IO.Compression;
namespace iPower.Platform.UI
{
    /// <summary>
    /// ViewState处理。
    /// </summary>
    partial  class BaseModulePage
    {
        #region 属性。
        /// <summary>
        /// 获取ViewState是否存储在服务器上。
        /// </summary>
        public virtual bool ViewStateInServer
        {
            get { return false; }
        }
        #endregion

        #region 重载。
        /// <summary>
        /// 保存页的所有视图状态信息和控件状态信息。
        /// </summary>
        /// <param name="state"></param>
        protected override void SavePageStateToPersistenceMedium(object state)
        {
            if (this.ViewStateInServer && this.CurrentUserID.IsValid)
            {
                lock (this)
                {
                    string path = this.GetViewStateFileName(this.CurrentUserID);
                    if (!string.IsNullOrEmpty(path))
                    {
                        string dir = Path.GetDirectoryName(path);
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }
                        using (FileStream fs = new FileStream(path, FileMode.Create))
                        {
                            new LosFormatter().Serialize(fs, state);
                        }
                    }
                }
            }
            else
            {
                base.SavePageStateToPersistenceMedium(state);
            }
        }
        /// <summary>
        /// 将所有保存的视图状态信息加载到 <see cref="Page"/> 对象。
        /// </summary>
        /// <returns></returns>
        protected override object LoadPageStateFromPersistenceMedium()
        {
            if (this.ViewStateInServer && this.CurrentUserID.IsValid)
            {
                lock (this)
                {
                    object obj = null;
                    string path = this.GetViewStateFileName(this.CurrentUserID);
                    if (File.Exists(path))
                    {
                        using (FileStream fs = new FileStream(path, FileMode.Open))
                        {
                            obj = new LosFormatter().Deserialize(fs);
                        }
                    }
                    return obj;
                }
            }
            else
            {
                return base.LoadPageStateFromPersistenceMedium();
            }
        }
        #endregion

        #region 辅助函数。
        /// <summary>
        /// 获取ViewState存储文件名。
        /// </summary>
        /// <returns></returns>
        protected virtual string GetViewStateFileName(string viewStateID)
        {
            string str = this.Request.Path.Replace("/", "_").Replace(".", "_");
            string path = Path.Combine(this.Request.PhysicalApplicationPath, string.Format(@"App_Data\ViewState\{0}_{1}.ViewState", viewStateID, str));
            return Path.GetFullPath(path);
        }
        #endregion
    }
}