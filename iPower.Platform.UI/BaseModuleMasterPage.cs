//================================================================================
//  FileName: BaseModuleMasterPage.cs
//  Desc:模板基类。
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2009-11-23
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
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using iPower.Cryptography;
using iPower.Platform;
namespace iPower.Platform.UI
{
    /// <summary>
    /// 模板基类。
    /// </summary>
    public partial class BaseModuleMasterPage : MasterPage,IUser
    {
        #region 成员变量，构造函数,析构函数。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public BaseModuleMasterPage()
        {
        }
        /// <summary>
        /// 析构函数。
        /// </summary>
        ~BaseModuleMasterPage()
        {
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取UI实例。
        /// </summary>
        protected virtual IBaseModulePage ModulePage
        {
            get
            {
                return this.Page as IBaseModulePage;
            }
        }
        /// <summary>
        /// 获取Loading图片地址。
        /// </summary>
        protected virtual string EffectImageURL
        {
            get
            {
                return this.ModulePage == null ? string.Empty : this.ModulePage.EffectImageURL;
            }
        }
        #endregion

        #region 重载。
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            if (this.Page != null)
            {
                this.Page.Title = this.GetPageTitle;
                if (this.Page.Header != null)
                    this.AddCssStyle(this.Page.Header);
                if (this.Page.ClientScript != null)
                    this.AddScript(this.Page.ClientScript);
            }
            base.OnLoad(e);
        }
        #endregion

        #region 函数。
        /// <summary>
        /// 获取页面标题。
        /// </summary>
        protected virtual string GetPageTitle
        {
            get
            {
                string strTitle = string.Empty;
                if (this.ModulePage != null)
                {
                    if (!string.IsNullOrEmpty(this.ModulePage.CurrentPageTile))
                        strTitle = this.ModulePage.CurrentPageTile;
                    else
                        strTitle = this.ModulePage.CurrentSystemName;

                    string moduleTitle = this.ModulePage.CurrentModuleTitle;
                    if (!string.IsNullOrEmpty(moduleTitle))
                    {
                        strTitle = string.Format("{0}-{1}", strTitle, moduleTitle);
                    }
                    
                }
                return strTitle;
            }
        }

        /// <summary>
        /// 添加样式文件。
        /// </summary>
        /// <param name="head">HtmlHead。</param>
        protected virtual void AddCssStyle(HtmlHead head)
        {
            if (this.ModulePage != null && head != null)
            {
                string[] styles = this.ModulePage.WebCssPaths;
                if (styles != null && styles.Length > 0)
                {
                    foreach (string href in styles)
                    {
                        if (!string.IsNullOrEmpty(href))
                        {
                            HtmlLink styleLink = new HtmlLink();
                            styleLink.Href = this.Page.ResolveUrl(href);
                            styleLink.Attributes.Add("rel", "stylesheet");
                            styleLink.Attributes.Add("type", "text/css");
                            head.Controls.Add(styleLink);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 添加样式脚本。
        /// </summary>
        /// <param name="scriptManger">ClientScriptManager。</param>
        protected virtual void AddScript(ClientScriptManager scriptManger)
        {
            string[] scripts = this.ModulePage.WebScriptPaths;
            if (scripts != null && scripts.Length > 0 && scriptManger!= null)
            {
                foreach (string path in scripts)
                {
                    if (!string.IsNullOrEmpty(path))
                    {
                        string strKey = HashCrypto.Hash(path, "md5");
                        if(!scriptManger.IsClientScriptIncludeRegistered(strKey))
                            scriptManger.RegisterClientScriptInclude(strKey, this.Page.ResolveUrl(path));
                    }
                }
            }
        }
        #endregion

        #region IUser 成员
        /// <summary>
        /// 获取或设置当前用户ID。
        /// </summary>
        public GUIDEx CurrentUserID
        {
            get
            {
                object obj = this.ViewState["CurrentUserID"];
                return obj == null ? GUIDEx.Null : new GUIDEx(obj);
            }
            set
            {
                this.ViewState["CurrentUserID"] = value;
            }
        }
        /// <summary>
        /// 获取或设置当前用户姓名。
        /// </summary>
        public string CurrentUserName
        {
            get
            {
                return this.ViewState["CurrentUserName"] as string;
            }
            set
            {
                this.ViewState["CurrentUserName"] = value;
            }
        }

        #endregion
    }
}
