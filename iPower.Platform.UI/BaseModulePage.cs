using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.Caching;
using iPower;
using iPower.Web.UI;
using iPower.Platform;
using iPower.Platform.Security;
namespace iPower.Platform.UI
{
    /// <summary>
    /// 页面基础类。
    /// </summary>
    public abstract partial class BaseModulePage : Page, IBaseModulePage
    {
        #region 成员变量，构造函数。
        System.Web.Caching.Cache cache = null;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public BaseModulePage()
            : base()
        {
            this.cache = this.Context.Cache;
        }
        #endregion

        #region IPagebase 成员
        /// <summary>
        /// 获取状态信息字典。
        /// </summary>
        public StateBag PageViewState
        {
            get
            {
                lock (this)
                {
                    return this.ViewState;
                }
            }
        }
        /// <summary>
        /// 获取或设置当前页面的标题。
        /// </summary>
        public string CurrentPageTile
        {
            get
            {
                string s = (string)this.ViewState["CurrentPageTile"];
                return string.IsNullOrEmpty(s) ? string.Empty : s;
            }
            set
            {
                this.ViewState["CurrentPageTile"] = value;
            }
        }

        #endregion

        #region IUser 成员
        /// <summary>
        /// 获取当前用户标示。
        /// </summary>
        public GUIDEx CurrentUserID
        {
            get { return new GUIDEx(this.ViewState["CurrentUserID"]); }
            set { this.ViewState["CurrentUserID"] = value; }
        }
        /// <summary>
        /// 获取当前用户名称。
        /// </summary>
        public string CurrentUserName
        {
            get { return this.ViewState["CurrentUserName"] as string; }
            set { this.ViewState["CurrentUserName"] = value; }
        }

        #endregion

        #region IUISettings 成员
        /// <summary>
        /// 获取或设置特效图片URL。
        /// </summary>
        public string EffectImageURL
        {
            get
            {
                return this.ViewState["EffectImageURL"] as string;
            }
            set
            {
                this.ViewState["EffectImageURL"] = value;
            }
        }
        /// <summary>
        ///  获取或设置当前目录标示。
        /// </summary>
        public string CurrentFolderID
        {
            get
            {
                object obj = this.ViewState["CurrentFolderID"];
                if (obj == null)
                    return this.SecurityID;
                string folderId = obj.ToString();
                return string.IsNullOrEmpty(folderId) ? this.SecurityID.ToString() : folderId;
            }
            set
            {
                this.ViewState["CurrentFolderID"] = value;
            }
        }
        /// <summary>
        /// 获取或设置当前模块标题名称。
        /// </summary>
        public string CurrentModuleTitle
        {
            get
            {
                return this.ViewState["CurrentModuleTitle"] as string;
            }
            set
            {
                this.ViewState["CurrentModuleTitle"] = value;
            }
        }
        /// <summary>
        /// 获取或设置版权名称。
        /// </summary>
        public string CopyRight
        {
            get
            {
                return this.ViewState["CopyRight"] as string;
            }
            set
            {
                this.ViewState["CopyRight"] = value;
            }
        }
        /// <summary>
        /// 获取或设置Benner头图片路径。
        /// </summary>
        public string[] BannerImageUrl
        {
            get
            {
                return this.ViewState["BannerImageUrl"] as string[];
            }
            set
            {
                this.ViewState["BannerImageUrl"] = value;
            }
        }
        /// <summary>
        /// 获取或设置Logo图片路径。
        /// </summary>
        public string[] LogoImageUrl
        {
            get
            {
                return this.ViewState["LogoImageUrl"] as string[];
            }
            set
            {
                this.ViewState["LogoImageUrl"] = value;
            }
        }
        /// <summary>
        /// 获取或设置样式表资源集合。
        /// </summary>
        public string[] WebCssPaths
        {
            get
            {
                return this.ViewState["WebCssPaths"] as string[];
            }
            set
            {
                this.ViewState["WebCssPaths"] = value;
            }
        }
        /// <summary>
        /// 获取或设置脚本资源集合。
        /// </summary>
        public string[] WebScriptPaths
        {
            get
            {
                return this.ViewState["WebScriptPaths"] as string[];
            }
            set
            {
                this.ViewState["WebScriptPaths"] = value;
            }
        }
        #endregion

        #region 函数。
        /// <summary>
        /// 数据保存并关闭页面。
        /// </summary>
        /// <returns></returns>
        public virtual bool SaveData(string result)
        {
            this.Context.Response.Write(string.Format(WebUIConst.SaveReturnScript, result));
            return true;
        }
        /// <summary>
        /// Picker页面数据保存并关闭页面。
        /// </summary>
        /// <param name="resultText">显示值。</param>
        /// <param name="resultValue">值。</param>
        /// <returns></returns>
        public virtual bool SaveData(string resultText, string resultValue)
        {
            return this.SaveData(string.Format("{0}|{1}", resultText, resultValue));
        }
        /// <summary>
        /// 数据保存并关闭页面。
        /// </summary>
        /// <returns></returns>
        public bool SaveData()
        {
            return this.SaveData("true");
        }
        /// <summary>
        /// 获取给定键值的GUIDEx类型数据。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual GUIDEx RequestGUIEx(string key)
        {
            if (!string.IsNullOrEmpty(key))
                return new GUIDEx(this.Request[key]);
            return GUIDEx.Null;
        }
        #endregion

        #region ISystem 成员
        /// <summary>
        /// 获取或设置当前系统ID。
        /// </summary>
        public GUIDEx CurrentSystemID
        {
            get
            {
                object obj = this.ViewState["CurrentSystemID"];
                return obj == null ? GUIDEx.Null : new GUIDEx(obj);
            }
            set { this.ViewState["CurrentSystemID"] = value; }
        }
        /// <summary>
        /// 获取或设置当前系统名称。
        /// </summary>
        public string CurrentSystemName
        {
            get { return this.ViewState["CurrentSystemName"] as string; }
            set { this.ViewState["CurrentSystemName"] = value; }
        }

        #endregion

        #region IMenuData 成员
        /// <summary>
        /// 获取或设置菜单数据。
        /// </summary>
        public ModuleDefineCollection MenuData
        {
            get
            {
                return this.ViewState["MenuData"] as ModuleDefineCollection;
            }
            set
            {
                this.ViewState["MenuData"] = value;
            }
        }
        /// <summary>
        /// 获取或设置主菜单数据。
        /// </summary>
        public ModuleDefineCollection MainMenuData
        {
            get
            {
                return this.ViewState["MainMenuData"] as ModuleDefineCollection;
            }
            set
            {
                this.ViewState["MainMenuData"] = value;
            }
        }
        #endregion

        #region IDataHandler 成员
        /// <summary>
        /// 加载数据。
        /// </summary>
        public virtual void LoadData()
        {
        }
        /// <summary>
        /// 删除数据。
        /// </summary>
        /// <returns></returns>
        public virtual bool DeleteData()
        {
            return false;
        }

        #endregion

        #region IBaseView 成员
        /// <summary>
        /// 获取或设置导航数据。
        /// </summary>
        public string NavigationContent
        {
            get
            {
                object obj = this.ViewState["NavigationContent"];
                return obj == null ? string.Empty : (string)obj;
            }
            set
            {
                this.ViewState["NavigationContent"] = value;
            }
        }

        #endregion
    }
}
