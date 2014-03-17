//================================================================================
//  FileName: ExportData.cs
//  Desc:�������ݡ�
//
//  Called by
//
//  Auth:���£�jeason1914@gmail.com��
//  Date: 2009-11-17
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
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Text;
namespace iPower.Web.UI
{
    /// <summary>
    /// �������ݡ�
    /// </summary>
    public class ExportData
    {
        #region ��Ա���������캯��������������
        string downloadFileName;
        ExportFileType exportFileType;
        DataGridView gridView;
        Page page;
        /// <summary>
        /// ���캯����
        /// </summary>
        /// <param name="gridView">gridView<see cref="DataGridView"/>��</param>
        public ExportData(DataGridView gridView)
            : this(gridView.Page)
        {
            if (gridView == null)
                throw new ArgumentNullException("gridView");
            this.gridView = gridView;
        }
        /// <summary>
        /// ���캯����
        /// </summary>
        /// <param name="page">page<see cref="Page"/>��</param>
        public ExportData(Page page)
        {
            if (page == null)
                throw new ArgumentNullException("page");
            this.page = page;
            this.exportFileType = ExportFileType.csv;
        }
        #endregion

        #region ���ԡ�
        /// <summary>
        /// �����ļ�������ļ�����
        /// </summary>
        public string DownloadFileName
        {
            get
            {
                return string.IsNullOrEmpty(this.downloadFileName) ?
                    DateTime.Now.ToString("yyyyMMddHH") : this.downloadFileName;
            }
            set
            {
                this.downloadFileName = value;
            }
        }
        /// <summary>
        /// �����ļ������͡�
        /// </summary>
        public ExportFileType ExportFileType
        {
            get
            {
                return this.exportFileType;
            }
            set
            {
                this.exportFileType = value;
            }
        }
        #endregion

        #region �����ļ�
        /// <summary>
        /// �����ļ���
        /// </summary>
        public void Download()
        {
            if (this.page == null)
                throw new ArgumentException("Page");
            HttpResponse resp = this.page.Response;

            resp.Clear();
            resp.Buffer = true;
            resp.Charset = "gb2312";
            resp.AddHeader("Content-Disposition",
                string.Format("attachment;filename={0}.{1}",
                this.DownloadFileName,
                this.ExportFileType));
            resp.ContentEncoding = Encoding.GetEncoding("gb2312");//���������Ϊ��������
            resp.ContentType = "application/OCTET-STREAM;";

            StringWriter sw = this.Download(this.ExportFileType);
            if (sw != null)
                resp.Write(sw.ToString());
            resp.Flush();
            resp.End();
        }
        /// <summary>
        /// ��������ʵ�֡�
        /// </summary>
        /// <param name="fileType"></param>
        /// <returns></returns>
        protected virtual StringWriter Download(ExportFileType fileType)
        {
            if (this.gridView != null)
                return this.Export(this.gridView, fileType);
            return null;
        }
        #endregion

