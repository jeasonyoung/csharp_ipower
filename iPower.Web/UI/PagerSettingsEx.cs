//================================================================================
//  FileName: PagerSettingsEx.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/3/8
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
using System.Drawing;
using System.Drawing.Design;
using System.ComponentModel;
namespace iPower.Web.UI
{
    /// <summary>
    /// 支持分页的控件中的分页控件的属性。无法继承此类。
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class PagerSettingsEx : IStateManager
    {
        #region 成员变量，构造函数。
        bool isTracking;
        StateBag viewState;

        /// <summary>
        /// 构造函数。
        /// </summary>
        public PagerSettingsEx()
        {
            this.viewState = new StateBag();
        }
        #endregion

        #region 事件处理。
        /// <summary>
        /// 属性更改值时发生。
        /// </summary>
        [Browsable(false)]
        public event EventHandler PropertyChanged;
        void OnPropertyChanged()
        {
            EventHandler handler = this.PropertyChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取状态视图。
        /// </summary>
        protected StateBag ViewState
        {
            get { return this.viewState; }
        }
        /// <summary>
        /// 获取或设置第一页按钮显示的图像的 URL。
        /// </summary>
        [Category("Appearance")]
        [Description("获取或设置第一页按钮显示的图像的 URL。")]
        [UrlProperty]
        [Editor(typeof(System.Web.UI.Design.ImageUrlEditor), typeof(UITypeEditor))]
        public string FirstPageImageUrl
        {
            get { return this.ViewState["FirstPageImageUrl"] as string; }
            set
            {
                if (this.FirstPageImageUrl != value)
                {
                    this.ViewState["FirstPageImageUrl"] = value;
                    this.OnPropertyChanged();
                }
            }
        }
        /// <summary>
        /// 获取或设置为第一页按钮显示的文字。
        /// </summary>
        [Category("Appearance")]
        [Description("获取或设置为第一页按钮显示的文字。")]
        [DefaultValue("&lt;&lt;")]
        [NotifyParentProperty(true)]
        public string FirstPageText
        {
            get
            {
                object obj = this.ViewState["FirstPageText"];
                if(obj != null)
                    return (string)obj;
                return "&lt;&lt;";
            }
            set
            {
                if (this.FirstPageText != value)
                {
                    this.ViewState["FirstPageText"] = value;
                    this.OnPropertyChanged();
                }
            }
        }
        /// <summary>
        /// 获取导航是否在底部。
        /// </summary>
        internal bool IsPagerOnBottom
        {
            get
            {
                PagerPosition position = this.Position;
                if (position != PagerPosition.Bottom)
                    return position == PagerPosition.TopAndBottom;
                return true;
            }
        }
        /// <summary>
        /// 获取导航是否在顶部。
        /// </summary>
        internal bool IsPagerOnTop
        {
            get
            {
                PagerPosition position = this.Position;
                if (position != PagerPosition.Top)
                    return position == PagerPosition.TopAndBottom;
                return true;
            }
        }
        /// <summary>
        /// 获取或设置为最后一页按钮显示的图像的 URL。
        /// </summary>
        [Category("Appearance")]
        [Description("获取或设置为最后一页按钮显示的图像的 URL。")]
        [NotifyParentProperty(true)]
        [Editor(typeof(System.Web.UI.Design.ImageUrlEditor), typeof(UITypeEditor))]
        public string LastPageImageUrl
        {
            get { return this.ViewState["LastPageImageUrl"] as string; }
            set
            {
                if (this.LastPageImageUrl != value)
                {
                    this.ViewState["LastPageImageUrl"] = value;
                    this.OnPropertyChanged();
                }
            }
        }
        /// <summary>
        /// 获取或设置为最后一页按钮显示的文字。
        /// </summary>
        [Category("Appearance")]
        [Description("获取或设置为最后一页按钮显示的文字。")]
        [DefaultValue("&gt;&gt;")]
        [NotifyParentProperty(true)]
        public string LastPageText
        {
            get
            {
                object obj = this.ViewState["LastPageText"];
                if (obj != null)
                    return (string)obj;
                return "&gt;&gt;";
            }
            set
            {
                if (this.LastPageText != value)
                {
                    this.ViewState["LastPageText"] = value;
                    this.OnPropertyChanged();
                }
            }
        }
        /// <summary>
        /// 获取或设置支持分页的控件中的页导航控件的显示模式。
        /// </summary>
        [Category("Appearance")]
        [Description("获取或设置支持分页的控件中的页导航控件的显示模式。")]
        [NotifyParentProperty(true)]
        public PagerButtons Mode
        {
            get
            {
                object obj = this.ViewState["PagerMode"];
                if (obj != null)
                    return (PagerButtons)obj;
                return PagerButtons.Numeric;
            }
            set
            {
                if ((value < PagerButtons.NextPrevious) || (value > PagerButtons.NumericFirstLast))
                    throw new ArgumentOutOfRangeException("value");
                if (this.Mode != value)
                {
                    this.ViewState["PagerMode"] = value;
                    this.OnPropertyChanged();
                }
            }
        }
        /// <summary>
        /// 获取或设置为下一页按钮显示的图像的 URL。
        /// </summary>
        [Category("Appearance")]
        [Description("获取或设置为下一页按钮显示的图像的 URL。")]
        [UrlProperty]
        [Editor(typeof(System.Web.UI.Design.ImageUrlEditor), typeof(UITypeEditor))]
        public string NextPageImageUrl
        {
            get { return this.ViewState["NextPageImageUrl"] as string; }
            set
            {
                if (this.NextPageImageUrl != value)
                {
                    this.ViewState["NextPageImageUrl"] = value;
                    this.OnPropertyChanged();
                }
            }
        }
        /// <summary>
        /// 获取或设置为下一页按钮显示的文字。
        /// </summary>
        [Category("Appearance")]
        [Description("获取或设置为下一页按钮显示的文字。")]
        [DefaultValue("&gt;")]
        [NotifyParentProperty(true)]
        public string NextPageText
        {
            get
            {
                object obj = this.ViewState["NextPageText"];
                if (obj != null)
                     return (string)obj;
                 return "&gt;";
            }
            set
            {
                if (this.NextPageText != value)
                {
                    this.ViewState["NextPageText"] = value;
                    this.OnPropertyChanged();
                }
            }
        }
        /// <summary>
        /// 获取或设置在 <see cref="Mode"/> 属性设置为 Numeric 或 NumericFirstLast 值时页导航中显示的页按钮的数量。
        /// </summary>
        [Category("Behavior")]
        [Description(" 获取或设置在 Mode 属性设置为 Numeric 或 NumericFirstLast 值时页导航中显示的页按钮的数量。")]
        [NotifyParentProperty(true)]
        [DefaultValue(10)]
        public int PageButtonCount
        {
            get
            {
                object obj = this.ViewState["PageButtonCount"];
                if (obj != null)
                     return (int)obj;
                 return 10;
            }
            set
            {
                if (value < 1)
                     throw new ArgumentOutOfRangeException("value");
                 if (this.PageButtonCount != value)
                {
                    this.ViewState["PageButtonCount"] = value;
                    this.OnPropertyChanged();
                }
            }
        }
        /// <summary>
        /// 获取或设置一个值，该值指定页导航的显示位置。
        /// </summary>
        [Category("Layout")]
        [Description("获取或设置一个值，该值指定页导航的显示位置。")]
        [NotifyParentProperty(true)]
        public PagerPosition Position
        {
            get
            {
                object obj = this.ViewState["Position"];
                if (obj != null)
                    return (PagerPosition)obj;
                return PagerPosition.Bottom;
            }
            set
            {
                if ((value < PagerPosition.Bottom) || (value > PagerPosition.TopAndBottom))
                    throw new ArgumentOutOfRangeException("value");
                this.ViewState["Position"] = value;
            }
        }
        /// <summary>
        /// 获取或设置为上一页按钮显示的图像的 URL。
        /// </summary>
        [Category("Appearance")]
        [Description("获取或设置为上一页按钮显示的图像的 URL。")]
        [UrlProperty]
        [Editor(typeof(System.Web.UI.Design.ImageUrlEditor), typeof(UITypeEditor))]
        public string PreviousPageImageUrl
        {
            get { return this.ViewState["PreviousPageImageUrl"] as string; }
            set
            {
                if (this.PreviousPageImageUrl != value)
                {
                    this.ViewState["PreviousPageImageUrl"] = value;
                    this.OnPropertyChanged();
                }
            }
        }
        /// <summary>
        /// 获取或设置为上一页按钮显示的文字。
        /// </summary>
        [Category("Appearance")]
        [Description("获取或设置为上一页按钮显示的文字。")]
        [DefaultValue("&lt;")]
        [NotifyParentProperty(true)]
        public string PreviousPageText
        {
            get
            {
                object obj = this.ViewState["PreviousPageText"];
                if (obj != null)
                     return (string)obj;
                 return "&lt;";
            }
            set
            {
                if (this.PreviousPageText != value)
                {
                    this.ViewState["PreviousPageText"] = value;
                    this.OnPropertyChanged();
                }
            }
        }
        /// <summary>
        /// 获取或设置是否在支持分页的控件中显示分页控件。
        /// </summary>
        [Category("Appearance")]
        [Description("获取或设置是否在支持分页的控件中显示分页控件。")]
        [NotifyParentProperty(true)]
        [DefaultValue(true)]
        public bool Visible
        {
            get
            {
                object obj = this.ViewState["PagerVisible"];
                if (obj != null)
                     return (bool)obj;
                 return true;
            }
            set
            {
                this.ViewState["PagerVisible"] = value;
            }
        }
 
        #endregion

        #region IStateManager 成员
        /// <summary>
        /// 获取服务器控件是否正在跟踪其视图状态更改。
        /// </summary>
        public bool IsTrackingViewState
        {
            get { return this.isTracking; }
        }
        /// <summary>
        /// 加载以前保存的对象的视图状态。
        /// </summary>
        /// <param name="state"></param>
        public void LoadViewState(object state)
        {
            if (state != null)
                ((IStateManager)this.ViewState).LoadViewState(state);
        }
        /// <summary>
        /// 保存对象的当前视图状态。
        /// </summary>
        /// <returns></returns>
        public object SaveViewState()
        {
            return ((IStateManager)this.ViewState).SaveViewState();
        }
        /// <summary>
        /// 标记开始跟踪并将视图状态更改保存到对象的起始点。
        /// </summary>
        public void TrackViewState()
        {
            this.isTracking = true;
            ((IStateManager)this.ViewState).TrackViewState();
        }

        #endregion

        #region 重载。
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Empty;
        }
        #endregion
    }
}
