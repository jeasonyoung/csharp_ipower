//================================================================================
//  FileName: ControlExportData.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/12/23
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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace iPower.Web.UI
{
    /// <summary>
    /// 导出控件数据。
    /// </summary>
    public class ControlExportData
    {
        #region 成员变量，构造函数。
        Control control = null;
        string exportFileName;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="control">需导出的控件。</param>
        /// <param name="exportFileName">导出的文件名称。</param>
        public ControlExportData(Control control, string exportFileName)
        {
            if (control == null)
                throw new ArgumentNullException("ctrl");
            this.control = control;
            this.exportFileName = exportFileName;
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="ctrol">需导出的控件。</param>
        public ControlExportData(Control ctrol)
            : this(ctrol, null)
        {
        }
        #endregion

        /// <summary>
        /// 获取或设置下载文件保存的文件名。
        /// </summary>
        public string ExportFileName
        {
            get
            {
                return string.IsNullOrEmpty(this.exportFileName) ?
                    DateTime.Now.ToString("yyyyMMddHH") : this.exportFileName;
            }
            set
            {
                this.exportFileName = value;
            }
        }
         
        /// <summary>
        /// 导出内容。
        /// </summary>
        /// <returns></returns>
        protected virtual StringWriter ExportContent()
        {
            StringWriter sw = new StringWriter();
            if (this.control != null)
            {
                HtmlTextWriter writer = new HtmlTextWriter(sw);
                this.control.RenderControl(writer);
            }
            return sw;
        }
        /// <summary>
        /// 下载文件。
        /// </summary>
        public void ExportDownload()
        {
            Page page = this.control.Page;
            if (page == null)
                throw new ArgumentException("Page");
            HttpResponse resp = page.Response;

            resp.Clear();
            resp.Buffer = true;
            resp.Charset = "gb2312";
            //resp.ContentType = "application/OCTET-STREAM;";
            resp.ContentType = "application/ms-excel";
            resp.ContentEncoding = Encoding.GetEncoding("gb2312");//设置输出流为简体中文
            resp.AddHeader("Content-Disposition",
                string.Format("attachment;filename={0}.xls",this.ExportFileName));

            StringWriter sw = this.ExportContent();
            if (sw != null)
            {
                resp.Write(sw.ToString());
            }
            resp.Flush();
            resp.End();
        }
    }
}