        #region ����������
        /// <summary>
        /// �������ݡ�
        /// </summary>
        /// <returns></returns>
        public virtual StringWriter Export(DataGridView gView, ExportFileType fileType)
        {
            StringWriter sw = new StringWriter();
            if (gView != null)
            {
                string split = (fileType == ExportFileType.txt ? "\t" : ",");
                DataControlFieldExCollection columns = gView.Columns;
                sw.WriteLine(this.RemoveHtmlTag(this.ExportHeader(columns, split)));

                DataGridViewRowCollection rows = gView.Rows;
                foreach (DataGridViewRow row in rows)
                    sw.WriteLine(this.RemoveHtmlTag(this.ExportItem(row, columns, split)));
            }
            return sw;
        }
        /// <summary>
        /// �������ݡ�
        /// </summary>
        protected StringWriter ExportDataSourceData(DataTable dtSource, ExportFileType fileType)
        {
            StringWriter sw = new StringWriter();
            if (dtSource != null)
            {
                string split = (fileType == ExportFileType.txt ? "\t" : ",");
                string strRowData = string.Empty;
                //������
                foreach (DataColumn col in dtSource.Columns)
                {
                    if (string.IsNullOrEmpty(strRowData))
                        strRowData = col.ColumnName;
                    else
                        strRowData += string.Format("{0}{1}", split, col.ColumnName);
                }
                sw.WriteLine(strRowData.Replace("\r\n", " "));
                //������
                foreach (DataRow dr in dtSource.Rows)
                {
                    strRowData = string.Empty;
                    foreach (DataColumn col in dtSource.Columns)
                    {
                        if (string.IsNullOrEmpty(strRowData))
                            strRowData = this.CellDataFormat(dr[col].ToString().Replace(",", "��"));
                        else
                            strRowData += string.Format("{0}{1}", split, this.CellDataFormat(dr[col].ToString().Replace(",", "��")));
                    }
                    sw.WriteLine(this.RemoveHtmlTag(strRowData.Replace("\r\n", " ")));
                }
            }
            return sw;
        }
        /// <summary>
        /// ���õ�������split�ֶ�֮��ķָ����š�
        /// </summary>
        /// <param name="colums">DataControlFieldCollection<see cref="DataControlFieldCollection"/>��</param>
        /// <param name="split">�ָ�����</param>
        /// <returns>�������⡣</returns>
        protected string ExportHeader(DataControlFieldExCollection colums, string split)
        {
            StringBuilder strRowData = new StringBuilder();
            if (colums != null)
            {
                foreach (DataControlFieldEx col in colums)
                {
                    if (!(col is CheckBoxFieldEx) && col.Visible)
                        strRowData.AppendFormat("{0}{1}", split, col.HeaderText);
                }
            }
            if (strRowData.Length > 0)
                strRowData.Remove(0, 1);
            return strRowData.ToString();
        }
        /// <summary>
        /// ���������塣
        /// </summary>
        /// <param name="row">�����С�</param>
        /// <param name="columns">�������ϡ�</param>
        /// <param name="split">�ָ�����</param>
        /// <returns>�����ַ�����</returns>
        protected string ExportItem(DataGridViewRow row, DataControlFieldExCollection columns, string split)
        {
            StringBuilder strRowData = new StringBuilder();
            if (row != null && columns != null)
            {
                DataGridViewRowState rowState = row.RowState;
                if ((rowState == DataGridViewRowState.Normal) ||  (rowState == DataGridViewRowState.AlterNate))
                {
                    int index = 0;
                    string strValue = string.Empty;
                    foreach (DataControlFieldEx ctrlField in columns)
                    {
                        if (ctrlField.Visible)
                        {
                            if (ctrlField is MultiQueryStringFieldEx)
                            {
                                string result = string.Empty;
                                if (row.Cells[index].Controls[0] is HtmlAnchor)
                                    result = ((HtmlAnchor)row.Cells[index].Controls[0]).InnerText;
                                strValue = result.Replace("\r\n", string.Empty).Replace(',', '��');
                                strValue = this.CellDataFormat(strValue);
                                strRowData.AppendFormat("{0}{1}", split, strValue);
                            }
                            else if (!(ctrlField is CheckBoxFieldEx))
                            {
                                strValue = row.Cells[index].Text.Replace("\r\n", string.Empty).Replace(',', '��');
                                strValue = this.CellDataFormat(strValue);
                                strRowData.AppendFormat("{0}{1}", split, strValue);
                            }
                           
                        }
                        index++;

                    }

                }
            }
            if (strRowData.Length > 0)
                strRowData.Remove(0, 1);
            return strRowData.ToString();
        }
        /// <summary>
        /// ȥ��HTML��ǡ�
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        protected string RemoveHtmlTag(string str)
        {
            str = Regex.Replace(str, @"<(.|\n)+?>", "", RegexOptions.IgnoreCase);
            str = str.Replace("&nbsp;", "");
            str = str.Replace("&uarr;", "");
            str = str.Replace("&darr;", "");

            return str;
        }
        static Regex regex = new Regex(@"^\d+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// ���ݸ�ʽ����
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual string CellDataFormat(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (regex.IsMatch(value))
                {
                    value = string.Format("'{0}", value);
                }
            }
            return value;
        }
        #endregion
    }
}
