//================================================================================
//  FileName: UploadView.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/4/19
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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Data;
using System.Drawing;
using System.ComponentModel;

using System.IO;
using System.IO.Compression;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

[assembly: WebResource("iPower.Web.Upload.UploadView.js", "text/javascript")]
namespace iPower.Web.Upload
{
    /// <summary>
    /// 上传异常委托。
    /// </summary>
    /// <param name="e"></param>
    public delegate void UploadViewExceptionHandler(Exception e);
    /// <summary>
    /// 上传处理控件。
    /// </summary>
    [ToolboxData("<{0}:UploadView runat='server'></{0}:UploadView>")]
    public class UploadView : WebControl, IPostBackEventHandler
    {
        #region 成员变量，构造函数。
        UploadViewDataItemCollection items;
        object dataSource = null;
        double maxUploadSize = 0;
        int maxUploadCount = 0;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public UploadView()
        {
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 数据项集合。
        /// </summary>
        [Browsable(false)]
        [Bindable(false)]
        public UploadViewDataItemCollection Items
        {
            get
            {
                if (this.items == null)
                    this.items = new UploadViewDataItemCollection();
                return this.items;
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
        /// 获取或设置上传最大的文件大小(为0不限制)。
        /// </summary>
        [Category("Data")]
        [Description("获取或设置上传最大的文件大小(为0不限制)。")]
        public double MaxUploadSize
        {
            get { return this.maxUploadSize; }
            set { this.maxUploadSize = value; }
        }
        /// <summary>
        /// 获取或设置上传的文件个数(为0不限制)。
        /// </summary>
        [Category("Data")]
        [Description("获取或设置上传的文件个数(为0不限制)。")]
        public int MaxUploadCount
        {
            get { return this.maxUploadCount; }
            set { this.maxUploadCount = value; }
        }
        /// <summary>
        /// 获取或设置是否显示删除按钮。
        /// </summary>
        [Category("Data")]
        [Description("获取或设置是否显示删除按钮。")]
        public bool ShowDeleteButtom
        {
            get
            {
                object obj = this.ViewState["ShowDeleteButtom"];
                return obj == null ? true : (bool)obj;
            }
            set
            {
                this.ViewState["ShowDeleteButtom"] = value;
            }
        }
        /// <summary>
        /// 获取或设置允许Office在线编辑。
        /// </summary>
        [Category("Data")]
        [Description("获取或设置允许Office在线编辑。")]
        public bool AllowOfficeOnlineEdit
        {
            get
            {
                object obj = this.ViewState["AllowOfficeOnlineEdit"];
                return (obj != null) && (bool)obj;
            }
            set
            {
                if (this.AllowOfficeOnlineEdit != value)
                {
                    this.ViewState["AllowOfficeOnlineEdit"] = value;
                }
            }
        }
        /// <summary>
        /// 获取或设置Office文件后缀名称。
        /// </summary>
        [Category("Data")]
        [Description("获取或设置Office文件后缀名称(多个后缀用|隔开)。")]
        public string OfficeSuffix
        {
            get
            {
                object obj = this.ViewState["OfficeSuffix"];
                return obj == null ?  ".doc|.docx|.xls|.xlsx|.xml" : obj.ToString();
            }
            set
            {
                this.ViewState["OfficeSuffix"] = value;
            }
        }
        /// <summary>
        /// 获取删除的文件ID。
        /// </summary>
        [Category("Data")]
        [Description("获取删除的文件ID。")]
        public StringCollection DeleteFileID
        {
            get
            {
                object obj = this.ViewState["DeleteFileID"];
                return obj == null ? new StringCollection() : (StringCollection)obj;
            }
            private set
            {
                this.ViewState["DeleteFileID"] = value;
            }
        }
        /// <summary>
        /// 获取或设置是否只读。
        /// </summary>
        [Category("Data")]
        [Description("获取或设置是否只读。")]
        public bool ReadOnly
        {
            get
            {
                object obj = this.ViewState["ReadOnly"];
                return (obj != null) && (bool)obj;
            }
            set
            {
                if (this.ReadOnly != value)
                    this.ViewState["ReadOnly"] = value;
            }
        }
        /// <summary>
        /// 获取或设置是否允许下载文件。
        /// </summary>
        [Category("Data")]
        [Description("获取或设置是否允许下载文件。")]
        public bool AllowDownload
        {
            get
            {
                object obj = this.ViewState["AllowDownload"];
                return (obj == null) ? true : (bool)obj;
            }
            set
            {
                if (this.AllowDownload != value)
                    this.ViewState["AllowDownload"] = value;
            }
        }
        /// <summary>
        /// 获取或设置下载页面根URI。
        /// </summary>
        [Category("Data")]
        [Description("获取或设置下载页面根URI。")]
        public string DownloadBaseURI
        {
            get
            {
                object obj = this.ViewState["DownloadBaseURI"];
                return (obj == null) ? string.Empty : (string)obj;
            }
            set
            {
                this.ViewState["DownloadBaseURI"] = value;
            }
        }
        /// <summary>
        /// 获取或设置数据样式。
        /// </summary>
        [Category("Styles")]
        [Description("获取或设置数据样式。")]
        public string DataItemClass
        {
            get
            {
                object obj = this.ViewState["DataItemClass"];
                return obj == null ? "DataGrid" : (string)obj;
            }
            set
            {
                this.ViewState["DataItemClass"] = value;
            }
        }
        /// <summary>
        /// 获取或设置标题样式。
        /// </summary>
        [Category("Styles")]
        [Description("获取或设置标题样式。")]
        public string DataItemHeaderClass
        {
            get
            {
                object obj = this.ViewState["DataItemHeaderClass"];
                return obj == null ? "DataGridHeader" : (string)obj;
            }
            set
            {
                this.ViewState["DataItemHeaderClass"] = value;
            }
        }
        /// <summary>
        /// 获取或设置数据普通行样式。
        /// </summary>
        [Category("Styles")]
        [Description("获取或设置数据普通行样式。")]
        public string DataItemNormalClass
        {
            get
            {
                object obj = this.ViewState["DataItemNormalClass"];
                return obj == null ? "DataGridItem" : (string)obj;
            }
            set
            {
                this.ViewState["DataItemNormalClass"] = value;
            }
        }
        /// <summary>
        /// 获取或设置数据交替行样式。
        /// </summary>
        [Category("Styles")]
        [Description("获取或设置数据交替行样式。")]
        public string DataItemAlterClass
        {
            get
            {
                object obj = this.ViewState["DataItemAlterClass"];
                return obj == null ? "DataGridAlter" : (string)obj;
            }
            set
            {
                this.ViewState["DataItemAlterClass"] = value;
            }
        }

        /// <summary>
        /// 获取或设置标题样式。
        /// </summary>
        [Category("Styles")]
        [Description("获取或设置标题样式。")]
        public string DataItemFooterClass
        {
            get
            {
                object obj = this.ViewState["DataItemFooterClass"];
                return obj == null ? "DataGridFooter" : (string)obj;
            }
            set
            {
                this.ViewState["DataItemFooterClass"] = value;
            }
        }
        #endregion

        #region 事件。
        /// <summary>
        /// 异常事件。
        /// </summary>
        public event UploadViewExceptionHandler UploadViewExceptionEvent;
        /// <summary>
        /// 触发异常。
        /// </summary>
        /// <param name="e"></param>
        protected void OnUploadViewExceptionEvent(Exception e)
        {
            if (e != null)
            {
                UploadViewExceptionHandler handler = this.UploadViewExceptionEvent;
                if (handler != null)
                    handler(e);
                else
                    throw e;
            }
        }
        #endregion

        #region 重载。
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
                Pair p = savedState as Pair;
                if (p != null)
                {
                    base.LoadViewState(p.First);
                    this.Items.LoadViewState(p.Second);
                }
            }
            else
                base.LoadViewState(savedState);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override object SaveViewState()
        {
            object x = base.SaveViewState();
            object y = this.Items.SaveViewState();
            return new Pair(x, y);
        }
        /// <summary>
        /// 
        /// </summary>
        protected override void TrackViewState()
        {
            base.TrackViewState();
            this.Items.TrackViewState();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (this.Page != null)
            {
                HtmlForm form = this.Page.Form;
                if ((form != null) && string.IsNullOrEmpty(form.Enctype))
                {
                    form.Enctype = "multipart/form-data";
                }
            }
            ClientScriptManager csm = this.Page.ClientScript;
            if (csm != null)
            {
                string strKey = string.Format("UploadView_doPostBack");
                if (!csm.IsClientScriptBlockRegistered(typeof(UploadView), strKey))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("<script type=\"text/javascript\">");
                    sb.AppendLine("<!--");
                    sb.AppendLine("function " + strKey + "(cmd)");
                    sb.AppendLine("{");
                    sb.AppendLine("\tif(cmd == 'btnSave')");
                    sb.AppendLine("\t\t" + csm.GetPostBackEventReference(this, "btnSave"));
                    sb.AppendLine("\telse if(cmd == 'btnDelete')");
                    sb.AppendLine("\t\t" + csm.GetPostBackEventReference(this, "btnDelete"));
                    sb.AppendLine("\telse");
                    sb.AppendLine("\t\talert('命令未知！')");
                    sb.AppendLine("}");
                    sb.AppendLine("//-->");
                    sb.AppendLine("</script>");

                    csm.RegisterClientScriptBlock(typeof(UploadView), strKey, sb.ToString());
                }
                csm.RegisterClientScriptResource(typeof(UploadView), "iPower.Web.Upload.UploadView.js");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (this.Page != null)
                this.Page.VerifyRenderingInServerForm(this);
            base.Render(writer);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
            if (string.IsNullOrEmpty(this.CssClass))
                writer.AddAttribute(HtmlTextWriterAttribute.Class, this.DataItemClass);
            base.AddAttributesToRender(writer);
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            string[][] headers = new string[][] { new string[]{"文件名","65%"},
                                                  new string[]{"后缀名","15%"},
                                                  new string[]{"文件大小(M)","20%"}};
            #region 表头。
            TableRow headerRow = new TableRow();
            headerRow.CssClass = this.DataItemHeaderClass;
            TableCell cell = null;
            if (!this.ReadOnly && this.ShowDeleteButtom)
            {
                cell = new TableCell();
                cell.Width = new Unit("8px");
                HtmlInputCheckBox chk = new HtmlInputCheckBox();
                chk.ID = string.Format("{0}_chkSelectAll", this.ClientID);
                chk.Attributes["onclick"] = string.Format("javascript:UploadView_SelectAll('{0}',this);", this.ClientID);
                cell.Controls.Add(chk);
                headerRow.Cells.Add(cell);
            }
            foreach (string[] h in headers)
            {
                cell = new TableCell();
                cell.Width = new Unit(h[1]);
                cell.Text = h[0];
                headerRow.Cells.Add(cell);
            }
            headerRow.RenderControl(writer);
            #endregion

            #region 表体。
            UploadViewDataItemCollection itemCollection = this.Items;
            if (itemCollection != null && itemCollection.Count > 0)
            {
                int len = itemCollection.Count;
                bool bReadOnly = this.ReadOnly;
                if (!bReadOnly)
                    bReadOnly = !this.ShowDeleteButtom;
                bool bAllowDown = this.AllowDownload;
                for (int i = 0; i < len; i++)
                {
                    this.CreateDataItem(i, bReadOnly, bAllowDown, itemCollection[i], writer);
                }
            }
            #endregion

            if (!this.ReadOnly)
            {
                TableRow footer = new TableRow();
                footer.CssClass = this.DataItemFooterClass;
                TableCell footerCell = new TableCell();
                footerCell.ColumnSpan = headers.Length + 1;
                               
                this.CreateUploadDeleteBtn(footerCell);
                this.CreateUpload(footerCell);

                footer.Cells.Add(footerCell);
                footer.RenderControl(writer);
            }
        }
        #endregion

        #region 辅助函数。
        void CreateDataItem(int index, bool bReadOnly, bool bAllowDownd, UploadViewDataItem item, HtmlTextWriter writer)
        {
            if (item != null)
            {
                TableRow row = new TableRow();
                row.CssClass = (index % 2 == 0) ? this.DataItemNormalClass : this.DataItemAlterClass;
                TableCell cell = null;
                if (!bReadOnly)
                {
                    cell = new TableCell();
                    cell.HorizontalAlign = HorizontalAlign.Center;
                    HtmlInputCheckBox chk = new HtmlInputCheckBox();
                    chk.ID = chk.Name = string.Format("{0}_CHK_{1}", this.ClientID, item.FileID);
                    chk.Value = item.FileID;
                    cell.Controls.Add(chk);
                    row.Cells.Add(cell);
                }
                string downloadBaseURI = this.DownloadBaseURI;
                if (bAllowDownd && !this.UploadRaws.ContainsKey(item.FileID) && !string.IsNullOrEmpty(downloadBaseURI))
                {
                    cell = new TableCell();
                    cell.HorizontalAlign = HorizontalAlign.Left;
                    HtmlAnchor a = new HtmlAnchor();
                    a.InnerText = string.Format("{0}.{1}", index + 1, item.FileName);
                    string[] exts = this.OfficeSuffix.Split('|');
                    if (!this.ReadOnly && this.AllowOfficeOnlineEdit && exts != null && exts.Length > 0 && (Array.BinarySearch(exts, item.Extension.ToLower()) > -1))
                    {
                        string url = string.Empty;
                        if (downloadBaseURI.IndexOf("/") > 0)
                            url = downloadBaseURI.Substring(0, downloadBaseURI.LastIndexOf("/"));
                        if (string.IsNullOrEmpty(url))
                            url = string.Format("http://{0}/", HttpContext.Current.Request.Url.Host);
                        url = string.Format("{0}{1}{2}", url, item.FileID, item.Extension);
                        a.HRef = "#";
                        a.Attributes["onclick"] = string.Format("javascript:UploadView_DocumentEdit('{0}');", url);
                    }
                    else
                    {
                        a.HRef = string.Format("{0}{1}", downloadBaseURI, item.FileID);
                        a.Target = "_blank";
                    }
                    cell.Controls.Add(a);
                    row.Cells.Add(cell);
                }
                else
                {
                    cell = new TableCell();
                    cell.HorizontalAlign = HorizontalAlign.Left;
                    cell.Text = string.Format("{0}.{1}", index + 1, item.FileName);
                    row.Cells.Add(cell);
                }

                cell = new TableCell();
                cell.HorizontalAlign = HorizontalAlign.Left;
                cell.Text = item.Extension;
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.HorizontalAlign = HorizontalAlign.Right;
                cell.Text = item.Size < 0.01 ? "< 0.01" : string.Format("{0:N2}", item.Size);
                row.Cells.Add(cell);

                row.RenderControl(writer);
            }
        }
        void CreateUploadDeleteBtn(TableCell parent)
        {
            HtmlGenericControl div = new HtmlGenericControl("div");
            div.Attributes["style"] = "float:left;width:100%;";

            if (this.Items.Count > 0 && this.ShowDeleteButtom)
            {
                HtmlInputButton btnDelete = new HtmlInputButton("button");
                btnDelete.ID = string.Format("{0}_btnDelete", this.ClientID);
                btnDelete.Attributes["style"] = "float:right;";
                btnDelete.Attributes["onclick"] = string.Format("javascript:UploadView_UploadDelete('{0}')", this.ClientID);
                btnDelete.Value = "删除";
                div.Controls.Add(btnDelete);
            }
            HtmlInputButton btnUpload = new HtmlInputButton("button");
            btnUpload.ID = string.Format("{0}_btnUpload", this.ClientID);
            btnUpload.Value = "上传";
            btnUpload.Attributes["style"] = "float:right;";
            btnUpload.Attributes["onclick"] = string.Format("javascript:UploadView_DisplayUploadControl('{0}',this);", this.ClientID);
            div.Controls.Add(btnUpload);

            parent.Controls.Add(div);
        }
        void CreateUpload(TableCell parent)
        {
            HtmlGenericControl uploadControlLayer = new HtmlGenericControl("div");
            uploadControlLayer.ID = string.Format("{0}_UploadControlLayer", this.ClientID);
            uploadControlLayer.Attributes["style"] = string.Format("position:absolute;z-index:99999;width:{0};margin-top:24px; background-color:#fff;border:solid 1px #ccc; display:none;", this.Width);
            HtmlGenericControl div = new HtmlGenericControl("div");
            div.Attributes["style"] = "float:left;width:100%;";

            HtmlGenericControl span = new HtmlGenericControl("span");
            span.Attributes["style"] = "float:left;";
            span.InnerText = "上传文件：";
            div.Controls.Add(span);

            HtmlInputFile file = new HtmlInputFile();
            file.Attributes["style"] = "float:left;width:70%;";
            file.ID = string.Format("{0}_UploadFile", this.ClientID);
            div.Controls.Add(file);

            HtmlInputButton btnSave = new HtmlInputButton("button");
            btnSave.ID = string.Format("{0}_btnSave", this.ClientID);
            btnSave.Value = "确定";
            btnSave.Attributes["style"] = "float:left;";
            btnSave.Attributes["onclick"] = string.Format("javascript:UploadView_UploadSave('{0}');", this.ClientID);
            div.Controls.Add(btnSave);

            uploadControlLayer.Controls.Add(div);
            parent.Controls.Add(uploadControlLayer);
        }
        #endregion

        #region IPostBackEventHandler 成员
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventArgument"></param>
        public void RaisePostBackEvent(string eventArgument)
        {
            if (!string.IsNullOrEmpty(eventArgument))
            {
                bool result = false;
                switch (eventArgument)
                {
                    case "btnSave":
                        result = this.SaveUpload();
                        break;
                    case "btnDelete":
                        result = this.DeleteUpload();
                        break;
                }

                if (result)
                    this.DataBind();
            }
        }

        #endregion

        #region 事件处理。
        bool SaveUpload()
        {
            if (this.HasFile)
            {
                try
                {
                    HttpPostedFile postedFile = this.PostedFile;
                    UploadViewDataItem item = new UploadViewDataItem();
                    item.FileID = GUIDEx.New;
                    item.FileName = Path.GetFileName(postedFile.FileName);
                    item.Extension = Path.GetExtension(postedFile.FileName);
                    item.Size = (double)(postedFile.ContentLength / (double)1024 / (double)1024);
                    if (this.MaxUploadSize > 0)
                    {
                        if (item.Size > this.MaxUploadSize)
                            throw new Exception(string.Format("上传文件为{0:N2}Mb超过了允许上传的最大上限（{1:N2}Mb）！", item.Size, this.MaxUploadSize));
                    }
                    if (this.MaxUploadCount > 0)
                    {
                        if (this.Items.Count + 1 > this.MaxUploadCount)
                            throw new Exception(string.Format("上传的文件个数({0}个)超过了允许上传的最大个数({1}个)。", this.Items.Count + 1, this.MaxUploadCount));
                    }
                    this.Items.Add(item);
                    this.SaveFileRawToPage(item.FileID, postedFile.ContentType, postedFile.InputStream);
                    return true;
                }
                catch (Exception e)
                {
                    this.OnUploadViewExceptionEvent(e);
                }
            }
            return false;
        }
        bool DeleteUpload()
        {
            if (this.Page != null && this.Page.Request != null)
            {
                List<GUIDEx> list = new List<GUIDEx>();
                string strarts = string.Format("{0}_CHK_", this.ClientID);
                foreach (string key in this.Page.Request.Form.AllKeys)
                {
                    if (key.StartsWith(strarts))
                    {
                        list.Add(this.Page.Request[key]);
                    }
                }

                if (list.Count > 0)
                {
                    StringCollection listFileID = this.FileIDFromDataSource;
                    UploadViewDataItem item = null;
                    bool status = false;
                    StringCollection result = new StringCollection();
                    foreach (GUIDEx fid in list)
                    {
                        item = this.Items[fid];
                        if (item != null)
                        {
                            status = this.Items.Remove(item);
                            if (!this.DeleteFileRawToPage(fid) && listFileID.Contains(fid))
                            {
                                result.Add(fid);
                            }
                        }
                    }
                    this.DeleteFileID = result;
                    return status;
                }
            }
            return false;
        }
        #endregion

        #region 处理文件数据。
        /// <summary>
        /// 获取客户端上传文件。
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        protected HttpPostedFile PostedFile
        {
            get
            {
                if ((this.Page != null) && this.Page.IsPostBack)
                {
                    return this.Context.Request.Files[string.Format("{0}_UploadFile", this.ClientID)];
                }
                return null;
            }
        }
        /// <summary>
        /// 是否有上传文件。
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        protected bool HasFile
        {
            get
            {
                HttpPostedFile postedFile = this.PostedFile;
                return (postedFile != null) && (postedFile.ContentLength > 0);
            }
        }
        /// <summary>
        /// 获取或设置上传的数据。
        /// </summary>
        protected Dictionary<GUIDEx, Pair> UploadRaws
        {
            get
            {
                object obj = this.ViewState["ListUploadRaw"];
                return obj == null ? new Dictionary<GUIDEx, Pair>() : (Dictionary<GUIDEx, Pair>)obj;
            }
            set
            {
                this.ViewState["ListUploadRaw"] = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileID"></param>
        /// <param name="contentType"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        protected void SaveFileRawToPage(GUIDEx fileID, string contentType, Stream stream)
        {
            lock (this)
            {
                if (fileID.IsValid && stream != null && stream.Length > 0)
                {
                    string strData = null;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (GZipStream zipStream = new GZipStream(ms, CompressionMode.Compress))
                        {
                            byte[] buf = new byte[256];
                            int len = 0;
                            while ((len = stream.Read(buf, 0, buf.Length)) > 0)
                            {
                                zipStream.Write(buf, 0, len);
                            }
                            zipStream.Close();
                            stream.Close();
                        }
                        byte[] data = ms.ToArray();
                        ms.Close();
                        strData = iPower.Utility.HexParser.ToHexString(data);
                    }
                    if (!string.IsNullOrEmpty(strData))
                    {
                        Dictionary<GUIDEx, Pair> dicRaws = this.UploadRaws;
                        dicRaws.Add(fileID, new Pair(contentType, strData));
                        this.UploadRaws = dicRaws;
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileID"></param>
        protected bool DeleteFileRawToPage(GUIDEx fileID)
        {
            lock (this)
            {
                bool result = false;
                if (fileID.IsValid)
                {
                    Dictionary<GUIDEx, Pair> dicRaws = this.UploadRaws;
                    if (dicRaws.ContainsKey(fileID))
                    {
                        dicRaws.Remove(fileID);
                        this.UploadRaws = dicRaws;
                        result = true;
                    }
                }
                return result;
            }
        }
        #endregion

        #region 数据绑定。
        /// <summary>
        /// 获取或设置文件ID字段名称。
        /// </summary>
        public string FileIDField
        {
            get
            {
                object obj = this.ViewState["FileIDField"];
                return obj == null ? "FileID" : (string)obj;
            }
            set
            {
                this.ViewState["FileIDField"] = value;
            }
        }
        /// <summary>
        /// 获取或设置文件字段名称。
        /// </summary>
        public string FileNameField
        {
            get
            {
                object obj = this.ViewState["FileNameField"];
                return obj == null ? "FileName" : (string)obj;
            }
            set
            {
                this.ViewState["FileNameField"] = value;
            }
        }
        /// <summary>
        /// 获取或设置扩展名字段名称。
        /// </summary>
        public string ExtensionField
        {
            get
            {
                object obj = this.ViewState["ExtensionField"];
                return obj == null ? "Extension" : (string)obj;
            }
            set
            {
                this.ViewState["ExtensionField"] = value;
            }
        }
        /// <summary>
        /// 获取或设置文件大小字段名称。
        /// </summary>
        public string SizeField
        {
            get
            {
                object obj = this.ViewState["SizeField"];
                return obj == null ? "Size" : (string)obj;
            }
            set
            {
                this.ViewState["SizeField"] = value;
            }
        }
        StringCollection FileIDFromDataSource
        {
            get
            {
                object obj = this.ViewState["FileIDFromDataSource"];
                return obj == null ? new StringCollection() : (StringCollection)obj;
            }
            set
            {
                this.ViewState["FileIDFromDataSource"] = value;
            }
        }
        /// <summary>
        /// 构建数据。
        /// </summary>
        public void BuildUploadView()
        {
            object dataSource = this.DataSource;
            if (dataSource != null)
            {
                this.Items.Clear();
                if (dataSource is ICollection)
                    this.BuildItems((ICollection)dataSource);
                else if (dataSource is DataTable)
                    this.BuildItems((DataTable)dataSource);
                this.DataBind();
            }
        }
        void BuildItems(ICollection dataSource)
        {
            if (dataSource != null)
            {
                IEnumerator enumerator = dataSource.GetEnumerator();
                PropertyDescriptorCollection pdc = null;
                PropertyDescriptor pd = null;

                StringCollection allFileID = new StringCollection();
                while (enumerator.MoveNext())
                {
                    object obj = enumerator.Current;
                    if (obj != null)
                    {
                        pdc = TypeDescriptor.GetProperties(obj);
                        if (pdc != null && pdc.Count > 0)
                        {
                            bool flag = false;
                            UploadViewDataItem item = new UploadViewDataItem();
                            pd = pdc.Find(this.FileIDField, true);
                            if (pd != null)
                            {
                                item.FileID = new GUIDEx(pd.GetValue(obj));
                                allFileID.Add(item.FileID);
                                flag = true;
                            }
                            pd = pdc.Find(this.FileNameField, true);
                            if (pd != null)
                            {
                                item.FileName = Convert.ToString(pd.GetValue(obj));
                                flag &= true;
                            }
                            pd = pdc.Find(this.ExtensionField, true);
                            if (pd != null)
                            {
                                item.Extension = Convert.ToString(pd.GetValue(obj));
                                flag &= true;
                            }
                            pd = pdc.Find(this.SizeField, true);
                            if (pd != null)
                                item.Size = Convert.ToDouble(pd.GetValue(obj));
                            if (flag)
                                this.Items.Add(item);
                        }
                    }
                }
                this.FileIDFromDataSource = allFileID;
            }
        }
        void BuildItems(DataTable dtSource)
        {
            if (dtSource != null)
            {
                StringCollection allFileID = new StringCollection();
                foreach (DataRow row in dtSource.Rows)
                {
                    UploadViewDataItem item = new UploadViewDataItem();
                    item.FileID = new GUIDEx(row[this.FileIDField]);
                    allFileID.Add(item.FileID);
                    item.FileName = Convert.ToString(row[this.FileNameField]);
                    item.Extension = Convert.ToString(row[this.ExtensionField]);
                    item.Size = Convert.ToDouble(row[this.SizeField]);
                    this.Items.Add(item);
                }
                this.FileIDFromDataSource = allFileID;
            }
        }
        #endregion

        #region 数据保存。
        /// <summary>
        /// 保存上传文件。
        /// </summary>
        /// <param name="handler"></param>
        public void SaveUploadAs(EventHandler<UploadViewEventArgs> handler)
        {
            try
            {
                if (handler != null)
                {
                    Dictionary<GUIDEx, Pair> dicRaws = this.UploadRaws;
                    if (dicRaws != null && dicRaws.Count > 0)
                    {
                        UploadViewDataItem item = null;
                        foreach (KeyValuePair<GUIDEx, Pair> kvp in dicRaws)
                        {
                            item = this.Items[kvp.Key];
                            if (item != null)
                            {
                                Pair p = kvp.Value;
                                byte[] data = iPower.Utility.HexParser.Parse(p.Second.ToString());
                                if (data != null && data.Length > 0)
                                {
                                    using (MemoryStream ms = new MemoryStream())
                                    {
                                        using (GZipStream zipStream = new GZipStream(new MemoryStream(data), CompressionMode.Decompress))
                                        {
                                            byte[] buf = new byte[256];
                                            int len = 0;
                                            while ((len = zipStream.Read(buf, 0, buf.Length)) > 0)
                                            {
                                                ms.Write(buf, 0, len);
                                            }
                                            zipStream.Close();
                                        }
                                        byte[] d = ms.ToArray();
                                        ms.Close();
                                        if (d != null && d.Length > 0)
                                            handler(this, new UploadViewEventArgs(new UploadViewDataItemRaw(item, p.First.ToString(), d)));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                this.OnUploadViewExceptionEvent(e);
            }
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class UploadViewEventArgs : EventArgs
    {
        #region 成员变量，构造函数。
        UploadViewDataItemRaw itemRaw;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public UploadViewEventArgs(UploadViewDataItemRaw itemRaw)
        {
            this.itemRaw = itemRaw;
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        public UploadViewDataItemRaw ItemRaw
        {
            get { return this.itemRaw; }
        }
    }
}
