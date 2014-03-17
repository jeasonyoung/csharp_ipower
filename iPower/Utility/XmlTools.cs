//================================================================================
//  FileName: XmlTools.cs
//  Desc:Xml工具类。
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2009-10-29
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
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.IO;
namespace iPower.Utility
{
    /// <summary>
    /// Xml工具类。
    /// </summary>
    public static class XmlTools
    {
        #region 创建Xml节点。
        /// <summary>
        /// 创建Xml节点。
        /// </summary>
        /// <param name="doc">XmlDocument<see cref="XmlDocument"/>。</param>
        /// <param name="parent">父节点。</param>
        /// <param name="elemName">节点名。</param>
        /// <param name="elemValue">节点值。</param>
        /// <param name="isCData">是否为CDATA。</param>
        /// <returns>Xml节点。</returns>
        public static XmlElement CreateXmlElement(ref XmlDocument doc, ref XmlElement parent, string elemName, string elemValue, bool isCData)
        {
            if (!string.IsNullOrEmpty(elemName))
            {
                XmlElement elem = doc.CreateElement(elemName);
                if (isCData)
                    elem.AppendChild(doc.CreateCDataSection(elemValue));
                else if(!Guard.ArgumentNotNullOrEmptyString("elemValue",elemValue, false))
                    elem.AppendChild(doc.CreateTextNode(elemValue));
                parent.AppendChild(elem);
                return elem;
            }
            return null;
        }
        /// <summary>
        /// 创建Xml节点。
        /// </summary>
        /// <param name="doc">XmlDocument。</param>
        /// <param name="parent">父节点。</param>
        /// <param name="elemName">节点名。</param>
        /// <param name="elemValue">节点值。</param>
        /// <returns>Xml节点。</returns>
        public static XmlElement CreateXmlElement(ref XmlDocument doc, ref XmlElement parent, string elemName, string elemValue)
        {
            return CreateXmlElement(ref doc, ref parent, elemName, elemValue, false);
        }

        /// <summary>
        /// 创建Xml节点。
        /// </summary>
        /// <param name="doc">XmlDocument。</param>
        /// <param name="parent">父节点。</param>
        /// <param name="elemName">节点名。</param>
        /// <returns>Xml节点。</returns>
        public static XmlElement CreateXmlElement(ref XmlDocument doc, ref XmlElement parent, string elemName)
        {
            return CreateXmlElement(ref doc, ref parent, elemName, null);
        }
        /// <summary>
        /// 创建Xml节点。
        /// </summary>
        /// <param name="doc">XmlDocument。</param>
        /// <param name="parent">父节点。</param>
        /// <param name="elemName">节点名。</param>
        /// <param name="elemValue">节点值。</param>
        /// <returns>Xml节点。</returns>
        public static XmlElement CreateCDATAElement(ref XmlDocument doc, ref XmlElement parent, string elemName, string elemValue)
        {
            if (!string.IsNullOrEmpty(elemName))
                return CreateXmlElement(ref doc, ref parent, elemName, elemValue, true);
            return null;
        }
        #endregion

        #region 创建属性。
        /// <summary>
        /// 创建节点属性。
        /// </summary>
        /// <param name="doc">XmlDocument。</param>
        /// <param name="elem">节点。</param>
        /// <param name="attrName">属性名。</param>
        /// <param name="attrValue">属性值。</param>
        /// <returns>Xml节点。</returns>
        public static XmlAttribute CreateAttribute(ref XmlDocument doc, ref XmlElement elem, string attrName, string attrValue)
        {
            if (!string.IsNullOrEmpty(attrName))
            {
                XmlAttribute attr = doc.CreateAttribute(attrName);
                attr.Value = attrValue;
                elem.Attributes.Append(attr);
                return attr;
            }
            return null;
        }
        #endregion

        #region 工具类。
        /// <summary>
        /// 将Xml转化为Html。
        /// </summary>
        /// <param name="doc">XmlDocument。</param>
        /// <param name="xsltPath">XsltPath。</param>
        public static string XmlAndXsltToHtml(XmlDocument doc, string xsltPath)
        {
            string result = string.Empty;
            if (doc != null && !string.IsNullOrEmpty(xsltPath) && File.Exists(xsltPath))
            {
                StringBuilder output = new StringBuilder();
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = "\t";
                settings.Encoding = Encoding.Default;
                settings.ConformanceLevel = ConformanceLevel.Auto;

                XmlWriter writer = XmlWriter.Create(output, settings); 
                try
                {
                    XslCompiledTransform transform = new XslCompiledTransform();
                    transform.Load(xsltPath);
                    transform.Transform(doc, writer);
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    writer.Close();
                }

                if (output.Length > 0)
                {
                    result = output.ToString();
                    int index = result.IndexOf("?>");
                    if (index > 0)
                        result = result.Substring(index + 2);
                }
            }
            return result;
        }
        /// <summary>
        /// 将Xml转化为Html。
        /// </summary>
        /// <param name="doc">XmlDocument.</param>
        /// <param name="xsltStream">Xslt流数据。</param>
        /// <returns></returns>
        public static string XmlAndXsltToHtml(XmlDocument doc, Stream xsltStream)
        {
            string result = string.Empty;
            if (doc != null && xsltStream != null)
            {
                StringBuilder output = new StringBuilder();
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = "\t";
                settings.Encoding = Encoding.Default;
                settings.ConformanceLevel = ConformanceLevel.Auto;

                XmlWriter writer = XmlWriter.Create(output, settings);
                try
                {

                    XslCompiledTransform transform = new XslCompiledTransform();
                    transform.Load(new XPathDocument(xsltStream));
                    transform.Transform(doc, writer);
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    writer.Close();
                }

                if (output.Length > 0)
                {
                    result = output.ToString();
                    int index = result.IndexOf("?>");
                    if (index > 0)
                        result = result.Substring(index + 2);
                }
            }
            return result;
        }
        #endregion
    }
}
