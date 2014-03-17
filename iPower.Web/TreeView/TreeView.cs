//================================================================================
//  FileName:TreeView.cs
//  Desc:树型控件。
//
//  Called by
//
//  Auth:JeasonYoung
//  Date:2010-09-25 08:52:05
//================================================================================
//  Change History
//================================================================================
//  Date  Author  Description
// ----  ------  -----------
//
//================================================================================
//  Copyright (C) 2009-2010 Jeason Young Corporation
//================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using System.Xml;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Security.Permissions;

using System.Drawing;
using System.ComponentModel;

using iPower;
using iPower.Utility;
using iPower.Platform;

[assembly: WebResource("iPower.Web.TreeView.F.gif", "image/gif")]
[assembly: WebResource("iPower.Web.TreeView.I.gif", "image/gif")]
[assembly: WebResource("iPower.Web.TreeView.L.gif", "image/gif")]
[assembly: WebResource("iPower.Web.TreeView.Lminus.gif", "image/gif")]
[assembly: WebResource("iPower.Web.TreeView.Lplus.gif", "image/gif")]
[assembly: WebResource("iPower.Web.TreeView.T.gif", "image/gif")]
[assembly: WebResource("iPower.Web.TreeView.Tminus.gif", "image/gif")]
[assembly: WebResource("iPower.Web.TreeView.Tplus.gif", "image/gif")]
[assembly: WebResource("iPower.Web.TreeView.openfolder.gif", "image/gif")]
[assembly: WebResource("iPower.Web.TreeView.closedfolder.gif", "image/gif")]
namespace iPower.Web.TreeView
{
    /// <summary>
    /// 树形菜单。
    /// </summary>
    [ToolboxData("<{0}:TreeView runat='server'></{0}:TreeView>")]
    [ToolboxBitmap(typeof(TreeView))]
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal),
     AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public partial class TreeView : Panel,INamingContainer, ITreeView
    {
        #region 成员变量，构造函数。
        object dataSource;
        StringCollection checkedValueCollection;
        TreeViewNodeCollection items = null;
        bool enabledChecked;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public TreeView()
        {
            this.checkedValueCollection = new StringCollection();
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取节点项数据。
        /// </summary>
        [Browsable(false)]
        [Bindable(false)]
        public TreeViewNodeCollection Items
        {
            get
            {
                if (this.items == null)
                    this.items = new TreeViewNodeCollection();
                return this.items;
            }
        }
        /// <summary>
        /// 获取或设置选择时的所有值。
        /// </summary>
        [Browsable(false)]
        [Bindable(false)]
        public StringCollection CheckedValue
        {
            get
            {
                if (this.checkedValueCollection == null)
                    this.checkedValueCollection = new StringCollection();
                this.checkedValueCollection.Clear();
                if (this.ShowCheckBox && this.Page != null)
                {
                    NameValueCollection parameters = this.Page.Request.Form;
                    string startKey = string.Empty;
                    if (this.CheckType == CheckBoxType.CheckBox)
                    {
                        startKey = string.Format("{0}_chkb_", this.ClientID);
                        foreach (string key in parameters)
                        {
                            if (key.StartsWith(startKey))
                                this.checkedValueCollection.Add(parameters[key]);
                        }
                    }
                    else
                    {
                        startKey = string.Format("{0}_rd", this.ClientID);
                        string value = parameters[startKey];
                        if (!string.IsNullOrEmpty(value))
                            this.checkedValueCollection.Add(value);
                    }
                }
                return this.checkedValueCollection;
            }
            set
            {
                if (value == null)
                {
                    if (this.checkedValueCollection == null)
                        this.checkedValueCollection = new StringCollection();
                    this.checkedValueCollection.Clear();
                }
                else
                    this.checkedValueCollection = value;
                this.EnabledCheckValueChecked = true;
                this.DataBind();
            }
        }

        /// <summary>
        /// 获取或设置是否启用CheckedValue选中。
        /// </summary>
        [Category("Data")]
        [Description("获取或设置是否启用CheckedValue选中。")]
        public bool EnabledCheckValueChecked
        {
            get { return this.enabledChecked; }
            set { this.enabledChecked = value; }
        }
        /// <summary>
        /// 获取或设置选择类型。
        /// </summary>
        [Category("Data")]
        [DefaultValue(0)]
        [Description("获取或设置选择类型。")]
        public CheckBoxType CheckType
        {
            get
            {
                object obj = this.ViewState["CheckType"];
                return obj == null ? CheckBoxType.None : (CheckBoxType)obj;
            }
            set
            {
                this.ViewState["CheckType"] = value;
            }
        }

        /// <summary>
        /// 获取或设置选项单击动作映射字段，必须有效的JS方法。
        /// </summary>
        [Category("Data")]
        [Description("获取或设置选项单击动作映射字段，必须有效的JS方法")]
        public string ClickActionField
        {
            get
            {
                object obj = this.ViewState["ClickActionField"];
                return obj == null ? string.Empty : (string)obj;
            }
            set { this.ViewState["ClickActionField"] = value; }
        }
              

        /// <summary>
        /// 获取或设置当前目录Value。
        /// </summary>
        [Bindable(false)]
        [Browsable(false)]
        public string CurrentFolderValue
        {
            get
            {
                object obj = this.ViewState["CurrentFolderValue"];
                return obj == null ? string.Empty : (string)obj;
            }
            set
            {
                this.ViewState["CurrentFolderValue"] = value;
                this.DataBind();
            }
        }

        /// <summary>
        /// 获取或设置数据源。
        /// </summary>
        [Bindable(false)]
        [Browsable(false)]
        public object DataSource
        {
            get { return this.dataSource; }
            set { this.dataSource = value; }
        }
        /// <summary>
        /// 获取或设置是否展开第一层次。
        /// </summary>
        [Category("Data")]
        [Description(" 获取或设置是否展开第一层次。")]
        public bool ExpandFirstLevel
        {
            get
            {
                object obj = this.ViewState["ExpandFirstLevel"];
                return (obj != null) && (bool)obj;
            }
            set { this.ViewState["ExpandFirstLevel"] = value; }
        }
        /// <summary>
        /// 获取或设置是否全部展开。
        /// </summary>
        [Category("Data")]
        [Description("获取或设置是否全部展开。")]
        public bool ExpandAllLevel
        {
            get
            {
                object obj = this.ViewState["ExpandAllLevel"];
                return (obj != null) && (bool)obj;
            }
            set { this.ViewState["ExpandAllLevel"] = value; }
        }
        /// <summary>
        /// 获取或设置选项链接映射字段，必须为有效的URL地址。
        /// </summary>
        [Category("Data")]
        [DefaultValue("")]
        [Description("获取或设置选项链接映射字段，必须为有效的URL地址。")]
        public string HrefField
        {
            get
            {
                object obj = this.ViewState["HrefField"];
                return obj == null ? string.Empty : (string)obj;
            }
            set { this.ViewState["HrefField"] = value; }
        }
        /// <summary>
        /// 获取或设置ID映射字段。
        /// </summary>
        [Category("Data")]
        [DefaultValue("ID")]
        [Description("获取或设置ID映射字段。")]
        public string IDField
        {
            get
            {
                object obj = this.ViewState["IDField"];
                return obj == null ? string.Empty : (string)obj;
            }
            set { this.ViewState["IDField"] = value; }
        }
        /// <summary>
        /// 获取或设置父ID值映射字段。
        /// </summary>
        [Category("Data")]
        [DefaultValue("PID")]
        [Description("获取或设置父ID值映射字段。")]
        public string PIDField
        {
            get
            {
                object obj = this.ViewState["PIDField"];
                return obj == null ? string.Empty : (string)obj;
            }
            set { this.ViewState["PIDField"] = value; }
        }

        /// <summary>
        /// 获取或设置树选项展开状态映射字段。
        /// </summary>
        [Category("Data")]
        [DefaultValue("Status")]
        [Description("获取或设置树选项展开状态映射字段。")]
        public string StatusField
        {
            get
            {
                object obj = this.ViewState["StatusField"];
                return obj == null ? "status" : (string)obj;
             }
            set { this.ViewState["StatusField"] = value; }
        }

        /// <summary>
        /// 获取或设置标题映射字段。
        /// </summary>
        [Category("Data")]
        [DefaultValue("Title")]
        [Description("获取或设置标题映射字段。")]
        public string TitleField
        {
            get
            {
                object obj = this.ViewState["TitleField"];
                return obj == null ? string.Empty : (string)obj;
            }
            set { this.ViewState["TitleField"] = value; }
        }
        /// <summary>
        /// 获取或设置排序映射字段。
        /// </summary>
        [Category("Data")]
        [DefaultValue("OrderNo")]
        [Description("获取或设置排序映射字段。")]
        public string OrderNoField
        {
            get
            {
                object obj = this.ViewState["OrderNoField"];
                return obj == null ? string.Empty : (string)obj;
            }
            set { this.ViewState["OrderNoField"] = value; }
        }
        /// <summary>
        /// 获取是否显示选择框，默认为否。
        /// </summary>
        protected bool ShowCheckBox
        {
            get { return (this.CheckType != CheckBoxType.None); }
         }
        
        /// <summary>
        /// 获取或设置是否显示滚动条。
        /// </summary>
        [Category("Data")]
        [DefaultValue(false)]
        [Description("获取或设置是否显示滚动条。")]
        public bool ShowScrollBar
        {
            get
            {
                object obj = this.ViewState["ShowScrollBar"];
                return (obj != null) && (bool)obj;
            }
            set { this.ViewState["ShowScrollBar"] = value; }
        }
        #endregion

        #region 重载。
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Name, this.ClientID);
            writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
            base.AddAttributesToRender(writer);
            if (this.ShowScrollBar && (!this.Height.IsEmpty || !this.Width.IsEmpty))
            {
                writer.AddStyleAttribute(HtmlTextWriterStyle.Overflow, "auto");
                writer.AddStyleAttribute(HtmlTextWriterStyle.OverflowX, "auto");
                writer.AddStyleAttribute(HtmlTextWriterStyle.OverflowY, "auto");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            ClientScriptManager csm = this.Page.ClientScript;
            if (csm != null)
            {
                string closeOpenTreeKey = string.Format("{0}_CloseOpenTree", this.GetType().Name);
                if (!csm.IsClientScriptBlockRegistered(this.GetType(), closeOpenTreeKey))
                {
                    StringBuilder builder = new StringBuilder();
                    builder.AppendLine("<script type=\"text/javascript\">");
                    builder.AppendLine(string.Format("function {0}(obj,openUrl,closeUrl)", closeOpenTreeKey));
                    builder.AppendLine("{");
                    builder.AppendLine("\ttry{");
                    builder.AppendLine("\t\tvar expand = obj.ep;");
                    //builder.AppendLine("\t\talert(expand);");
                    builder.AppendLine("\t\tvar trobj = obj.parentNode.nextSibling;");
                    builder.AppendLine("\t\tif(trobj && (trobj.tagName == \"TR\"))");
                    builder.AppendLine("\t\t{");
                    builder.AppendLine("\t\t\ttrobj.style.display = (expand == 1) ? \"none\":\"block\";");
                    builder.AppendLine("\t\t\tobj.ep = (expand == 1) ? 0:1; ");
                    builder.AppendLine("\t\t}");
                    builder.AppendLine("\t\tvar img = obj.childNodes[0];");
                    builder.AppendLine("\t\tif(img && (img.tagName == 'IMG') && openUrl && closeUrl)");
                    builder.AppendLine("\t\t{");
                    builder.AppendLine("\t\t\timg.src = (expand == 0) ? openUrl : closeUrl");
                    builder.AppendLine("\t\t}");
                    builder.AppendLine(string.Format("\t\tvar openF='{0}';", this.Page.ClientScript.GetWebResourceUrl(typeof(TreeView), "iPower.Web.TreeView.openfolder.gif")));
                    builder.AppendLine(string.Format("\t\tvar closeF='{0}';", this.Page.ClientScript.GetWebResourceUrl(typeof(TreeView), "iPower.Web.TreeView.closedfolder.gif")));
                    builder.AppendLine("\t\tvar imgF = obj.parentNode.childNodes[1].childNodes[0];");
                    builder.AppendLine("\t\tif(imgF && (imgF.tagName == 'IMG') && openF && closeF)");
                    builder.AppendLine("\t\t{");
                    builder.AppendLine("\t\t\timgF.src = (expand == 0) ? openF : closeF");
                    builder.AppendLine("\t\t}");
                    builder.AppendLine("\t}catch(e){");
                    builder.AppendLine("\t\talert(e.description);");
                    builder.AppendLine("\t}");
                    builder.AppendLine("}");
                    builder.AppendLine("</script>");
                    builder.AppendLine();
                    csm.RegisterClientScriptBlock(this.GetType(), closeOpenTreeKey, builder.ToString());
                }
                if (this.ShowCheckBox && this.CheckType == CheckBoxType.CheckBox)
                {
                    string checkAll_key = string.Format("{0}_CheckAll", this.GetType().Name);
                    if (!csm.IsClientScriptBlockRegistered(this.GetType(), checkAll_key))
                    {
                        StringBuilder builder = new StringBuilder();
                        builder.AppendLine("<script type=\"text/javascript\">");
                        builder.AppendLine(string.Format("function {0}(obj)", checkAll_key));
                        builder.AppendLine("{");
                        builder.AppendLine("\tvar trobj = obj.parentNode.parentNode.nextSibling;");
                        builder.AppendLine("\tif(trobj && (trobj.tagName == \"TR\")) ");
                        builder.AppendLine("\t{");
                        builder.AppendLine("\t\tvar tables=trobj.getElementsByTagName(\"TABLE\");");
                        builder.AppendLine("\t\tif(tables && tables.length > 0)");
                        builder.AppendLine("\t\t{");
                        builder.AppendLine("\t\t\tvar bChecked=obj.checked;");
                        builder.AppendLine("\t\t\tfor(var i=0; i<tables.length;i++)");
                        builder.AppendLine("\t\t\t{");
                        builder.AppendLine("\t\t\t\tvar table=tables[i];");
                        builder.AppendLine("\t\t\t\tif(table)");
                        builder.AppendLine("\t\t\t\t{");
                        builder.AppendLine("\t\t\t\t\tvar inputs=table.getElementsByTagName(\"INPUT\");");
                        builder.AppendLine("\t\t\t\t\tif(inputs && inputs.length>0)");
                        builder.AppendLine("\t\t\t\t\t{");
                        builder.AppendLine("\t\t\t\t\t\tfor(var j=0; j<inputs.length; j++)");
                        builder.AppendLine("\t\t\t\t\t\t{");
                        builder.AppendLine("\t\t\t\t\t\t\tvar input=inputs[j];");
                        builder.AppendLine("\t\t\t\t\t\t\tif(input && input.type==\"checkbox\")");
                        builder.AppendLine("\t\t\t\t\t\t\t{");
                        builder.AppendLine("\t\t\t\t\t\t\t\tinput.checked=bChecked;");
                        builder.AppendLine("\t\t\t\t\t\t\t}");
                        builder.AppendLine("\t\t\t\t\t\t}");
                        builder.AppendLine("\t\t\t\t\t}");
                        builder.AppendLine("\t\t\t\t}");
                        builder.AppendLine("\t\t\t}");
                        builder.AppendLine("\t\t}");
                        builder.AppendLine("\t}");
                        builder.AppendLine("}");
                        builder.AppendLine("</script>");
                        builder.AppendLine();
                        csm.RegisterClientScriptBlock(this.GetType(), checkAll_key, builder.ToString());
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Table;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="savedState"></param>
        protected override void LoadViewState(object savedState)
        {
            if (savedState != null)
            {
                Triplet triplet = savedState as Triplet;
                if (triplet != null)
                {
                    base.LoadViewState(triplet.First);
                    ((IStateManager)this.Items).LoadViewState(triplet.Second);
                    this.checkedValueCollection = triplet.Third as StringCollection;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override object SaveViewState()
        {
            object x = base.SaveViewState();
            object y = this.Items.SaveViewState();
            object z = this.checkedValueCollection;
            return new Triplet(x, y, z);
        }
        /// <summary>
        /// 
        /// </summary>
        protected override void TrackViewState()
        {
            base.TrackViewState();
            if (this.Items != null)
                ((IStateManager)this.Items).TrackViewState();
        }
        /// <summary>
        /// 绘制内容。
        /// </summary>
        /// <param name="writer"></param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (this.Items.Count > 0 && this.Page != null)
            {
                int len = this.Items.Count;
                TableRow tr = null;
                System.Web.UI.WebControls.Image image = null;
                TreeViewNode item = null;
                TableCell cell_0 = null, cell_1 = null, cell_2 = null;
                for (int i = 0; i < len; i++)
                {
                    item = this.Items[i];

                    tr = new TableRow();
                    cell_0 = new TableCell();
                    image = new System.Web.UI.WebControls.Image();
                    if (i < len - 1)
                    {
                        if (item.Childs.Count > 0)
                        {
                            image.ImageUrl = item.Expand ?
                                this.Page.ClientScript.GetWebResourceUrl(typeof(TreeView), "iPower.Web.TreeView.Tminus.gif") :
                                this.Page.ClientScript.GetWebResourceUrl(typeof(TreeView), "iPower.Web.TreeView.Tplus.gif");
                            cell_0.Attributes.Add("ep", item.Expand ? "1" : "0");
                            cell_0.Attributes.Add("onclick", string.Format("javascript:{0}_CloseOpenTree(this,'{1}','{2}');", this.GetType().Name,
                                this.Page.ClientScript.GetWebResourceUrl(typeof(TreeView), "iPower.Web.TreeView.Tminus.gif"),
                                 this.Page.ClientScript.GetWebResourceUrl(typeof(TreeView), "iPower.Web.TreeView.Tplus.gif")
                                ));
                        }
                        else
                            image.ImageUrl = this.Page.ClientScript.GetWebResourceUrl(typeof(TreeView), "iPower.Web.TreeView.T.gif");
                    }
                    else
                    {
                        if (item.Childs.Count > 0)
                        {
                            image.ImageUrl = item.Expand ?
                                this.Page.ClientScript.GetWebResourceUrl(typeof(TreeView), "iPower.Web.TreeView.Lminus.gif") :
                           this.Page.ClientScript.GetWebResourceUrl(typeof(TreeView), "iPower.Web.TreeView.Lplus.gif");
                            cell_0.Attributes.Add("ep", item.Expand ? "1" : "0");
                            cell_0.Attributes.Add("onclick", string.Format("javascript:{0}_CloseOpenTree(this,'{1}','{2}');",this.GetType().Name,
                                this.Page.ClientScript.GetWebResourceUrl(typeof(TreeView), "iPower.Web.TreeView.Lminus.gif"),
                                 this.Page.ClientScript.GetWebResourceUrl(typeof(TreeView), "iPower.Web.TreeView.Lplus.gif")
                                ));
                        }
                        else
                            image.ImageUrl = this.Page.ClientScript.GetWebResourceUrl(typeof(TreeView), "iPower.Web.TreeView.L.gif");
                    }
                    if (item.Childs.Count > 0)
                        cell_0.Style.Add(HtmlTextWriterStyle.Cursor, "pointer");
                    cell_0.Width = new Unit("17px");
                    cell_0.Controls.Add(image);
                    tr.Cells.Add(cell_0);

                    cell_1 = new TableCell();
                    image = new System.Web.UI.WebControls.Image();
                    if (item.Expand)
                        image.ImageUrl = this.Page.ClientScript.GetWebResourceUrl(typeof(TreeView), "iPower.Web.TreeView.openfolder.gif");
                    else
                        image.ImageUrl = this.Page.ClientScript.GetWebResourceUrl(typeof(TreeView), "iPower.Web.TreeView.closedfolder.gif");
                    cell_1.Width = new Unit("16px");
                    cell_1.Controls.Add(image);
                    tr.Cells.Add(cell_1);

                    if (this.ShowCheckBox)
                    {
                        TableCell cellBox = new TableCell();
                        cellBox.Width = new Unit("17px");
                        if (this.CheckType == CheckBoxType.CheckBox)
                        {
                            HtmlInputCheckBox checkBox = new HtmlInputCheckBox();
                            checkBox.ID = string.Format("{0}_chkb_{1}", this.ClientID, item.Value);
                            checkBox.Name = string.Format("{0}_chkb", this.ClientID);
                            checkBox.Value = item.Value;
                            if (this.EnabledCheckValueChecked)
                                checkBox.Checked = this.checkedValueCollection.Contains(item.Value);
                            else
                                checkBox.Checked = item.Checked;
                            checkBox.Attributes.Add("onclick", string.Format("javascript:{0}_CheckAll(this);", this.GetType().Name));
                            cellBox.Controls.Add(checkBox);
                        }
                        else
                        {
                            HtmlInputRadioButton radio = new HtmlInputRadioButton();
                            radio.ID = string.Format("{0}_rd_{1}", this.ClientID, item.Value);
                            radio.Name = string.Format("{0}_rd", this.ClientID);
                            radio.Value = item.Value;
                            if (this.EnabledCheckValueChecked)
                                radio.Checked = this.checkedValueCollection.Contains(item.Value);
                            else
                                radio.Checked = item.Checked;
                            cellBox.Controls.Add(radio);
                        }
                        tr.Cells.Add(cellBox);
                    }

                    cell_2 = new TableCell();
                    cell_2.Width = new Unit("100%");
                    cell_2.HorizontalAlign = HorizontalAlign.Left;
                    if (!string.IsNullOrEmpty(item.HrefURL) || !string.IsNullOrEmpty(item.ClickAction) || this.EnabledNodeClickEvent)
                    {
                        HtmlAnchor link = new HtmlAnchor();
                        link.Attributes.Add("class", "TreeViewA");
                        if (this.CurrentFolderValue == item.Value)
                        {
                            HtmlGenericControl span = new HtmlGenericControl("span");
                            span.Attributes.Add("class", "TreeViewCurrentNode");
                            span.InnerText = item.Text;
                            link.Controls.Add(span);
                        }
                        else
                            link.Controls.Add(new LiteralControl(item.Text));
                        link.HRef = string.IsNullOrEmpty(item.HrefURL) ? "#" : item.HrefURL;

                        if (this.EnabledNodeClickEvent && this.Page != null)
                            link.Attributes.Add("onclick", this.Page.ClientScript.GetPostBackEventReference(this, HttpUtility.HtmlEncode(item.Value)));
                        else if (!string.IsNullOrEmpty(item.ClickAction))
                            link.Attributes.Add("onclick", item.ClickAction);
                        cell_2.Controls.Add(link);
                        cell_2.Style.Add(HtmlTextWriterStyle.Cursor, "pointer");
                    }
                    else
                        cell_2.Text = item.Text;
                    tr.Cells.Add(cell_2);


                    tr.RenderControl(writer);

                    if (item.Childs.Count > 0)
                    {
                        tr = new TableRow();
                        if (!item.Expand)
                            tr.Style.Add(HtmlTextWriterStyle.Display, "none");
                        cell_0 = new TableCell();
                        cell_0.Width = new Unit("17px");
                        if (i < len - 1)
                        {
                            cell_0.Style.Add(HtmlTextWriterStyle.BackgroundImage, string.Format("url({0})",
                                this.Page.ClientScript.GetWebResourceUrl(typeof(TreeView), "iPower.Web.TreeView.I.gif")));
                        }
                        else if (i != len - 1)
                        {
                            image = new System.Web.UI.WebControls.Image();
                            image.ImageUrl = this.Page.ClientScript.GetWebResourceUrl(typeof(TreeView), "iPower.Web.TreeView.L.gif");
                            cell_0.Controls.Add(image);
                        }
                        tr.Cells.Add(cell_0);
                        cell_1 = new TableCell();
                        cell_1.Width = new Unit("100%");
                        cell_1.ColumnSpan = this.ShowCheckBox ? 3 : 2;
                        this.CreateTreeView(item, cell_1);
                        cell_1.VerticalAlign = VerticalAlign.Top;
                        tr.Cells.Add(cell_1);
                        tr.RenderControl(writer);
                    }
                }
            }
        }
        #endregion

        #region 数据处理。
        /// <summary>
        /// 
        /// </summary>
        protected virtual void CreateTreeView(TreeViewNode item, TableCell cell)
        {
            if (cell != null && item != null && item.Childs.Count > 0)
            {
                Table table = new Table();
                table.CellSpacing = table.CellPadding = 0;
                table.BorderWidth = new Unit("0");
                table.Width = table.Height = new Unit("100%");

                TreeViewNode node = null;
                TableRow tr = null;
                TableCell cell_0 = null, cell_1 = null, cell_2 = null;
                System.Web.UI.WebControls.Image image = null;
                int len = item.Childs.Count;
                for (int i = 0; i < len; i++)
                {
                    node = item.Childs[i];
                    tr = new TableRow();
                    
                    cell_0 = new TableCell();
                    image = new System.Web.UI.WebControls.Image();
                    if (i < len - 1)
                    {
                        if (node.Childs.Count > 0)
                        {
                            image.ImageUrl = node.Expand ?
                                this.Page.ClientScript.GetWebResourceUrl(typeof(TreeView), "iPower.Web.TreeView.Tminus.gif") :
                                this.Page.ClientScript.GetWebResourceUrl(typeof(TreeView), "iPower.Web.TreeView.Tplus.gif");
                            cell_0.Attributes.Add("ep", node.Expand ? "1" : "0");
                            cell_0.Attributes.Add("onclick", string.Format("javascript:{0}_CloseOpenTree(this,'{1}','{2}');", this.GetType().Name,
                                this.Page.ClientScript.GetWebResourceUrl(typeof(TreeView), "iPower.Web.TreeView.Tminus.gif"),
                                 this.Page.ClientScript.GetWebResourceUrl(typeof(TreeView), "iPower.Web.TreeView.Tplus.gif")
                                ));

                        }else
                            image.ImageUrl = this.Page.ClientScript.GetWebResourceUrl(typeof(TreeView), "iPower.Web.TreeView.T.gif");
                    }
                    else
                    {
                        if (node.Childs.Count > 0)
                        {
                            image.ImageUrl = node.Expand ?
                                this.Page.ClientScript.GetWebResourceUrl(typeof(TreeView), "iPower.Web.TreeView.Lminus.gif") :
                           this.Page.ClientScript.GetWebResourceUrl(typeof(TreeView), "iPower.Web.TreeView.Lplus.gif");
                            cell_0.Attributes.Add("ep", node.Expand ? "1" : "0");
                            cell_0.Attributes.Add("onclick", string.Format("javascript:{0}_CloseOpenTree(this,'{1}','{2}');", this.GetType().Name,
                                this.Page.ClientScript.GetWebResourceUrl(typeof(TreeView), "iPower.Web.TreeView.Lminus.gif"),
                                 this.Page.ClientScript.GetWebResourceUrl(typeof(TreeView), "iPower.Web.TreeView.Lplus.gif")
                                ));
                        }
                        else
                            image.ImageUrl = this.Page.ClientScript.GetWebResourceUrl(typeof(TreeView), "iPower.Web.TreeView.L.gif");
                    }
                    if (node.Childs.Count > 0)
                        cell_0.Style.Add(HtmlTextWriterStyle.Cursor, "pointer");
                    cell_0.Width = new Unit("17px");
                    cell_0.Controls.Add(image);
                    tr.Cells.Add(cell_0);

                    cell_1 = new TableCell();
                    image = new System.Web.UI.WebControls.Image();
                    if (node.Expand)
                        image.ImageUrl = this.Page.ClientScript.GetWebResourceUrl(typeof(TreeView), "iPower.Web.TreeView.openfolder.gif");
                    else
                        image.ImageUrl = this.Page.ClientScript.GetWebResourceUrl(typeof(TreeView), "iPower.Web.TreeView.closedfolder.gif");
                    cell_1.Width = new Unit("16px");
                    cell_1.Controls.Add(image);
                    tr.Cells.Add(cell_1);

                    if (this.ShowCheckBox)
                    {
                        TableCell cellBox = new TableCell();
                        cellBox.Width = new Unit("17px");
                        if (this.CheckType == CheckBoxType.CheckBox)
                        {
                            HtmlInputCheckBox checkBox = new HtmlInputCheckBox();
                            checkBox.ID = string.Format("{0}_chkb_{1}", this.ClientID, node.Value);
                            checkBox.Name = string.Format("{0}_chkb", this.ClientID);
                            checkBox.Value = node.Value;
                            if (this.EnabledCheckValueChecked)
                                checkBox.Checked = this.checkedValueCollection.Contains(node.Value);
                            else
                                checkBox.Checked = node.Checked;
                            checkBox.Attributes.Add("onclick", string.Format("javascript:{0}_CheckAll(this);", this.GetType().Name));
                            cellBox.Controls.Add(checkBox);
                        }
                        else
                        {
                            HtmlInputRadioButton radio = new HtmlInputRadioButton();
                            radio.ID = string.Format("{0}_rd_{1}", this.ClientID, node.Value);
                            radio.Name = string.Format("{0}_rd", this.ClientID);
                            radio.Value = node.Value;
                            if (this.EnabledCheckValueChecked)
                                radio.Checked = this.checkedValueCollection.Contains(node.Value);
                            else
                                radio.Checked = node.Checked;
                            cellBox.Controls.Add(radio);
                        }
                        tr.Cells.Add(cellBox);
                    }

                    cell_2 = new TableCell();
                    cell_2.Width = new Unit("100%");
                    cell_2.HorizontalAlign = HorizontalAlign.Left;
                    if (!string.IsNullOrEmpty(node.HrefURL) || !string.IsNullOrEmpty(node.ClickAction) || this.EnabledNodeClickEvent)
                    {
                        HtmlAnchor link = new HtmlAnchor();
                        link.Attributes.Add("class", "TreeViewA");
                        if (this.CurrentFolderValue == node.Value)
                        {
                            HtmlGenericControl span = new HtmlGenericControl("span");
                            span.Attributes.Add("class", "TreeViewCurrentNode");
                            span.InnerText = node.Text;
                            link.Controls.Add(span);
                        }
                        else
                            link.Controls.Add(new LiteralControl(node.Text));
                        link.HRef = string.IsNullOrEmpty(node.HrefURL) ? "#" : node.HrefURL;

                        if (this.EnabledNodeClickEvent && this.Page != null)
                            link.Attributes.Add("onclick", this.Page.ClientScript.GetPostBackEventReference(this, HttpUtility.HtmlEncode(node.Value)));
                        else if (!string.IsNullOrEmpty(node.ClickAction))
                            link.Attributes.Add("onclick", node.ClickAction);
                        cell_2.Controls.Add(link);
                        cell_2.Style.Add(HtmlTextWriterStyle.Cursor, "pointer");
                    }
                    else
                        cell_2.Text = node.Text;
                    tr.Cells.Add(cell_2);
                    table.Rows.Add(tr);

                    if (node.Childs.Count > 0)
                    {
                        tr = new TableRow();
                        if (!node.Expand)
                            tr.Style.Add(HtmlTextWriterStyle.Display, "none");

                        cell_0 = new TableCell();
                        cell_0.Width = new Unit("17px");
                        if (i < len - 1)
                        {
                            cell_0.Style.Add(HtmlTextWriterStyle.BackgroundImage, string.Format("url({0})",
                                this.Page.ClientScript.GetWebResourceUrl(typeof(TreeView), "iPower.Web.TreeView.I.gif")));
                        }
                        else if (i != len - 1)
                        {
                            image = new System.Web.UI.WebControls.Image();
                            image.ImageUrl = this.Page.ClientScript.GetWebResourceUrl(typeof(TreeView), "iPower.Web.TreeView.L.gif");
                            cell_0.Controls.Add(image);
                        }
                        tr.Cells.Add(cell_0);
                        cell_1 = new TableCell();
                        cell_1.Width = new Unit("100%");
                        cell_1.ColumnSpan = this.ShowCheckBox ? 3 : 2;
                        this.CreateTreeView(node, cell_1);
                        tr.Cells.Add(cell_1);
                        table.Rows.Add(tr);
                    }
                }
                cell.Controls.Add(table);
            }
        }
        #endregion

        #region 辅助函数。
        void CloseExpand(TreeViewNode node)
        {
            if (node != null)
            {
                node.Expand = false;
                foreach (TreeViewNode n in node.Childs)
                {
                    this.CloseExpand(n);
                }
            }
        }
        void OpenExpand(TreeViewNode node)
        {
            if (node != null)
            {
                node.Expand = true;
                this.OpenExpand(node.Parent);
            }
        }
        #endregion
    }

    partial class TreeView : IPostBackEventHandler
    {
        #region 事件。
        /// <summary>
        /// 获取或设置是否启用节点单击事件。
        /// </summary>
        [Category("Events")]
        [Description("获取或设置是否启用节点单击事件。")]
        public bool EnabledNodeClickEvent
        {
            get
            {
                object obj = this.ViewState["EnabledNodeClickEvent"];
                return (obj != null) && (bool)obj;
            }
            set
            {
                if (this.EnabledNodeClickEvent != value)
                {
                    this.ViewState["EnabledNodeClickEvent"] = value;
                }
            }
        }
        /// <summary>
        /// 节点点击事件。
        /// </summary>
        [Category("Events")]
        [Description(" 节点点击事件。")]
        public event TreeViewNodeClickHandler NodeClick;
        /// <summary>
        /// 触发<see cref="NodeClick"/>事件。
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnNodeClick(TreeViewNodeClickEventArgs e)
        {
            TreeViewNodeClickHandler handler = this.NodeClick;
            if (handler != null)
                handler(this, e);
        }
        #endregion


        #region  辅助函数。
        /// <summary>
        /// 构建数据。
        /// </summary>
        protected virtual void BuildItems()
        {
            this.Items.Clear();
            DataTable dtSource = null;
            if (this.dataSource is ICollection)
                dtSource = this.BuildICollection((ICollection)this.dataSource);
            else if (this.dataSource is DataTable)
                dtSource = this.dataSource as DataTable;
            if (dtSource != null)
                this.BuildDataTableItems(dtSource);
        }

        #region 数据源为 DataTable。
        void BuildDataTableItems(DataTable dtSource)
        {
            if (dtSource != null)
            {
                DataSet set = new DataSet();
                set.Tables.Add(dtSource.Copy());
                set.Tables[0].PrimaryKey = new DataColumn[] { set.Tables[0].Columns[this.IDField] };
                if (!set.Tables[0].Columns.Contains(this.StatusField))
                    set.Tables[0].Columns.Add(this.StatusField, typeof(string));

                bool expandFirstLevel = this.ExpandFirstLevel;
                bool expandAllLevel = this.ExpandAllLevel;
                bool IsParent = !string.IsNullOrEmpty(this.PIDField);
                foreach (DataRow row in set.Tables[0].Rows)
                {
                    if (IsParent && (set.Tables[0].Rows.Find(row[this.PIDField]) == null))
                    {
                        row[this.PIDField] = DBNull.Value;
                        if (expandFirstLevel)
                            row[this.StatusField] = "expand";
                    }
                    if (expandAllLevel)
                        row[this.StatusField] = "expand";
                }

                DataRow[] drs = IsParent ? set.Tables[0].Select(string.Format("isnull({0},'') = ''", this.PIDField), this.OrderNoField) :
                                           set.Tables[0].Select(string.Empty, this.OrderNoField);
                if (IsParent)
                    set.Relations.Add("TreeRelation", set.Tables[0].Columns[this.IDField], set.Tables[0].Columns[this.PIDField]);

                bool bClick = !string.IsNullOrEmpty(this.ClickActionField);
                bool bHref = !string.IsNullOrEmpty(this.HrefField);
                bool bStatus = !string.IsNullOrEmpty(this.StatusField);
                foreach (DataRow row in drs)
                {
                    TreeViewNode item = new TreeViewNode(
                                                         Convert.ToString(row[this.TitleField]),
                                                         Convert.ToString(row[this.IDField])
                                                         );
                    if (bClick)
                        item.ClickAction = Convert.ToString(row[this.ClickActionField]);
                    if (bHref)
                        item.HrefURL = Convert.ToString(row[this.HrefField]);
                    if (bStatus)
                        item.SetExpand(string.Equals(Convert.ToString(row[this.StatusField]), "expand", StringComparison.InvariantCultureIgnoreCase) ? true : false);

                    if (this.ShowCheckBox && this.checkedValueCollection != null && this.checkedValueCollection.Count > 0)
                        item.Checked = this.checkedValueCollection.Contains(item.Value);

                    if (!string.IsNullOrEmpty(this.OrderNoField))
                        item.OrderNo = Convert.ToString(row[this.OrderNoField]);

                    if (IsParent)
                        this.BuildDataRowItems(item, row.GetChildRows("TreeRelation"), bClick, bHref, bStatus, this.ShowCheckBox);

                    this.Items.Add(item);
                }
                set.Dispose();
            }
        }
        void BuildDataRowItems(TreeViewNode parent, DataRow[] drs, bool bClick, bool bHref, bool bStatus, bool bChecked)
        {
            if (parent != null && drs != null)
            {
                foreach (DataRow row in drs)
                {
                    TreeViewNode item = new TreeViewNode(parent,
                                                         Convert.ToString(row[this.TitleField]),
                                                         Convert.ToString(row[this.IDField])
                                                         );
                    if (bClick)
                        item.ClickAction = Convert.ToString(row[this.ClickActionField]);
                    if (bHref)
                        item.HrefURL = Convert.ToString(row[this.HrefField]);
                    if (bStatus)
                        item.SetExpand(string.Equals(Convert.ToString(row[this.StatusField]), "expand", StringComparison.InvariantCultureIgnoreCase) ? true : false);

                    if (bChecked && this.checkedValueCollection != null && this.checkedValueCollection.Count > 0)
                        item.Checked = this.checkedValueCollection.Contains(item.Value);

                    if (!string.IsNullOrEmpty(this.OrderNoField))
                        item.OrderNo = Convert.ToString(row[this.OrderNoField]);

                    this.BuildDataRowItems(item, row.GetChildRows("TreeRelation"), bClick, bHref, bStatus, bChecked);

                    parent.Childs.Add(item);
                }
            }
        }
        #endregion

        #region 数据源为 ICollection。
        DataTable BuildICollection(ICollection dataSource)
        {
            DataTable dtSource = new DataTable();
            dtSource.Columns.Add(this.IDField, typeof(string));
            dtSource.Columns.Add(this.PIDField, typeof(string));
            dtSource.Columns.Add(this.TitleField, typeof(string));
            if (!string.IsNullOrEmpty(this.OrderNoField) && !dtSource.Columns.Contains(this.OrderNoField))
                dtSource.Columns.Add(this.OrderNoField, typeof(string));
            if (!string.IsNullOrEmpty(this.ClickActionField) && !dtSource.Columns.Contains(this.ClickActionField))
                dtSource.Columns.Add(this.ClickActionField, typeof(string));
            if (!string.IsNullOrEmpty(this.HrefField) && !dtSource.Columns.Contains(this.HrefField))
                dtSource.Columns.Add(this.HrefField, typeof(string));

            if (dataSource != null && dataSource.Count > 0)
            {
                IEnumerator enumerator = dataSource.GetEnumerator();
                PropertyDescriptorCollection pdc = null;
                PropertyDescriptor pd = null;
                DataRow dr = null;
                while (enumerator.MoveNext())
                {
                    object obj = enumerator.Current;
                    if (obj != null)
                    {
                        dr = dtSource.NewRow();
                        pdc = TypeDescriptor.GetProperties(obj);
                        if (pdc != null && pdc.Count > 0)
                        {
                            foreach (DataColumn col in dtSource.Columns)
                            {
                                pd = pdc.Find(col.ColumnName, true);
                                if (pd != null)
                                    dr[col.ColumnName] = pd.GetValue(obj);
                            }
                            dtSource.Rows.Add(dr);
                        }
                    }
                }
            }
            return dtSource;
        }
        #endregion

        #endregion

        #region 公开函数。
        /// <summary>
        /// 构建树。
        /// </summary>
        public void BuildTree()
        {
            this.BuildItems();
            this.DataBind();
        }
        #endregion

        #region IPostBackEventHandler 成员
        /// <summary>
        /// 使服务器控件能够处理将窗体发送到服务器时引发的事件。
        /// </summary>
        /// <param name="eventArgument">表示要传递到事件处理程序的可选事件参数。</param>
        public void RaisePostBackEvent(string eventArgument)
        {
            if (!string.IsNullOrEmpty(eventArgument))
            {
                TreeViewNode node = this.Items[eventArgument];
                if (node != null)
                {
                    node.ChangedExpand += new EventHandler<TreeViewNodeClickEventArgs>(delegate(object sender, TreeViewNodeClickEventArgs e)
                    {
                        if (e.Node != null)
                        {
                            if (e.Node.Expand && e.Node.Parent != null)
                                this.OpenExpand(e.Node.Parent);
                            else if (!e.Node.Expand && e.Node.Childs.Count > 0)
                            {
                                foreach (TreeViewNode n in e.Node.Childs)
                                    this.CloseExpand(n);
                            }
                        }
                    });
                    this.OnNodeClick(new TreeViewNodeClickEventArgs(node));
                }
            }
        }

        #endregion

        /// <summary>
        /// 选择框类型枚举。
        /// </summary>
        public enum CheckBoxType
        {
            /// <summary>
            /// 无。
            /// </summary>
            None,
            /// <summary>
            /// CheckBox。
            /// </summary>
            CheckBox,
            /// <summary>
            /// Radio。
            /// </summary>
            RadioButton
        }
    }

    /// <summary>
    /// 单击委托。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void TreeViewNodeClickHandler(object sender,TreeViewNodeClickEventArgs e);
    /// <summary>
    /// 为单击事件提供数据。
    /// </summary>
    public class TreeViewNodeClickEventArgs : EventArgs
    {
        #region 成员变量，构造函数。
        TreeViewNode node;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="node"></param>
        public TreeViewNodeClickEventArgs(TreeViewNode node)
        {
            this.node = node;
        }
        #endregion

        /// <summary>
        /// 获取节点对象。
        /// </summary>
        public TreeViewNode Node
        {
            get { return this.node; }
        }
    }
}
