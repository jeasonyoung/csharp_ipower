//================================================================================
//  FileName: ExportDataSource.cs
//  Desc:数据导出。
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
using System.Data;
using System.IO;

using iPower.Web.UI;
namespace iPower.Web.Utility
{
    /// <summary>
    /// 数据导出。
    /// </summary>
    public class ExportDataSource : ExportData
    {
        #region 成员变量，构造函数，析构函数。
        DataTable dtSource;
        GridView gView;
        Page thePage;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="thePage"></param>
        /// <param name="dtSource"></param>
        public ExportDataSource(Page thePage, DataTable dtSource)
            : base(thePage)
        {
            this.thePage = thePage;
            this.dtSource = dtSource;
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="gView"></param>
        /// <param name="dtSource"></param>
        public ExportDataSource(GridView gView, DataTable dtSource)
            : base(gView)
        {
            this.gView = gView;
            this.thePage = this.gView.Page;
            this.dtSource = dtSource;
        }
        #endregion

        #region 重载。
        /// <summary>
        /// 下载文件
        /// </summary>
        public new void Download()
        {
            if (this.thePage == null)
                throw new ArgumentException("Page");
            HttpResponse resp = this.thePage.Response;

            resp.Clear();
            resp.Buffer = true;
            resp.Charset = "gb2312";
            resp.AddHeader("Content-Disposition",
                //string.Format("attachment;filename={0}.{1}",
                string.Format("online;filename={0}.{1}",
                this.DownloadFileName,
                this.ExportFileType));
            resp.ContentEncoding = Encoding.GetEncoding("gb2312");//设置输出流为简体中文
            //resp.ContentType = this.ExportFileType == ExportFileType.txt ?
            //    "application/OCTET-STREAM;" : "application/vnd.ms-excel";
            resp.ContentType = "application/OCTET-STREAM;";

            StringWriter sw = this.Download(this.ExportFileType);
            if (sw != null)
                resp.Write(sw.ToString());
            resp.Flush();
            resp.End();
        }
        /// <summary>
        /// 导出数据类型。
        /// </summary>
        /// <param name="fileType"></param>
        /// <returns></returns>
        protected override System.IO.StringWriter Download(ExportFileType fileType)
        {
            if (this.gView == null)
                return this.ExportDataSourceData(this.dtSource, fileType);
            else
                return this.Export(this.gView, fileType, this.dtSource);
        }
        #endregion

        #region 导出给定数据源的全部数据。
        /// <summary>
        /// 导出数据。
        /// </summary> 
        protected virtual StringWriter Export(GridView gView, ExportFileType fileType, DataTable dtSource)
        {
            StringWriter sw = new StringWriter();
            if (gView != null)
            {
                string split = (fileType == ExportFileType.txt ? "\t" : ",");
                Dictionary<string, string> dataInfo;
                sw.WriteLine("<table border=\"1\">");
                this.ExportHeader(gView.Columns, split, ref sw, out dataInfo);
                this.ExportSourceData(dtSource, dataInfo, split, ref sw);
                sw.WriteLine("</table>");
            }
            return sw;
        }
        /// <summary>
        /// 导出头。
        /// </summary>
        protected void ExportHeader(DataControlFieldCollection colums, string split, ref StringWriter swHeader, out Dictionary<string, string> dataInfo)
        {
            dataInfo = new Dictionary<string, string>();
            if (colums != null)
            {
                BoundField field;
                //int i = 0;
                string /*headName,*/ datafiled;
                //StringBuilder buidler = new StringBuilder();
                swHeader.WriteLine("<tr>");
                foreach (DataControlField col in colums)
                {
                    field = col as BoundField;
                    if (field != null && !(field is CheckBoxFieldEx) && field.Visible)
                    {
                        datafiled = field.DataField;
                        swHeader.WriteLine(string.Format("<td>{0}</td>", field.HeaderText));
                        //headName = field.HeaderText;
                        //if (split.IndexOf(",") > -1)
                        //    headName = ModuleUtil.StringReplace(headName, ",", "，");
                        //buidler.AppendFormat(string.Format("{0}{1}",
                        //    i == 0 ? string.Empty : split, headName));

                        if (!dataInfo.ContainsKey(datafiled))
                            dataInfo.Add(datafiled, field.DataFormatString);
                        //i++;
                    }
                }
                swHeader.WriteLine("</tr>");
                //swHeader.WriteLine(buidler.ToString());
            }
        }
        /// <summary>
        /// 数据体。
        /// </summary> 
        protected void ExportSourceData(DataTable dtSource, Dictionary<string, string> dataInfo, string split, ref StringWriter swBody)
        {
            if (dataInfo != null && dtSource != null)
            {
                foreach (string dataName in dataInfo.Keys)
                {
                    if (!string.IsNullOrEmpty(dataName) && !dtSource.Columns.Contains(dataName))
                    {
                        throw new ArgumentNullException(string.Format("字段{0}在数据源中不存在!", dataName));
                    }
                }

                //int index;
                //StringBuilder builder;
                string format/*, data*/;
                foreach (DataRow row in dtSource.Rows)
                {
                    //builder = new StringBuilder();
                    //index = 0;
                    swBody.WriteLine("<tr>");
                    foreach (KeyValuePair<string, string> filed in dataInfo)
                    {

                        //builder.AppendFormat("{0}", index == 0 ? string.Empty : split);
                        format = filed.Value;
                        if (string.IsNullOrEmpty(format))
                            swBody.WriteLine(string.Format("<td>{0}</td>", row[filed.Key]));
                        else
                            swBody.WriteLine(string.Format("<td>{0}</td>", string.Format(format, row[filed.Key])));
                        //data = ModuleUtil.StringReplace(data, ",", "，");
                        //builder.Append(data.Trim());
                        //index++;
                    }
                    swBody.WriteLine("</tr>");
                    //swBody.WriteLine(builder.ToString());
                }
            }
        }
        #endregion

        #region 静态函数。
        /// <summary>
        /// 导出控件数据为Excel表格。
        /// </summary>
        /// <param name="page"></param>
        /// <param name="ctl"></param>
        public static void ExportDataFromControl(Page page, Control ctl)
        {
            lock (typeof(ExportDataSource))
            {
                if (ctl != null && page != null)
                {
                    StringBuilder sbContent = new StringBuilder();
                    StringWriter sWriter = new StringWriter(sbContent);
                    HtmlTextWriter writer = new HtmlTextWriter(sWriter);
                    ctl.RenderControl(writer);
                    page.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0:yyyyMMddHHmmss}.xls", DateTime.Now));
                    page.Response.Charset = "UTF-8";
                    page.Response.ContentEncoding = System.Text.Encoding.UTF8;
                    page.Response.ContentType = "application/ms-excel";

                    page.Response.Write(sbContent.ToString());
                    page.Response.Flush();
                    page.Response.Close();
                }
            }
        }
        #endregion
    }
}
